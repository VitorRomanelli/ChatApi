using ChatApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Persistence.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<ChatMessage>().HasOne(e => e.SenderUser).WithMany(x  => x.SendedMessages);
            builder.Entity<ChatMessage>().HasOne(e => e.RecipientUser).WithMany(x  => x.ReceivedMessages);
        }

        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<ChatMessage> Messages { get; set; }
        public virtual DbSet<ChatGroup> Groups { get; set; }
        public virtual DbSet<ChatGroupMessage> GroupMessages { get; set; }
        public virtual DbSet<UserChatGroup> UserGroups { get; set; }
    }
}
