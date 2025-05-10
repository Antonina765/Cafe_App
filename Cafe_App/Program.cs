using Cafe_App.CustomMiddlewares;
using Cafe_App.Hubs;
using Cafe_App.Services;
using Cafe_App.Services.Apis;
using Cafe.Data;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Cafe.Data.Repositories;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAuthentication(AuthService.AUTH_TYPE_KEY)
    .AddCookie(AuthService.AUTH_TYPE_KEY, config =>
    {
        config.LoginPath = "/Auth/Login";
        config.AccessDeniedPath = "/Home/Forbidden";

    });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<WebDbContext>(options => options.UseNpgsql(WebDbContext.CONNECTION_STRING));

builder.Services.AddSignalR();

builder.Services.AddScoped<ICafeRepository<CafeData>, CafeRepository>();
builder.Services.AddScoped<IUserRepository<UserData>, UserRepository>();
builder.Services.AddScoped<IMenuRepository<MenuItemData>, MenuRepository>();
builder.Services.AddScoped<IBookingRepository<BookingData>, BookingRepository>();

var registrationHelper = new RegistrationHelper();
registrationHelper.AutoRegisterRepositories(builder.Services);

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<EnumHelper>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();


builder.Services.AddHttpClient<HttpJokeApi>(httpClient =>
    httpClient.BaseAddress = new Uri("https://official-joke-api.appspot.com/random_joke")
);

registrationHelper.AutoRegisterServiceByAttribute(builder.Services);
registrationHelper.AutoRegisterServiceByAttributeOnConstructor(builder.Services);

builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.SetIsOriginAllowed(origin => true);
        policy.AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();

var seed = new Seed();
seed.Fill(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<CustomLocalizationMiddleware>();
app.UseMiddleware<CustomThemeMiddleware>();

//app.MapStaticAssets();

app.MapHub<ChatHub>("/hub/chatMainPage");

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Cafe}/{action=Index}/{id?}");

app.Run();