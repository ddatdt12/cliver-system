using AutoMapper;
using CliverSystem.Core.Contracts;
using CliverSystem.DTOs;
using CliverSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;

namespace CliverSystem.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private IDictionary<string, string> _connections { get; set; }
        private IUnitOfWork _unitOfWork { get; set; }
        private IMapper _mapper { get; set; }
        public ChatHub(IDictionary<string, string> connections, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _connections = connections;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task SendMessage(CreateMessageDto message)
        {
            try
            {
                var userId = Context.UserIdentifier;
                System.Diagnostics.Debug.WriteLine("Check user send message: " + userId);
                message.SenderId = userId!;
                var newMessage = await _unitOfWork.Messages.CreateNewMessage(message);
                var membersId = await _unitOfWork.Rooms.GetMembersInRoom(newMessage.RoomId);
                
                var messageDto = _mapper.Map<MessageDto>(newMessage);
                await Clients.Users(membersId).SendAsync("ReceiveMessage", messageDto);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }
     
        public async Task SendMessageStr(string messageJson)
        {
            try
            {
                CreateMessageDto? message =
                   JsonSerializer.Deserialize<CreateMessageDto>(messageJson);

                if (message == null)
                {
                    throw new Exception("Wrong format!");
                }

                var userId = Context.UserIdentifier;
                System.Diagnostics.Debug.WriteLine("Check user send message: " + userId);
                message.SenderId = userId!;
                var newMessage = await _unitOfWork.Messages.CreateNewMessage(message);
                var membersId = await _unitOfWork.Rooms.GetMembersInRoom(newMessage.RoomId);
                
                var messageDto = _mapper.Map<MessageDto>(newMessage);
                await Clients.Users(membersId).SendAsync("ReceiveMessage", messageDto);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }

        public async Task TestStr(string messageJson)
        {
            try
            {
                var userId = Context.UserIdentifier;
                await Clients.User(userId!).SendAsync("ReceiveTestStr", "Nhận được rồi nha data: " + messageJson);
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
