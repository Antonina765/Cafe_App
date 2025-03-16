using ChatMinimalApi.DbStuff;
using ChatMinimalApi.DbStuff.Models;
using ChatMinimalApi.Hubs;
using ChatMinimalApi.ViewModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

builder.Services.AddDbContext<ChatDbContext>(o => o.UseNpgsql(ChatDbContext.CONNECTION_STRING));

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

app.MapControllers();

app.MapGet("/", () => "Hello World!");

app.MapGet("/smile", () => "Happy");

app.MapGet("/calc", (int x, int y) =>
{
    var answer = x + y;
    return answer;
});

app.UseSwagger();
app.UseSwaggerUI();

app.MapHub<ChatHub>("/hub/chat");

app.Run();
