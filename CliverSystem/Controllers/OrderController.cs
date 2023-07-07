using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.Order;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Error;
using CliverSystem.Models;
using CliverSystem.Models.Settings;
using CliverSystem.Services.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Protect]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IPaymentService _paymentService;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper, IPaymentService paymentService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentService = paymentService;
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(ApiResponse<IEnumerable<OrderDto>>))]
        public async Task<IActionResult> GetOrders(OrderStatus? status)
        {
            var user = HttpContext.Items["User"] as User;
            var orders = await _unitOfWork.Orders.GetOrders(user!.Id, status, Common.Enum.Mode.Buyer);
            IEnumerable<OrderDto> orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);

            return Ok(new ApiResponse<IEnumerable<OrderDto>>(orderDtos, "Get buyer orders successfully"));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        [HttpGet("custom-package/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Produces(typeof(ApiResponse<OrderDto>))]
        public async Task<IActionResult> GetOrderByCustomPackage(int id)
        {
            var order = await _unitOfWork.Orders.GetOrderByCustomPackageId(id);

            if (order == null)
            {
                return NotFound("Order not found");
            }

            return Ok(new ApiResponse<OrderDto>(_mapper.Map<OrderDto>(order), "Get order by id successfully"));
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Produces(typeof(ApiResponse<OrderDto>))]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var user = HttpContext.Items["User"] as User;
            var newOrder = _mapper.Map<Order>(orderDto);
            newOrder.BuyerId = user!.Id;
            await _unitOfWork.Orders.InsertOrder(newOrder, PaymentMethod.MyWallet);

            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, newOrder);
        }

        [HttpGet("{orderId}/wallet-payment")]
        [Produces(typeof(string))]
        [Protect]
        public async Task<IActionResult> WalletPayment(int orderId)
        {
            var user = HttpContext.Items["User"] as User;
            await _unitOfWork.Orders.PaymentOrderByWallet(orderId, user.Id!);
            return NoContent();
        }
        [HttpGet("{orderId}/payment")]
        [ProducesResponseType(StatusCodes.Status308PermanentRedirect)]
        [Produces(typeof(string))]
        public async Task<IActionResult> Payment(int orderId)
        {
            var vnpayConfig = _configuration.GetSection(VnPayConfig.Name).Get<VnPayConfig>();

            var user = HttpContext.Items["User"] as User;
            var order = await _unitOfWork.Orders.FindById(orderId, false);

            if (order == null || user!.Id != order?.BuyerId)
            {
                throw new ApiException("Đơn hàng không hợp lệ", 400);
            }

            if (order.Status != OrderStatus.PendingPayment)
            {
                throw new ApiException("Đơn hàng đã thanh toán", 400);
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            string paymentUrl = _paymentService.CreateOrderPaymentUrl(vnpayConfig, ipAddress, order);

            return Ok(paymentUrl);
        }

        [HttpPost]
        [Route("{orderId}/action")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> DoActionWithOrder(int orderId, [FromBody] OrderActionDto action)
        {
            var userId = HttpContext.Items["UserId"] as string;
            switch (action.Action)
            {
                case OrderActionType.Cancel:
                    await _unitOfWork.Orders.CancelOrder(orderId, userId!, Mode.Buyer);
                    break;
                case OrderActionType.Receive:
                    await _unitOfWork.Orders.ReceiveOrder(orderId, userId!);
                    break;
                case OrderActionType.Revision:
                    await _unitOfWork.Orders.ReviseOrder(orderId, userId!);
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
            var review = await _unitOfWork.Reviews.CreateReview(orderId, userId, createDto, Mode.Buyer);

            var reviewDto = _mapper.Map<ReviewDto>(review);
            return Ok(new ApiResponse<ReviewDto>(reviewDto, "Review succesfully"));
        }
    }
}
