using CliverSystem.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs.User;
using CliverSystem.DTOs;
using AutoMapper;
using CliverSystem.Attributes;
using CliverSystem.Services.Payment;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Controllers
{
    [Route("api/earning")]
    [ApiController]
    public class EarningStatisticController : ControllerBase
    {
        private readonly string API_URL;
        private readonly ILogger<EarningStatisticController> _logger;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public EarningStatisticController(IConfiguration configuration, ILogger<EarningStatisticController> logger, IMapper mapper, DataContext context)
        {
            API_URL = configuration["API_Url"];
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("analytics")]
        [Protect]
        [Produces(typeof(ApiResponse<EarningAnalytics>))]
        public async Task<IActionResult> GetAnalytics()
        {
            var userId = HttpContext.Items["UserId"] as string;
            var analytics = new EarningAnalytics();

            var userOrders = await _context.Orders.Where(o => o.SellerId == userId).ToListAsync();
            var wallet = await _context.Wallets.Where(o => o.User!.Id == userId).FirstOrDefaultAsync();

            var firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            analytics.EarningInMonth = userOrders.Where(o => o.CreatedAt >= firstDate && o.Status == OrderStatus.Completed || o.Status == OrderStatus.Delivered)
            .Select(p => p.Price)
            .Sum(p => p);
            analytics.CompletedOrders = userOrders.Where(o => o.Status == OrderStatus.Completed).Count();
            analytics.ActiveOrders = userOrders.Where(o => o.Status != OrderStatus.Completed).Count();
            analytics.AvailableForWithdrawal = wallet?.AvailableForWithdrawal ?? 0;
            analytics.AvgSellingPrice = (await _context.Packages.Where(p => p.Post!.UserId == userId).Select(p => p.Price).ToListAsync()).DefaultIfEmpty(0).Average(p => p);

            return Ok(new ApiResponse<object>(analytics, "Get EarningAnalytics details"));
        }
        [HttpGet("revenues")]
        [Protect]
        [Produces(typeof(ApiResponse<EarningRevenue>))]
        public async Task<IActionResult> GetRevenues()
        {
            var userId = HttpContext.Items["UserId"] as string;
            var revenue = new EarningRevenue();

            var userOrders = await _context.Orders.Where(o => o.SellerId == userId).ToListAsync();
            var buyerOrders = await _context.Orders.Where(o => o.BuyerId == userId && o.Status != OrderStatus.Cancelled).ToListAsync();
            var wallet = await _context.Wallets.Where(o => o.User!.Id == userId).FirstOrDefaultAsync();

            var firstDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            revenue.Withdrawn = wallet?.Withdrawn ?? 0;
            revenue.CancelledOrders = userOrders.Where(o => o.Status == OrderStatus.Cancelled).Count();
            revenue.PendingClearance = userOrders.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.Price);
            revenue.UsedForPurchases = buyerOrders.Sum(o => o.Price);
            revenue.Cleared = buyerOrders.Where(bo => bo.Status == OrderStatus.Completed).Sum(o => o.Price);

            return Ok(new ApiResponse<object>(revenue, "Get EarningRevenue details"));
        }
    }
}
