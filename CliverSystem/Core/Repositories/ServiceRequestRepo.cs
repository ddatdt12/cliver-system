using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Error;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static CliverSystem.Common.Enum;

namespace CliverSystem.Core.Repositories
{
    public class ServiceRequestRepo : GenericRepository<ServiceRequest>, IServiceRequestRepo
    {
        public ServiceRequestRepo(DataContext context, ILogger logger, IMapper mapper)
        : base(context, logger, mapper)
        {
        }

        public async Task<IEnumerable<ServiceRequest>> GetServiceRequests(string userId, Mode mode)
        {
            var serviceReqsQuery = _context.ServiceRequests.AsNoTracking().Include(sr => sr.Category)
            .Include(sr => sr.Subcategory)
            .Include(Sr => Sr.User);
            if (mode == Mode.Buyer)
            {
                return await serviceReqsQuery.Where(sR => sR.UserId == userId).ToListAsync();
            }

            //List<Subcategory> subcates = (await _context.Posts.AsNoTracking().Where(p => p.UserId == userId).Include(p => p.Subcategory!.Category!)
            //.Select(p => p.Subcategory!).ToListAsync()).DistinctBy(s => s!.Id).ToList() ?? new List<Subcategory>();

            //if (subcates == null || subcates.Count() == 0)
            //{
            //    return new List<ServiceRequest>();
            //}


            //var cates = subcates.Select(s => s!.CategoryId).Distinct().ToList();
            //IEnumerable<int> subcateIds = subcates.Select(sc => sc.Id);

            //var serviceReqs = await serviceReqsQuery
            // .Where(s => s.UserId != userId && s.DoneAt == null && s.SubcategoryId != null ? subcateIds.Contains((int)s.SubcategoryId) : cates.Contains(s.CategoryId)).OrderByDescending(sq => sq.CreatedAt).ToListAsync();
           
            return await serviceReqsQuery.Where(s => s.UserId != userId).OrderByDescending(sq => sq.CreatedAt).ToListAsync();
        }

        async Task<ServiceRequest> IServiceRequestRepo.GetServiceRequestDetail(int id, string userId)
        {
            var serReqest = await _context.ServiceRequests.Where(sR => sR.Id == id && sR.UserId == userId)
            .Include(sr => sr.Category)
            .Include(sr => sr.Subcategory)
            .Include(Sr => Sr.User)
            .FirstOrDefaultAsync();

            if (serReqest == null)
            {
                throw new ApiException("Service request not found!", 404);
            }

            return serReqest;
        }
        public async Task<ServiceRequest> CreateServiceRequest(string userId, CreateServiceRequestDto createDto)
        {
            if (createDto.SubcategoryId.HasValue)
            {
                var isValidSub = _context.Subcategories.Where(s => s.Id == createDto.SubcategoryId && s.CategoryId == createDto.CategoryId).Any();
                if (!isValidSub)
                {
                    throw new ApiException("Invalid Subcategory", 400);
                }
            }
            var req = _mapper.Map<ServiceRequest>(createDto);

            req.UserId = userId;


            await _context.ServiceRequests.AddAsync(req);
            await _context.SaveChangesAsync();

            return req;
        }
        public async Task UpdateServiceRequest(int id, string userId, UpdateServiceRequestDto updateDto)
        {
            var serReqest = await _context.ServiceRequests.Where(sR => sR.Id == id && sR.UserId == userId).FirstOrDefaultAsync();

            if (serReqest == null)
            {
                throw new ApiException("Service request not found!", 404);
            }

            if (updateDto.SubcategoryId.HasValue)
            {
                bool isValidSub = true;
                if (updateDto.CategoryId != serReqest.CategoryId)
                {
                    isValidSub = await _context.Subcategories.AsNoTracking().Where(s => s.Id == updateDto.SubcategoryId && s.CategoryId == updateDto.CategoryId).AnyAsync();
                }
                else
                {
                    isValidSub = await _context.Subcategories.AsNoTracking().Where(s => s.Id == updateDto.SubcategoryId && s.CategoryId == serReqest.CategoryId).AnyAsync();
                }
                if (!isValidSub)
                {
                    throw new ApiException("Invalid Subcategory", 400);
                }
            }

            _mapper.Map(updateDto, serReqest);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteServiceRequest(int id, string userId)
        {
            var serReqest = await _context.ServiceRequests.Where(sR => sR.Id == id && sR.UserId == userId).FirstOrDefaultAsync();

            if (serReqest == null)
            {
                throw new ApiException("Service request not found!", 404);
            }

            _context.ServiceRequests.Remove(serReqest);
            await _context.SaveChangesAsync();
        }
        public async Task MarkDoneServiceRequest(int id, string userId)
        {
            var serReqest = await _context.ServiceRequests.Where(sR => sR.Id == id && sR.UserId == userId).FirstOrDefaultAsync();

            if (serReqest == null)
            {
                throw new ApiException("Service request not found!", 404);
            }

            serReqest.DoneAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }

    }
}
