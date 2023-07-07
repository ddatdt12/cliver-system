using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.Error;
using CliverSystem.Hubs;
using CliverSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using static CliverSystem.Common.Enum;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CliverSystem.Controllers
{
    [Route("api/seller/custom-packages")]
    [ApiController]
    public class SellerCustomPackageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        IHubContext<ChatHub> _chatHub;

        public SellerCustomPackageController(IUnitOfWork unitOfWork, IMapper mapper, DataContext context, IHubContext<ChatHub> chatHub)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _context = context;
            _chatHub = chatHub;
        }

        [HttpGet]
        [Produces(typeof(ApiResponse<IEnumerable<ServiceRequestDto>>))]
        public async Task<IActionResult> CreateCustomPackage()
        {
            var userId = HttpContext.Items["UserId"] as string;
            var cusPackages = await _context.Packages.IgnoreQueryFilters()
            .Where(p => p.Type == PackageType.Custom && p.Post!.UserId == userId!).ToListAsync();
            var cusPackDtos = _mapper.Map<IEnumerable<CustomPackageDto>>(cusPackages);
            return Ok(new ApiResponse<IEnumerable<CustomPackageDto>>(cusPackDtos, "Get custom package successfully"));
        }
        [HttpPost]
        [Produces(typeof(ApiResponse<IEnumerable<ServiceRequestDto>>))]
        public async Task<IActionResult> CreateCustomPackage([FromBody] CreateCustomPackageDto createDto)
        {
            var userId = HttpContext.Items["UserId"] as string;
            var package = _mapper.Map<Package>(createDto);
            using var trans = await _context.Database.BeginTransactionAsync();
            try
            {
                if (userId == createDto.BuyerId)
                {
                    throw new ApiException("Can not create custom package for yourself", 400);
                }

                package.IsAvailable = true;
                var buyer = await _context.Users.Where(u => u.Id == createDto.BuyerId).FirstOrDefaultAsync();

                if (buyer == null)
                {
                    throw new ApiException("Invalid buyer", 400);
                }

                var post = await _context.Posts.Where(p => p.Id == createDto.PostId && p.UserId == userId).FirstOrDefaultAsync();

                if (post == null)
                {
                    throw new ApiException("Post not found", 400);
                }

                package.Type = PackageType.Custom;
                await _context.AddAsync(package);
                await _context.SaveChangesAsync();
                var message = await _unitOfWork.Messages.CreateNewMessageForCustomPackage(new CreateMessageDto
                {
                    Content = "Custom Offer",
                    SenderId = userId!,
                    ReceiverId = createDto.BuyerId,
                    CustomPackageId = package.Id,
                    RoomId = createDto.RoomId
                });

                var messageDto = _mapper.Map<MessageDto>(message);

                await _chatHub.Clients.Users(new string[] { userId!, createDto.BuyerId }).SendAsync("ReceiveMessage", messageDto);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();

                var customP = _mapper.Map<CustomPackageDto>(package);
                return Ok(new ApiResponse<CustomPackageDto>(customP, "Create custom package successfully"));

            }
            catch (Exception)
            {
                await trans.RollbackAsync();
                throw;
            }
        }


        [HttpPut("{id}/status")]
        [Produces(typeof(ApiResponse<IEnumerable<ServiceRequestDto>>))]
        public async Task<IActionResult> UpdateCustomPackage(int id, [FromBody, Required] PackageStatus status)
        {
            var pack = await _context.Packages.IgnoreQueryFilters().Where(p => p.Id == id && p.Type == PackageType.Custom).FirstOrDefaultAsync();

            if (pack == null || pack.Type != PackageType.Custom)
            {
                throw new ApiException("Invalid Package", 404);
            }
            if (pack.Status == PackageStatus.Ordered)
            {
                throw new ApiException("Package have been ordered", 400);
            }
            if (status != PackageStatus.Closed && status != PackageStatus.Declined)
            {
                throw new ApiException("Only accept Closed or Declined status", 400);
            }
            pack.Status = status;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
