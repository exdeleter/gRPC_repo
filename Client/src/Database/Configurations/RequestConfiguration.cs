using Domain.Entities;

namespace Database.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RequestConfiguration : IEntityTypeConfiguration<RequestEntity>
{
    public void Configure(EntityTypeBuilder<RequestEntity> builder)
    {
        builder.ToTable("requests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>();

        builder.Property(x => x.Progress)
            .HasColumnName("progress");

        builder.Property(x => x.ErrorMessage)
            .HasColumnName("error_message");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasDefaultValueSql("NOW()");

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasDefaultValueSql("NOW()");

        builder.HasMany(x => x.Results)
            .WithOne(r => r.RequestEntity)
            .HasForeignKey(r => r.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}