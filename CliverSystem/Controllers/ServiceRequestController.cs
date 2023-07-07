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
    [Route("api/service-requests")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ServiceRequestController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Produces(typeof(ApiResponse<IEnumerable<ServiceRequestDto>>))]
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.Items["UserId"] as string;
            var reqs = await _unitOfWork.ServiceRequests.GetServiceRequests(userId!, Mode.Buyer);

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

        [HttpPost]
        [Protect]
        [Produces(typeof(ApiResponse<ServiceRequestDto>))]
        public async Task<IActionResult> CreateServiceRequest([FromBody] CreateServiceRequestDto createReqDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var req = await _unitOfWork.ServiceRequests.CreateServiceRequest(userId!, createReqDto);

            var reqDto = _mapper.Map<ServiceRequestDto>(req);
            return new CreatedResult
            ("New Post", new ApiResponse<ServiceRequestDto>(reqDto, "Create service request successfully"));
        }

        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateServiceRequestDto updateDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            await _unitOfWork.ServiceRequests.UpdateServiceRequest(id, userId!, updateDto);
           
            return NoContent();
        }


        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        [Produces(typeof(NoContentResult))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Items["UserId"] as string;
            await _unitOfWork.ServiceRequests.DeleteServiceRequest(id, userId!);

            return NoContent();
        }

        // DELETE api/<PostController>/5
        [HttpGet("{id}/done")]
        [Produces(typeof(NoContentResult))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> MarkDone(int id)
        {
            var userId = HttpContext.Items["UserId"] as string;
            await _unitOfWork.ServiceRequests.MarkDoneServiceRequest(id, userId!);

            return NoContent();
        }
    }
}
