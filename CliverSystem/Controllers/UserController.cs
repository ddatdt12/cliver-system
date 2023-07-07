using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.Order;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CliverSystem.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserController(ILogger<UserController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string search)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var users = await _unitOfWork.Users.Search(search, userId);
            var data = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(new
            {
                data = data
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var item = await _unitOfWork.Users.FindById(id.ToString(), userId);

            if (item == null)
                return NotFound();

            var userDto = _mapper.Map<UserDto>(item);
            return Ok(new ApiResponse<UserDto>(userDto, "Get user info"));
        }
        [HttpGet("{id}/posts")]
        public async Task<IActionResult> GetUserPosts(Guid id, PostParameters postParameters)
        {
            var posts = await _unitOfWork.Posts.GetPostsByUser(id.ToString(), postParameters);

            var postDtos = _mapper.Map<IEnumerable<PostDto>>(posts);
            return Ok(new ApiResponse<IEnumerable<PostDto>>(postDtos, "Get posts successfully"));
        }



        [HttpGet]
        [Route("{id}/reviews")]
        [Produces(typeof(ApiResponse<IEnumerable<ReviewDto>>))]
        public async Task<IActionResult> GetReviews(string id)
        {
            var reviews = await _unitOfWork.Reviews.GetReviewsOfUser(id);

            var reviewDtos = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
            return Ok(new ApiResponse<IEnumerable<ReviewDto>>(reviewDtos, "Get Reviews succesfully"));
        }

        [HttpGet]
        [Route("{id}/reviews/statistic")]
        [Produces(typeof(ApiResponse<IEnumerable<ReviewDto>>))]
        public async Task<IActionResult> GetReviewsStatistic(string id)
        {
            var reviewStats = await _unitOfWork.Reviews.GetReviewsStatsOfUser(id);

            return Ok(new ApiResponse<IEnumerable<RatingStat>>(reviewStats, "Get Reviews  statistic succesfully"));
        }

        [HttpGet]
        [Route("{id}/reviews/sentiments")]
        [Produces(typeof(ApiResponse<IEnumerable<ReviewSentimentDto>>))]
        public async Task<IActionResult> GetReviewsSentiment(string id)
        {
            var reviewSentiments = await _unitOfWork.Reviews.GetReviewsSentiment(id);
            var reviewSentimentsDto = _mapper.Map<IEnumerable<ReviewSentimentDto>>(reviewSentiments);
            return Ok(new ApiResponse<IEnumerable<ReviewSentimentDto>>(reviewSentimentsDto, "Get Reviews  statistic succesfully"));
        }
    }
}
