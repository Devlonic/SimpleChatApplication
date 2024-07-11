using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SimpleChatApplication.DAL.Entities {
    public class ChatMessageEntity {
        public int Id { get; set; }
        public string? MessageText { get; set; } = null!;

        public int? ChatRoomId { get; set; } = null!;
        public ChatRoomEntity? ChatRoom { get; set; } = null!;

        public int? SenderId { get; set; } = null!;
        public UserEntity? Sender { get; set; } = null!;

        public class ChatMessageEntityConfiguration : IEntityTypeConfiguration<ChatMessageEntity> {
            public void Configure(EntityTypeBuilder<ChatMessageEntity> builder) {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.MessageText)
                    .IsRequired()
                    .HasMaxLength(1024);

                builder.HasOne(x => x.Sender)
                    .WithMany()
                    .HasForeignKey(x => x.SenderId);

                builder.HasOne(x => x.ChatRoom)
                    .WithMany()
                    .HasForeignKey(x => x.ChatRoomId);
            }
        }
    }
}
