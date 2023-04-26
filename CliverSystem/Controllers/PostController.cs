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
    [Route("api/posts")]
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
        [Produces(typeof(ApiResponse<IEnumerable<PostDto>>))]
        public async Task<IActionResult> Get([FromQuery] PostParameters postParam)
        {
            var posts = await _unitOfWork.Posts.GetPosts(postParam);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(posts.MetaData));
            var postDtos = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(posts);
            return Ok(new ApiResponse<IEnumerable<PostDto>>(postDtos, "Get posts successfully"));
        }

        // GET api/<PostController>/5
        [HttpGet("{id}")]
        [Produces(typeof(ApiResponse<PostDto>))]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _unitOfWork.Posts.FindById(id);
            if (post == null)
            {
                throw new ApiException("Post not found!", 404);
            }

            PostDto postDto = _mapper.Map<PostDto>(post);
            return Ok(new ApiResponse<PostDto>(postDto, "Get post successfully"));
        }

        // POST api/<PostController>
        [HttpPost]
        [Protect]
        [Produces(typeof(ApiResponse<PostDto>))]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto post)
        {
            Post p = _mapper.Map<Post>(post);
            var user = HttpContext.Items["User"] as User;

            p.UserId = user!.Id;
            await _unitOfWork.Posts.Add(p);
            await _unitOfWork.CompleteAsync();
            PostDto postDto = _mapper.Map<PostDto>(p);
            return new CreatedResult
            ("New Post", new ApiResponse<PostDto>(postDto, "Create post successfully"));
        }

        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> Put(int id, [FromBody] UpdatePostDto Post)
        {
            await _unitOfWork.Posts.Update(id, Post);
            return NoContent();
        }

        // PUT api/<PostController>/5/status
        [HttpPut("{id}/status")]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] PostStatus status)
        {
            if (status != PostStatus.Active && status != PostStatus.Paused)
            {
                throw new ApiException("Only update post active or paused", 400);
            }
            await _unitOfWork.Posts.UpdateStatus(id, status);
            return NoContent();
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        [Produces(typeof(NoContentResult))]
        public void Delete(int id)
        {
        }
    }
}
