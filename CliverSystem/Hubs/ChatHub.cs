using CliverSystem.Models;
using Microsoft.AspNetCore.SignalR;

namespace CliverSystem.Hubs
{
    public class ChatHub : Hub
    {
        public IDictionary<string, string> _connections { get; set; }
        public static List<Room> Rooms = new List<Room>();
        public ChatHub(IDictionary<string, string> connections)
        {
            _connections =connections;
        }

        public async Task JoinRealtime(string userId)
        {
            //throw new HubException("This error will be sent to the client!");
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
                _connections.Add(Context.ConnectionId, userId);
                System.Diagnostics.Debug.WriteLine("Check connections" + string.Join("; ", _connections.Values));
                Console.WriteLine("Check connections:" + string.Join("; ", _connections.Values));
            }
            catch (Exception e)
            {
                throw new HubException(e.Message);
            }
        }

        public async Task SendMessage(Message message)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out string? userId)){
                System.Diagnostics.Debug.WriteLine("Check user send message: " + userId);
                Console.WriteLine("Check connections:" + string.Join("; ", _connections.Values));
                await Clients.Groups(_connections.Values).SendAsync("ReceiveMessage", message);
            }
            else
            {
                throw new HubException("Your connection is lost!");
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
