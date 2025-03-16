using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Cafe.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cafe_App.Localizations;
using Cafe_App.Models.Cafe;
using Cafe_App.Services;

namespace Cafe_App.ApiControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiChatController : ControllerBase
    {
        private IChatMessageRepository _chatMessageRepositry;

        public ApiChatController(IChatMessageRepository chatMessageRepositry)
        {
            _chatMessageRepositry = chatMessageRepositry;
        }

        public List<string> GetLastMessages()
        {
            return _chatMessageRepositry.GetLastMessages();
        }
    }
}
