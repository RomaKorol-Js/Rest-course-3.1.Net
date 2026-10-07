using Domain.Card;
using Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations; // Виправлено namespace

public class CardsConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasColumnType("varchar(255)").IsRequired();
        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(x => x.Description).HasColumnType("varchar(512)").IsRequired();
        builder.Property(x => x.Cost).HasColumnType("integer").IsRequired();
        builder.Property(x => x.Attack).HasColumnType("integer").IsRequired();
        builder.Property(x => x.Health).HasColumnType("integer").IsRequired();


        builder.Property(x => x.CreatedAt)
            .HasConversion(new DateTimeUtcConverter())
            .HasDefaultValueSql("timezone('utc', now())")
            .IsRequired();
    }
}