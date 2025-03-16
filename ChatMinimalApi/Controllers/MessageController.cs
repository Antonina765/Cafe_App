using ChatMinimalApi.DbStuff.Models;
using ChatMinimalApi.DbStuff;
using ChatMinimalApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using ChatMinimalApi.Controllers.Produces;
using Microsoft.AspNetCore.Http;

namespace ChatMinimalApi.Controllers
{
    [ApiController]
    public class MessageController : Controller
    {
        private ChatDbContext _dbContext;
        public MessageController(ChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("/getMessages")]
        public List<ChatMessageViewModel> GetMessages()
        {
            return _dbContext.Messages.Select(x => new ChatMessageViewModel
            {
                Id = x.Id,
                Text = x.Text,
                AuthorId = x.AuthorId,
                AuthorName = x.AuthorName,
            }).ToList();
        }

        [HttpPost("/addMessage")]
        [ProduceStatus(StatusCodes.Status201Created)]
        [ProduceStatus(StatusCodes.Status409Conflict)]
        public int AddMessage([FromBody] AddMessageViewModel vm)
        {
            // Check if the last message has the same text as new one
            if (_dbContext
                    .Messages
                    .OrderByDescending(x => x.Id)
                    .Select(x => x.Text)
                    .First() == vm.Text)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                return -1;
            }

            var message = new Message
            {
                AuthorId = vm.AuthorId,
                AuthorName = vm.AuthorName,
                Text = vm.Text
            };

            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();

            HttpContext.Response.StatusCode = StatusCodes.Status201Created;
            return message.Id;
        }
    }
}
