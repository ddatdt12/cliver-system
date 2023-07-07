using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.Order;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Error;
using CliverSystem.Models;
using CliverSystem.Models.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Controllers.Seller
{

    [Route("api/seller/orders")]
    [ApiController]
    [Protect]
    public class SellerOrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SellerOrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces(typeof(ApiResponse<IEnumerable<OrderDto>>))]
        public async Task<IActionResult> GetOrders(OrderStatus? status)
        {
            var user = HttpContext.Items["User"] as User;
            var orders = await _unitOfWork.Orders.GetOrders(user!.Id, status, Common.Enum.Mode.Seller);
            IEnumerable<OrderDto> orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);

            return Ok(new ApiResponse<IEnumerable<OrderDto>>(orderDtos, "Get seller orders successfully"));
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces(typeof(ApiResponse<OrderDto>))]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _unitOfWork.Orders.GetOrderById(id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            return Ok(new ApiResponse<OrderDto>(_mapper.Map<OrderDto>(order), "Get order by id successfully"));
        }

        [HttpPost]
        [Route("{orderId}/action")]
        public async Task<IActionResult> DoActionWithOrder(int orderId, [FromBody] OrderActionDto data)
        {
            var userId = HttpContext.Items["UserId"] as string;
            switch (data.Action)
            {
                case OrderActionType.Start:
                    await _unitOfWork.Orders.StartOrder(orderId, userId!);
                    break;
                case OrderActionType.Cancel:
                    await _unitOfWork.Orders.CancelOrder(orderId, userId!, Mode.Seller);
                    break;  
                case OrderActionType.Delivery:
                    await _unitOfWork.Orders.DeliveryOrder(orderId, userId!, data.Resource);
                    break;
                default:
                    throw new ApiException("Invalid Action", 400);
            }

            return NoContent();
        }



        [HttpPost]
        [Route("{orderId}/reviews")]
        [Produces(typeof(ApiResponse<ReviewDto>))]
        public async Task<IActionResult> ReviewOrder(int orderId, [FromBody] CreateReviewDto createDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            createDto.OrderId = orderId;
            var review = await _unitOfWork.Reviews.CreateReview(orderId, userId!, createDto, Mode.Seller);

            var reviewDto = _mapper.Map<ReviewDto>(review);
            return Ok(new ApiResponse<ReviewDto>(reviewDto, "Review succesfully"));
        }
    }
}
