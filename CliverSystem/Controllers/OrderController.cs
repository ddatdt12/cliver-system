using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [Authorize]
        [HttpGet]
        [Produces(typeof(ApiResponse<IEnumerable<OrderDto>>))]
        public async Task<IActionResult> GetOrders()
        {
            var user = HttpContext.Items["User"] as User;
            var orders = await _unitOfWork.Orders.GetOrders(user!.Id, Common.Enum.Mode.Buyer);
            IEnumerable<OrderDto> orderDtos = _mapper.Map<IEnumerable<OrderDto>>(orders);

            return Ok(new ApiResponse<IEnumerable<OrderDto>>(orderDtos, "Get buyer orders successfully"));
        }
    }
}
