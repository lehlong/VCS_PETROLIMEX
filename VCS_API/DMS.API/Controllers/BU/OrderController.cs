using Common;
using DMS.API.AppCode.Enum;
using Microsoft.AspNetCore.Mvc;

namespace DMS.API.Controllers.BU
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        //[HttpGet("GetAll")]
        //public async Task<IActionResult> GetAll([FromQuery] BaseMdFilter filter)
        //{
        //    var transferObject = new TransferObject();
        //    var result = await _service.GetAll(filter);
        //    if (_service.Status)
        //    {
        //        transferObject.Data = result;
        //    }
        //    else
        //    {
        //        transferObject.Status = false;
        //        transferObject.MessageObject.MessageType = MessageType.Error;
        //        transferObject.GetMessage("0001", _service);
        //    }
        //    return Ok(transferObject);
        //}

    }
}
