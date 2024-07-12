using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleChatApplication.DAL.Interfaces;

namespace SimpleChatApplication.DAL.Entities {
    public class UserEntity : IEntity {
        public int Id { get; set; }
        public string? UserName { get; set; } = null!;

        public override bool Equals(object? obj) {
            return (obj as UserEntity)?.Id == this.Id;
        }

        public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity> {
            public void Configure(EntityTypeBuilder<UserEntity> builder) {
                builder.HasKey(x => x.Id);

                builder.Property(x => x.UserName)
                    .IsRequired()
                    .HasMaxLength(256);
            }
        }
    }
}
