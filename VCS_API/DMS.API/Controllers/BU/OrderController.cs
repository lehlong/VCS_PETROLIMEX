using Microsoft.AspNetCore.Mvc;
using DMS.API.AppCode.Enum;
using DMS.API.AppCode.Extensions;
using Common;
using DMS.BUSINESS.Services.BU;
using DMS.BUSINESS.Dtos.BU;

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

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] OrderDto orderDto)
        {
            var transferObject = new TransferObject();
            var result = await _service.Add(orderDto);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0100", _service); // Thêm mới thành công
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service); // Thêm mới thất bại
            }
            return Ok(transferObject);
        }

        [HttpPut("UpdateOrderCall")]
        public async Task<IActionResult> UpdateOrderCall([FromBody] OrderUpdateDto orderDto)
        {
            var transferObject = new TransferObject();
            var result = await _service.UpdateOrderCall(orderDto);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service); // Thêm mới thành công
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpPut("UpdateOrderCome")] 
        public async Task<IActionResult> UpdateOrderCome([FromBody] OrderUpdateDto orderDto)
        {
            var transferObject = new TransferObject();
            var result = await _service.UpdateOrderCome(orderDto);
            if (_service.Status)
            {
                transferObject.Data = result;
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0103", _service); // Thêm mới thành công
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0104", _service);
            }
            return Ok(transferObject);
        }

        [HttpPost("Order")]
        public async Task<IActionResult> Order([FromBody] OrderDto orderDto)
        {
            var transferObject = new TransferObject();
            await _service.Order(orderDto);
            if (_service.Status)
            {
                transferObject.Status = true;
                transferObject.MessageObject.MessageType = MessageType.Success;
                transferObject.GetMessage("0100", _service); // Thêm mới thành công
            }
            else
            {
                transferObject.Status = false;
                transferObject.MessageObject.MessageType = MessageType.Error;
                transferObject.GetMessage("0101", _service); // Thêm mới thất bại
            }
            return Ok(transferObject);
        }
    }
}
