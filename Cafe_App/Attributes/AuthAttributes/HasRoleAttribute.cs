using Cafe_App.Services;
using Enums.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cafe_App.Attributes.AuthAttributes
{
    public class HasRoleAttribute : ActionFilterAttribute
    {
        private Roles _role;
        
        public HasRoleAttribute(Roles role)
        {
            _role = role;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var authService = context
                .HttpContext
                .RequestServices
                .GetRequiredService<AuthService>();

            if (!authService.HasRole(_role))
            {
                context.Result = new ForbidResult();
                return;
            }

            base.OnActionExecuted(context);
        }
    }
}
