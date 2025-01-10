using Microsoft.AspNetCore.Mvc;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using Common;
using DMS.BUSINESS.Services.BU;

namespace DMS.API.Controllers.BU
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService service) : ControllerBase
    {
        public readonly IOrderService _service = service;

        [HttpGet("GetOrder")]
        public async Task<IActionResult> GetOrder([FromQuery] BaseFilter filter)
        {
            var transferObject = new TransferObject();
            var result = await _service.GetOrder(filter);
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
       
    }
}
