using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CliverSystem.Core.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(DataContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public async Task<List<string>> GetMembersInRoom(int Id)
        {
            var room = await _context.Rooms.Where(r => r.Id == Id).FirstOrDefaultAsync();
            if (room == null)
            {
                throw new Exception("Invalid Room!");
            }
            return room.MemberKeys.Split("+", StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public async Task<List<Room>> GetRoomsOfUser(string userId)
        {
            var messagesQuery = _context.Messages;
            var roomsQuery = _context.Rooms.Where(r => r.MemberKeys.Contains(userId))
            .Include(r => r.Members)
            .AsNoTracking()
            .Join(messagesQuery, r => r.LastMessageId, m => m.Id, (room, message) => new Room
            {
                Id = room.Id,
                LastMessageId = room.LastMessageId,
                MemberKeys = room.MemberKeys,
                Members = room.Members,
                LastMessage = message
            });

            return await roomsQuery.ToListAsync();
        }

        public async Task<Room?> GetRoomDetail(string userId, int roomId)
        {
            var room = await _context.Rooms.Where(r => r.Id == roomId)
            .Include(r => r.Members).FirstOrDefaultAsync();

            if ((!room?.MemberKeys.Contains(userId)) ?? true)
            {
                return null;
            }

            var messages = (await _context.Messages.Where(m => m.RoomId == roomId)
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Include(m => m.RelatedPost)
            .Include(m => m.CustomPackage)
                .ThenInclude(p => p.Post)
            .OrderByDescending(m => m.CreatedAt).Take(20).ToListAsync()).OrderBy(m => m.CreatedAt);

            room!.Messages = messages.ToList();
            return room;
        }

        async Task<Room?> IRoomRepository.GetRoomWithPartnerId(string userId, string partnerId)
        {

            string memberKeyStr1 = userId + "+" + partnerId;
            string memberKeyStr2 = partnerId + "+" + userId;
            var room = await _context.Rooms.Where(r => r.MemberKeys == memberKeyStr1 || r.MemberKeys == memberKeyStr2)
            .Include(r => r.Members).FirstOrDefaultAsync();
            if (room == null)
            {
                return null;
            }

            var messages = (await _context.Messages
            .Where(m => m.RoomId == room.Id)
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Include(m => m.RelatedPost)
            .Include(m => m.CustomPackage)
                .ThenInclude(p => p.Post)
            .OrderByDescending(m => m.CreatedAt).Take(20).ToListAsync()).OrderBy(m => m.CreatedAt);

            room!.Messages = messages.ToList();
            return room;
        }
    }
}
