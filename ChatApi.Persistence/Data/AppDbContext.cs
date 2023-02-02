using ChatApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Persistence.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<ChatMessage>().HasOne(e => e.SenderUser).WithMany(x  => x.SendedMessages);
        }

        public virtual DbSet<Chat> Chats => Set<Chat>();
        public virtual DbSet<ChatMessage> Messages => Set<ChatMessage>();
        public virtual DbSet<ChatGroup> Groups => Set<ChatGroup>();
        public virtual DbSet<ChatGroupMessage> GroupMessages => Set<ChatGroupMessage>();
        public virtual DbSet<UserChatGroup> UserGroups => Set<UserChatGroup>();
    }
}
