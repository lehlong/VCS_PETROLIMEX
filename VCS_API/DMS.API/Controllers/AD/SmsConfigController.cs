using Common;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using DMS.BUSINESS.Services.AD;
using DMS.CORE.Entities.AD;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.AD
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsConfigController(IAccountService service) : ControllerBase
    {
        public readonly IAccountService _service = service;
        [HttpGet("GetSMS")]
        public async Task<IActionResult> GetSMS()
        {
            var transferObject = new TransferObject();
            var result = await _service.GetSMS();
            if (_service.Status)
            {
                transferObject.Data = result;
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("2000", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("UpdateSMS")]
        public async Task<IActionResult> UpdateSMS([FromBody] TblAdSmsConfig data)
        {
            var transferObject = new TransferObject();
            await _service.UpdateSMS(data);
            if (_service.Status)
            {
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("2000", _service);
            }
            return Ok(transferObject);
        }
    }
}
