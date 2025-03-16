using ChatMinimalApi.DbStuff;
using ChatMinimalApi.DbStuff.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatMinimalApi.Hubs
{
    public interface IChatHub
    {
        Task NewMessageAdded(string message);
    }

    public class ChatHub : Hub<IChatHub>
    {
        private ChatDbContext _chatDbContext;

        public ChatHub(ChatDbContext chatDbContext)
        {
            _chatDbContext = chatDbContext;
        }

        public void UserEnteredToChat(int authorId, string authorName)
        {
            var newMessage = $"{authorName} вошёл в чат";

            SendMessage(authorId, authorName, newMessage);
        }

        public void AddNewMessage(int authorId, string authorName, string message)
        {
            var newMessage = $"{authorName}: {message}";

            SendMessage(authorId, authorName, newMessage);
        }

        private void SendMessage(int authorId, string authorName, string text)
        {
            var message = new Message
            {
                AuthorId = authorId,
                AuthorName = authorName,
                Text = text
            };
            _chatDbContext.Messages.Add(message);
            _chatDbContext.SaveChanges();

            Clients.All.NewMessageAdded(text).Wait();
        }
    }
}
