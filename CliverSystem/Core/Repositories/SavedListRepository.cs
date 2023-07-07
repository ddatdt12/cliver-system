using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.Error;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CliverSystem.Core.Repositories
{
    public class SavedListRepository : GenericRepository<SavedList>, ISavedListRepository
    {
        public SavedListRepository(DataContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {

        }

        public async Task<ICollection<SavedList>> GetMyLists(string userId, int? postId, string? sellerId)
        {
            var savedLists = await _context.SavedLists.Where(l => l.UserId == userId).AsNoTracking().ToListAsync();

            var savedListIds = savedLists.Select(s => s.Id);

            foreach (var item in savedLists)
            {
                item.SellerCount = await _context.SavedSellers.Where(ss => ss.SavedListId == item.Id).CountAsync();
                item.ServiceCount= await _context.SavedServices.Where(ss => ss.SavedListId == item.Id).CountAsync();
                var savedSer = await _context.SavedServices.Where(ss => ss.SavedListId == item.Id).Include(ss => ss.Post).FirstOrDefaultAsync();
                if (savedSer != null && savedSer.Post != null)
                {
                    item.Thumnail = savedSer.Post.Images.Split(";").FirstOrDefault();
                }
                else
                {
                    var saveUser = await _context.SavedSellers.Where(ss => ss.SavedListId == item.Id && ss.User.Avatar != null).Include(ss => ss.User).FirstOrDefaultAsync();
                    item.Thumnail = saveUser?.User?.Avatar;
                }
            }

            if (postId != null)
            {
                var map = await _context.SavedServices.AsNoTracking().Where(sS => sS.PostId == postId && savedListIds.Contains(sS.SavedListId)).ToDictionaryAsync(p => p.PostId.ToString() + "-" + p.SavedListId.ToString(), p => p);
                foreach (var item in savedLists)
                {
                    if (map.ContainsKey(postId + "-" + item.Id))
                    {
                        item.IsSaved = true;
                    }
                }
            }
            else if (sellerId != null)
            {
                var map = await _context.SavedSellers.AsNoTracking()
                .Where(sS => sS.UserId == sellerId && savedListIds.Contains(sS.SavedListId))
                .ToDictionaryAsync(p => p.UserId + "-" + p.SavedListId.ToString(), p => p);

                foreach (var item in savedLists)
                {
                    if (map.ContainsKey(sellerId + "-" + item.Id))
                    {
                        item.IsSaved = true;
                    }
                }
            }

            return savedLists;
        }

        public async Task<SavedList> CreateNewList(string userId, string name)
        {
            var nameExist = await _context.SavedLists.Where(l => l.UserId == userId && l.Name == name).AnyAsync();
            if (nameExist)
            {
                throw new Exception("Name already exists");
            }

            SavedList savedList = new SavedList
            {
                UserId = userId,
                Name = name
            };
            await _context.SavedLists.AddAsync(savedList);

            await _context.SaveChangesAsync();

            return savedList;
        }
        public async Task<SavedList> UpdateSavedList(int id, string userId, string name)
        {
            var nameExist = await _context.SavedLists.Where(l => l.UserId == userId && l.Name == name).AnyAsync();
            if (nameExist)
            {
                throw new Exception("Name already exists");
            }

            var savedList = await _context.SavedLists.Where(l => l.Id == id && l.UserId == userId).FirstOrDefaultAsync();
            if (savedList == null)
            {
                throw new Exception("Danh sách yêu thích không tồn tại");
            }

            savedList.Name = name;
            await _context.SaveChangesAsync();

            return savedList;
        }
        public async Task<bool> DeleteList(int id)
        {
            var savedList = await _context.SavedLists.Where(l => l.Id == id).FirstOrDefaultAsync();
            if (savedList == null)
            {
                throw new Exception("Save List doesn't exist");
            }

            _context.SavedLists.Remove(savedList);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<IEnumerable<Post>> GetSavedServices(int savedListId)
        {
            var savedServices = (await _context.SavedServices
            .AsNoTracking()
            .Where(l => l.SavedListId == savedListId)
            .Include(l => l.Post).OrderByDescending(l => l.CreatedAt)
            .Select(p => p.Post!).ToListAsync()).Select(
            s =>
            {
                s.IsSaved = true;
                return s;
            });

            return savedServices;
        }
        public async Task<IEnumerable<User>> GetSavedSellers(int savedListId)
        {
            var savedSellers = (await _context.SavedSellers
            .AsNoTracking()
            .Where(l => l.SavedListId == savedListId)
            .Include(l => l.User).OrderByDescending(l => l.CreatedAt)
            .Select(p => p.User!).ToListAsync()).Select(s =>
            {
                s.IsSaved = true;
                return s;
            });
            return savedSellers;
        }
        public async Task<bool> SaveOrRemoveSeller(int savedListId, string sellerId, string userId)
        {
            var savedListExist = await _context.SavedLists.Where(sL => sL.Id == savedListId && sL.UserId == userId).AnyAsync();

            if (!savedListExist)
            {
                throw new ApiException("Invalid Saved list", 400);
            }
            var savedSeller = await _context.SavedSellers.Where(sL => sL.SavedListId == savedListId && sL.UserId == sellerId).FirstOrDefaultAsync();

            if (savedSeller == null)
            {
                await _context.SavedSellers.AddAsync(new SavedSeller
                {
                    SavedListId = savedListId,
                    UserId = sellerId
                });
            }
            else
            {
                _context.SavedSellers.Remove(savedSeller);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveOrRemoveService(int savedListId, int postId, string userId)
        {
            var savedListExist = await _context.SavedLists.AsNoTracking().Where(sL => sL.Id == savedListId && sL.UserId == userId).AnyAsync();

            if (!savedListExist)
            {
                throw new ApiException("Invalid Saved list", 400);
            }

            var savedService = await _context.SavedServices.Where(sL => sL.SavedListId == savedListId && sL.PostId == postId).FirstOrDefaultAsync();

            if (savedService == null)
            {
                await _context.SavedServices.AddAsync(new SavedService
                {
                    SavedListId = savedListId,
                    PostId = postId
                });
            }
            else
            {
                _context.SavedServices.Remove(savedService);
            }
            await _context.SaveChangesAsync();
            return true;
        }

       public async Task<IEnumerable<Post>> GetRecentlySavedServices(string userId)
        {
            var savedServices = (await _context.SavedServices
            .AsNoTracking()
            .Where(l => l.SavedList!.UserId == userId)
            .Include(l => l.Post)
                .ThenInclude(p => p!.Packages.OrderBy(p => p.Price))
            .Include(l => l.Post)
                .ThenInclude(p => p.User)
            .OrderByDescending(l => l.CreatedAt)
            .Select(p => p.Post!).ToListAsync()).Select(
            s =>
            {
                s.IsSaved = true;
                return s;
            });

            return savedServices;
        }
    }
}
