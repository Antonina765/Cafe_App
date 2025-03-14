using System.Globalization;
using System.Text.RegularExpressions;
using Cafe_App.Services;
using Cafe.Data.Interface.Models;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Cafe.Data.Repositories;
using Enums.Users;

namespace Cafe_App.CustomMiddlewares
{
    public class CustomLocalizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomLocalizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var authService = context.RequestServices.GetRequiredService<AuthService>();
            var userRepositry = context.RequestServices.GetRequiredService<IUserRepository<UserData>>();

            if (authService.IsAuthenticated())
            {
                var user = userRepositry.Get(authService.GetUserId()!.Value)!;
                SwitchLanguage(user.Language);
                await _next.Invoke(context);
                return;
            }

            var langFromCookie = context.Request.Cookies["lang"];
            if (langFromCookie != null)
            {
                var lang = Enum.Parse<Languages>(langFromCookie);
                SwitchLanguage(lang);
                await _next.Invoke(context);
                return;
            }
            
            await _next.Invoke(context);
        }

        private void SwitchLanguage(Languages language)
        {
            CultureInfo culture;

            switch (language)
            {
                case Languages.Ru:
                    culture = new CultureInfo("ru-RU");
                    break;
                case Languages.En:
                    culture = new CultureInfo("en-US");
                    break;
                default:
                    throw new Exception("Unknown language");
            }

            SwitchLanguages(culture);
        }

        private void SwitchLanguages(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}
