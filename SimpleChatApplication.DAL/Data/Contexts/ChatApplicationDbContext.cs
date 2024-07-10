using Microsoft.EntityFrameworkCore;
using SimpleChatApplication.DAL.Entities;

namespace SimpleChatApplication.DAL.Data.Contexts {
    public class ChatApplicationDbContext : DbContext {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ChatRoomEntity> ChatRooms { get; set; }

        public ChatApplicationDbContext(DbContextOptions<ChatApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new UserEntity.UserEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ChatRoomEntity.ChatRoomEntityConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
