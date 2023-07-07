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

namespace CliverSystem.Controllers.Seller
{
    [Route("api/seller/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [Protect]
        [Produces(typeof(ApiResponse<IEnumerable<PostDto>>))]
        public async Task<IActionResult> Get([FromQuery] PostParameters postParam)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var posts = await _unitOfWork.Posts.GetPostsByUser(userId!, postParam);

            var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);
            return Ok(new ApiResponse<IEnumerable<PostDto>>(postDtos, "Get posts successfully", posts.MetaData));
        }
        [HttpPut("{id}/status")]
        [Protect]
        [Produces(typeof(ApiResponse<IEnumerable<PostDto>>))]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] PostStatus status)
        {
            var userId = HttpContext.Items["UserId"] as string;
            await _unitOfWork.Posts.UpdateStatus(id, userId!, status);

            return NoContent();
        }
    }
}
