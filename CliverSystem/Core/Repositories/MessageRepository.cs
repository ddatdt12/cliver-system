using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.DTOs.Queries;
using CliverSystem.DTOs.RequestFeatures;
using CliverSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CliverSystem.Core.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(DataContext context, ILogger logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public async Task<Message> CreateNewMessage(CreateMessageDto mess)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Message message = new Message();
                _mapper.Map(mess, message);
                Room? room = null;
                var memberKeys = mess.ReceiverId + "+" + mess.SenderId;
                var memberKeys2 = mess.SenderId + "+" + mess.ReceiverId;

                if (!mess.RoomId.HasValue && !string.IsNullOrEmpty(mess.ReceiverId))
                {
                    room = await _context.Rooms.Where(r => r.MemberKeys == memberKeys
                    || r.MemberKeys == memberKeys2).FirstOrDefaultAsync();
                    if (room == null)
                    {
                        room = new Room();

                        room.SetMemberKeys(mess.SenderId, mess.ReceiverId);
                        await _context.Rooms.AddAsync(room);
                        await _context.SaveChangesAsync();

                        await _context.RoomMembers.AddRangeAsync(new List<RoomMember>() {
                            new RoomMember{
                            MemberId = mess.SenderId,
                            RoomId = room.Id,
                            },
                            new RoomMember{
                            MemberId = mess.ReceiverId,
                            RoomId = room.Id,
                            },
                        });
                    }
                    message.RoomId = room.Id;
                }
                else if (mess.RoomId.HasValue)
                {
                    room = await _context.Rooms.FindAsync(mess.RoomId);

                    if (room == null || !room.MemberKeys.Contains(mess.SenderId))
                    {
                        throw new Exception("Room not found");
                    }
                }
                else
                {
                    throw new Exception("RoomId is required!");
                }

                await _context.AddAsync(message);
                await _context.SaveChangesAsync();

                //Chỗ này có thể dùng Message queues
                room.LastMessageId = message.Id;
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                await _context.Entry(message)
                .Reference(m => m.Sender)
                .LoadAsync();

                await _context.Entry(message)
                .Reference(m => m.RepliedToMessage)
                .LoadAsync();
                await _context.Entry(message)
                 .Reference(m => m.RelatedPost)
                .LoadAsync();

                await _context.Entry(message)
                 .Reference(m => m.CustomPackage)
                .Query().IgnoreQueryFilters().Include(p => p.Post).LoadAsync();

                return message;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Message> CreateNewMessageForCustomPackage(CreateMessageDto mess)
        {
            Message message = new Message();
            _mapper.Map(mess, message);
            Room? room = null;
            var memberKeys = mess.ReceiverId + "+" + mess.SenderId;
            var memberKeys2 = mess.SenderId + "+" + mess.ReceiverId;

            if (!mess.RoomId.HasValue && !string.IsNullOrEmpty(mess.ReceiverId))
            {
                room = await _context.Rooms.Where(r => r.MemberKeys == memberKeys
                || r.MemberKeys == memberKeys2).FirstOrDefaultAsync();
                if (room == null)
                {
                    room = new Room();

                    room.SetMemberKeys(mess.SenderId, mess.ReceiverId);
                    await _context.Rooms.AddAsync(room);
                    await _context.SaveChangesAsync();

                    await _context.RoomMembers.AddRangeAsync(new List<RoomMember>() {
                            new RoomMember{
                            MemberId = mess.SenderId,
                            RoomId = room.Id,
                            },
                            new RoomMember{
                            MemberId = mess.ReceiverId,
                            RoomId = room.Id,
                            },
                        });
                }
                message.RoomId = room.Id;
            }
            else if (mess.RoomId.HasValue)
            {
                room = await _context.Rooms.FindAsync(mess.RoomId);

                if (room == null || !room.MemberKeys.Contains(mess.SenderId))
                {
                    throw new Exception("Room not found");
                }
            }
            else
            {
                throw new Exception("RoomId is required!");
            }

            await _context.AddAsync(message);
            await _context.SaveChangesAsync();

            room.LastMessageId = message.Id;
            await _context.SaveChangesAsync();

            await _context.Entry(message)
             .Reference(m => m.CustomPackage)
            .Query().IgnoreQueryFilters().Include(p => p.Post).LoadAsync();


            return message;
        }


        public async Task<IEnumerable<Message>> GetMessages(string userId, int roomId, MessageFilterQuery query)
        {
            var room = await _context.Rooms.Where(r => r.Id == roomId).AsNoTracking().FirstOrDefaultAsync();
            var messagesQuery = _context.Messages.Where(m => m.RoomId == roomId);

            var totalCount = await messagesQuery.CountAsync();

            if (room == null || !room.MemberKeys.Contains(userId))
            {
                throw new Exception("Room not found");
            }
            var messages = (await messagesQuery
            .AsNoTracking()
            .OrderByDescending(m => m.CreatedAt)
            .Skip(query.Offset).Take(query.Limit)
            .IgnoreQueryFilters()
            .Include(m => m.RelatedPost)
            .Include(m => m.RepliedToMessage)
            .Include(m => m.Sender)
            .Include(m => m.CustomPackage)
                .ThenInclude(p => p.Post)
            .OrderByDescending(m => m.CreatedAt).ToListAsync()).OrderBy(m => m.CreatedAt); ;
            var messageList = new PagedList<Message>(messages.ToList(), totalCount, query.Offset, query.Limit);
            return messageList;
        }
        public async Task<IEnumerable<Message>> GetMessagesWithParnetId(string userId, string partnerId, MessageFilterQuery query)
        {
            var memberKeys = userId + "+" + partnerId;
            var memberKeys2 = partnerId + "+" + userId;
            var room = await _context.Rooms
            .Where(r => r.MemberKeys == memberKeys || r.MemberKeys == memberKeys2).AsNoTracking().FirstOrDefaultAsync();
            if (room == null)
            {
                return new List<Message>();
            }

            var messagesQuery = _context.Messages.Where(m => m.RoomId == room.Id);
            var totalCount = await messagesQuery.CountAsync();
            var messages = (await messagesQuery
            .AsNoTracking()
            .OrderByDescending(m => m.CreatedAt)
             .Skip(query.Offset).Take(query.Limit)
            .IgnoreQueryFilters()
             .Include(m => m.Sender)
              .Include(m => m.RelatedPost)
             .Include(m => m.RepliedToMessage)
            .Include(m => m.CustomPackage)
                .ThenInclude(p => p.Post)
            .ToListAsync()).OrderBy(m => m.CreatedAt);

            var messageList = new PagedList<Message>(_context.Messages.ToList(), totalCount, query.Offset, query.Limit);
            return messageList;
        }
    }
}
