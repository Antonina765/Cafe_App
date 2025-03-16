using Microsoft.AspNetCore.Mvc;

namespace ChatMinimalApi.Controllers.Produces
{
    public class ProduceStatus : ProducesResponseTypeAttribute
    {
        public ProduceStatus(int statusCode) : base(statusCode)
        {
        }
    }
}
