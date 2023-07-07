using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Error;
using CliverSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static CliverSystem.Common.Enum;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CliverSystem.Controllers
{
    [Route("api/seller/service-requests")]
    [ApiController]
    public class SellerServiceRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SellerServiceRequestController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces(typeof(ApiResponse<IEnumerable<ServiceRequestDto>>))]
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.Items["UserId"] as string;
           var reqs= await _unitOfWork.ServiceRequests.GetServiceRequests(userId: userId!, Mode.Seller);

            var reqDtos = _mapper.Map<IEnumerable<ServiceRequestDto>>(reqs);
            return Ok(new ApiResponse<IEnumerable<ServiceRequestDto>>(reqDtos, "Get service requests successfully"));
        }

        [HttpGet("{id}")]
        [Produces(typeof(ApiResponse<ServiceRequestDto>))]
        public async Task<IActionResult> GetDetail(int id)
        {
            var userId = HttpContext.Items["UserId"] as string;
           var req = await _unitOfWork.ServiceRequests.GetServiceRequestDetail(id, userId!);

            var reqDto = _mapper.Map<ServiceRequestDto>(req);
            return Ok(new ApiResponse<ServiceRequestDto>(reqDto, "Get service requests successfully"));
        }
    }
}
