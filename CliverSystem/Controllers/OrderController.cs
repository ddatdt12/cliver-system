using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CliverSystem.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [Protect]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(ApiResponse<IEnumerable<OrderDto>>))]
        public async Task<IActionResult> GetOrders()
        {
            var user = HttpContext.Items["User"] as User;
            var orders = await _unitOfWork.Orders.GetOrders(user!.Id, Common.Enum.Mode.Buyer);
            IEnumerable<OrderDto> orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);

            return Ok(new ApiResponse<IEnumerable<OrderDto>>(orderDtos, "Get buyer orders successfully"));
        }

        [Protect]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _unitOfWork.Orders.GetOrderById(id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            return Ok(new ApiResponse<OrderDto>(_mapper.Map<OrderDto>(order), "Get order by id successfully"));
        }

        [Protect]
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces(typeof(ApiResponse<IEnumerable<OrderDto>>))]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var user = HttpContext.Items["User"] as User;
            var newOrder = _mapper.Map<Order>(orderDto);
            newOrder.BuyerId = user!.Id;
            await _unitOfWork.Orders.InsertOrder(newOrder);

            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id}, newOrder);
        }
    }
}
