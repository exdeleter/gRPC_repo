using Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RequestResultConfiguration : IEntityTypeConfiguration<RequestResult>
{
    public void Configure(EntityTypeBuilder<RequestResult> builder)
    {
        builder.ToTable("request_results");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.RequestId)
            .HasColumnName("request_id");

        builder.Property(x => x.Key)
            .HasColumnName("key");

        builder.Property(x => x.Value)
            .HasColumnName("value");
    }
}