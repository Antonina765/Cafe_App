using ChatMinimalApi.DbStuff.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatMinimalApi.DbStuff
{
    public class ChatDbContext : DbContext
    {
        public const string CONNECTION_STRING = "Host=localhost;Port=5432;Database=CafeAppChat;Username=postgres;Password=12345;";

        public DbSet<Message> Messages { get; set; }

        public ChatDbContext(DbContextOptions<ChatDbContext> contextOptions)
            : base(contextOptions) { }
    }
}
