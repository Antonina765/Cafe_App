using Cafe.Data.Repositories;
using Microsoft.AspNetCore.SignalR;
using Cafe_App.Services;
using Cafe.Data.Interface.Repositories;

namespace Cafe_App.Hubs
{
    public interface IChatHub
    {
        Task NewMessageAdded(string message);
    }

    public class ChatHub : Hub<IChatHub>
    {
        private AuthService _authService;
        private IChatMessageRepository _chatMessageRepositry;

        public ChatHub(AuthService authService, IChatMessageRepository chatMessageRepositry)
        {
            _authService = authService;
            _chatMessageRepositry = chatMessageRepositry;
        }

        public void UserEnteredToChat()
        {
            var userName = _authService.GetName();

            var newMessage = $"{userName} вошёл в чат";

            SendMessage(newMessage);
        }

        public void AddNewMessage(string message)
        {
            var userName = _authService.GetName();

            var newMessage = $"{userName}: {message}";

            SendMessage(newMessage);
        }

        private void SendMessage(string message)
        {
            var userId = _authService.GetUserId();
            _chatMessageRepositry.AddMessage(userId, message);
            Clients.All.NewMessageAdded(message).Wait();
        }
        
        public void UserCreatedNewPost()
        {
            var userName = _authService.GetName();

            var newMessage = $"{userName} add new post";

            SendMessage(newMessage);
        }
    }
}
