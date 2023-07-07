using CliverSystem.Core.Contracts;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using static CliverSystem.Common.Enum;
using CliverSystem.Error;
using CliverSystem.DTOs;

namespace CliverSystem.Core.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(DataContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {

        }

        public async Task<IEnumerable<Review>> GetReviewsOfUser(string userId)
        {
            var reviews = await _context.Reviews
            .Where(r => (r.Type == ReviewType.FromBuyer && r.Order!.SellerId == userId) || (r.Type == ReviewType.FromSeller && r.Order!.BuyerId == userId)).AsNoTracking()
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt).ToListAsync();
            return reviews;
        }

        public async Task<Review> CreateReview(int orderId, string userId, CreateReviewDto createDto, Mode mode)
        {
            var order = await _context.Orders.Where(r => r.Id == orderId).Include(o => o.Package).Include(r => r.Reviews).FirstOrDefaultAsync();

            if (order == null || order.Status != OrderStatus.Completed)
            {
                throw new ApiException("Invalid order", 400);
            }
            Review review;
            if (mode == Mode.Buyer)
            {
                if (order.BuyerId != userId)
                {
                    throw new ApiException("You are not authorized to review this order", 401);
                }
                if (order.Reviews.Where(r => r.Type == ReviewType.FromBuyer).Any())
                {
                    throw new ApiException("You have already review this order", 400);
                }

                review = _mapper.Map<Review>(createDto);
                review.UserId = userId;
                review.Type = ReviewType.FromBuyer;
                review.OrderId = orderId;

                var postId = order.Package!.PostId!;
                var post = await _context.Posts.Where(p => p.Id == postId).FirstOrDefaultAsync();
                var sumRatings = post!.RatingAvg * post.RatingCount;
                post.RatingCount++;
                post.RatingAvg = (sumRatings + review.Rating) * 1.0 / post.RatingCount;

                var sellerId = order.SellerId!;
                var seller = await _context.Users.Where(p => p.Id == sellerId).FirstOrDefaultAsync();
                sumRatings = seller!.RatingAvg * seller.RatingCount;
                seller.RatingCount++;
                seller.RatingAvg = (sumRatings + review.Rating) * 1.0 / seller.RatingCount;
            }
            else
            {
                if (order.SellerId != userId)
                {
                    throw new ApiException("You are not authorized to review this order", 401);
                }
                if (order.Reviews.Where(r => r.Type == ReviewType.FromSeller).Any())
                {
                    throw new ApiException("You have already review this order", 400);
                }

                if (!order.Reviews.Where(r => r.Type == ReviewType.FromBuyer).Any())
                {
                    throw new ApiException("You cannot review this order", 401);
                }
                review = _mapper.Map<Review>(createDto);
                review.UserId = userId;
                review.Type = ReviewType.FromSeller;
                review.OrderId = orderId;

                var buyerId = order.BuyerId!;
                var buyer = await _context.Users.Where(p => p.Id == buyerId).FirstOrDefaultAsync();
                var sumRatings = buyer!.RatingAvg * buyer.RatingCount;
                buyer.RatingCount++;
                buyer.RatingAvg = (sumRatings + review.Rating) * 1.0 / buyer.RatingCount;
            }



            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            return review;
        }

        async Task<IEnumerable<Review>> IReviewRepository.GetReviewsOfPost(int postId)
        {
            var reviews = await _context.Reviews.
            Where(r => r.Order!.Package!.PostId == postId)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt).ToListAsync();

            return reviews;
        }

        public async Task<List<RatingStat>> GetReviewsStats(int postId)
        {
            var ratings = new int[5].Select((r, i) => new RatingStat { Rating = i + 1, Count = 0 }).ToList();
            var ratingStats = await _context.Reviews.Where(r => r.Order!.Package!.PostId == postId).GroupBy(r => r.Rating)
             .OrderBy(gr => gr.Key).Select(gr => new { Rating = gr.Key, Count = gr.Count() }).ToListAsync();

            for (int i = 0; i < ratingStats.Count; i++)
            {
                var rate = ratingStats[i];
                ratings[rate.Rating - 1] = new RatingStat { Rating = rate.Rating, Count = rate.Count };
            }

            return ratings;
        }

        async Task<List<RatingStat>> IReviewRepository.GetReviewsStatsOfUser(string userId)
        {
            var ratings = new int[5].Select((r, i) => new RatingStat { Rating = i + 1, Count = 0 }).ToList();
            var ratingStats = await _context.Reviews.Where(r => (r.Type == ReviewType.FromBuyer && r.Order!.SellerId == userId) || (r.Type == ReviewType.FromSeller && r.Order!.BuyerId == userId)).GroupBy(r => r.Rating)
             .OrderBy(gr => gr.Key).Select(gr => new { Rating = gr.Key, Count = gr.Count() }).ToListAsync();

            for (int i = 0; i < ratingStats.Count; i++)
            {
                var rate = ratingStats[i];
                ratings[rate.Rating - 1] = new RatingStat { Rating = rate.Rating, Count = rate.Count };
            }

            return ratings;
        }

        public async Task<List<Review>> GetReviewsSentiment(string userId)
        {
            var reviewSentiments = await _context.Reviews
            .Where(r => (r.Type == ReviewType.FromBuyer && r.Order!.SellerId == userId) || (r.Type == ReviewType.FromSeller && r.Order!.BuyerId == userId)).AsNoTracking()
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt).ToListAsync();
            return reviewSentiments;
        }
    }
}
