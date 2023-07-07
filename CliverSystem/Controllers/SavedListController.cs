using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.User;
using CliverSystem.Error;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CliverSystem.Controllers
{
    [Route("api/me/saved-lists")]
    [Protect]
    [ApiController]
    public class SavedListController : ControllerBase
    {
        public IUnitOfWork _unitOfWork { get; set; }
        public IMapper _mapper { get; set; }
        public SavedListController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? postId, string? sellerId)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var myLists = await _unitOfWork.SavedLists.GetMyLists(userId!, postId,sellerId);
            var results = _mapper.Map<IEnumerable<SavedListDto>>(myLists);
            return Ok(new ApiResponse<IEnumerable<SavedListDto>>(results, "Get saved list succesfully"));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSavedList([FromBody] CreateSavedListDto createSavedListDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var saveList = await _unitOfWork.SavedLists.CreateNewList(userId!, createSavedListDto.Name);
            var result = _mapper.Map<SavedListDto>(saveList);
            return new CreatedResult("Create Saved List", new ApiResponse<SavedListDto>(result, "create saved list succesfully"));
        }

        [HttpGet("services/recently")]
        public async Task<IActionResult> GetSavedServices()
        {
            var userId = HttpContext.Items["UserId"] as string;
            var services = await _unitOfWork.SavedLists.GetRecentlySavedServices(userId!);

            var servicesResult = _mapper.Map<IEnumerable<PostDto>>(services);
            return Ok(new ApiResponse<IEnumerable<PostDto>>(servicesResult, "Get saved services succesfully"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSavedList(int id, [FromBody] CreateSavedListDto createSavedListDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            await _unitOfWork.SavedLists.UpdateSavedList(id, userId!, createSavedListDto.Name);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSavedList(int id)
        {
            await _unitOfWork.SavedLists.DeleteList(id);
            return NoContent();
        }

        [HttpGet("{id}/services")]
        public async Task<IActionResult> GetSavedServices(int id)
        {
            var services = await _unitOfWork.SavedLists.GetSavedServices(id);

            var servicesResult = _mapper.Map<IEnumerable<PostDto>>(services);
            return Ok(new ApiResponse<IEnumerable<PostDto>>(servicesResult, "Get saved services succesfully"));
        }



        [HttpPost("{id}/services")]
        public async Task<IActionResult> SaveOrRemoveService(int id, [FromBody] UpdateSavedService updateData)
        {
            var userId = HttpContext.Items["UserId"] as string;
            await _unitOfWork.SavedLists.SaveOrRemoveService(id, updateData.ServiceId, userId!);
            return Ok(new ApiResponse<string>("Successfully", "Update saved service succesfully"));
        }

        [HttpGet("{id}/sellers")]
        public async Task<IActionResult> GetSavedSellers(int id)
        {
            var sellers = await _unitOfWork.SavedLists.GetSavedSellers(id);
            var sellersResult = _mapper.Map<IEnumerable<UserDto>>(sellers);
            return Ok(new ApiResponse<IEnumerable<UserDto>>(sellersResult, "Get saved sellers succesfully"));
        }

        [HttpPost("{id}/sellers")]
        public async Task<IActionResult> SaveOrRemoveSeller(int id, [FromBody] UpdateSavedSeller updateData)
        {
            var userId = HttpContext.Items["UserId"] as string;
            if (userId == updateData.SellerId)
            {
                throw new ApiException("Cannot save yourselft", 400);
            }
            await _unitOfWork.SavedLists.SaveOrRemoveSeller(id, updateData.SellerId, userId!);
            return Ok(new ApiResponse<string>("Successfully", "Update saved seller succesfully"));
        }
    }
}
