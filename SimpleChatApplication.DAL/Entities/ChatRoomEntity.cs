using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.DAL.Entities {
    public class ChatRoomEntity : IEntity {
        public int Id { get; set; }
        public string? Title { get; set; } = null!;
        public int? CreatorId { get; set; } = null!;
        public UserEntity? Creator { get; set; } = null!;

        public class ChatRoomEntityConfiguration : IEntityTypeConfiguration<ChatRoomEntity> {
            public void Configure(EntityTypeBuilder<ChatRoomEntity> builder) {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.Title)
                    .IsRequired()
                    .HasMaxLength(64);

                builder.HasOne(x => x.Creator)
                    .WithMany()
                    .HasForeignKey(x => x.CreatorId);
            }
        }
    }
}
