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
            var userId = HttpContext.Items["UserId"] as string;
            var posts = await _unitOfWork.Posts.GetPosts(postParam, userId: userId);

            var postDtos = _mapper.Map<IEnumerable<Post>, IEnumerable<PostDto>>(posts);
            return Ok(new ApiResponse<IEnumerable<PostDto>>(postDtos, "Get posts successfully", posts.MetaData));
        }

        // GET api/posts/:id/recently
        [HttpGet("recently")]
        [Protect]
        [Produces(typeof(ApiResponse<PostDto>))]
        public async Task<IActionResult> GetRecentPosts([FromQuery] RecentPostParameters recentPostParameters)
        {
            var user = HttpContext.Items["User"] as User;
            var posts = await _unitOfWork.RecentPosts.GetRecentPosts(user!.Id, recentPostParameters);
            var metaData = (posts as PagedList<RecentPost>)?.MetaData;

            var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);
            return Ok(new ApiResponse<IEnumerable<PostDto>>(postDtos, "Get my recent posts successfully", metaData));
        }

        // GET api/posts/5
        [HttpGet("{id}")]
        [Produces(typeof(ApiResponse<PostDto>))]
        public async Task<IActionResult> Get(int id)
        {
            var user = HttpContext.Items["User"] as User;
            var post = await _unitOfWork.Posts.FindById(id, user?.Id);
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
            if (user != null && !user.IsVerified)
            {
                throw new ApiException("Unauthorized!", 401);
            }
            p.UserId = user!.Id;
            await _unitOfWork.Posts.Add(p);
            await _unitOfWork.CompleteAsync();
            PostDto postDto = _mapper.Map<PostDto>(p);
            return new CreatedResult
            ("New Post", new ApiResponse<PostDto>(postDto, "Create post successfully"));
        }

        // PUT api/<PostController>/5
        [HttpPut("{id}")]
        [Protect]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> Put(int id, [FromBody] UpdatePostDto Post)
        {
            var user = HttpContext.Items["User"] as User;
            if (user != null && !user.IsVerified)
            {
                throw new ApiException("Unauthorized!", 401);
            }

            await _unitOfWork.Posts.Update(id, Post);
            return NoContent();
        }

        // PUT api/<PostController>/5/status
        [HttpPut("{id}/status")]
        [Protect]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] PostStatus status)
        {
            var userId = HttpContext.Items["UserId"] as string;
            if (status != PostStatus.Active && status != PostStatus.Paused)
            {
                throw new ApiException("Only update post active or paused", 400);
            }
            await _unitOfWork.Posts.UpdateStatus(id, userId, status);
            return NoContent();
        }

        // DELETE api/<PostController>/5
        [HttpDelete("{id}")]
        [Protect]
        [Produces(typeof(NoContentResult))]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = HttpContext.Items["UserId"] as string;
            await _unitOfWork.Posts.DeletePost(id, userId!);
            return NoContent();
        }

        [HttpGet]
        [Route("{id}/reviews")]
        [Produces(typeof(ApiResponse<IEnumerable<ReviewDto>>))]
        public async Task<IActionResult> GetReviews(int id)
        {
            var reviews = await _unitOfWork.Reviews.GetReviewsOfPost(id);

            var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            return Ok(new ApiResponse<IEnumerable<ReviewDto>>(reviewDtos, "Get Reviews succesfully"));
        }

        [HttpGet]
        [Route("{id}/reviews/statistic")]
        [Produces(typeof(ApiResponse<IEnumerable<RatingStat>>))]
        public async Task<IActionResult> GetReviewsStatistic(int id)
        {
            var reviewStats = await _unitOfWork.Reviews.GetReviewsStats(id);

            return Ok(new ApiResponse<object>(reviewStats, "Get Reviews stats succesfully"));
        }
    }
}
