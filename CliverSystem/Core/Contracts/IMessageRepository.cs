using CliverSystem.DTOs;
using CliverSystem.DTOs.Queries;
using CliverSystem.Models;

namespace CliverSystem.Core.Contracts
{
    public interface IMessageRepository
    {
        Task<Message> CreateNewMessage(CreateMessageDto mess);
        Task<Message> CreateNewMessageForCustomPackage(CreateMessageDto mess);
        Task<IEnumerable<Message>> GetMessages(string userId, int roomId, MessageFilterQuery query);
        Task<IEnumerable<Message>> GetMessagesWithParnetId(string userId, string partnerId, MessageFilterQuery query);
    }
}
