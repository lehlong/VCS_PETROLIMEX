using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Services.BU;
using DMS.BUSINESS.Services.MD;
using DMS.BUSINESS.Filter.BU;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.BU
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeaderController(IHeaderService service) : ControllerBase
    {
        public readonly IHeaderService _service = service;
        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] HeaderFilter filter)
        {
            var transferObject = new TransferObject();
            var result = await _service.Search(filter);
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0001", _service);
            }
            return Ok(transferObject);
        }
        [HttpGet("GetHistoryDetail")]
        public async Task<IActionResult> GetHistory([FromQuery] string headerId)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetHistoryDetail(headerId);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0100", _service);
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service);
            }
            return Ok(transferObject);
        }


        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImages([FromForm] IFormFileCollection files)
        {
            string _baseDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            if (files == null || files.Count == 0)
            {
                return Ok(new { Status = true, Message = "Không có ảnh!" });
            }

            string todayPath = Path.Combine(_baseDirectory, DateTime.Now.ToString("yyyy/MM/dd"));
            if (!Directory.Exists(todayPath))
            {
                Directory.CreateDirectory(todayPath);
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    string filePath = Path.Combine(todayPath, file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            return Ok(new {Status = true, Message = "Đẩy ảnh lên server thành công!" });
        }

    }
}
