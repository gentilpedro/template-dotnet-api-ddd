using DddApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DddApiTemplate.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.CustomerName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(o => o.CreatedAtUtc);

        builder.OwnsMany(o => o.Items, items =>
        {
            items.ToTable("OrderItems");
            items.WithOwner().HasForeignKey(i => i.OrderId);
            items.HasKey(i => i.Id);

            items.Property(i => i.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            items.Property(i => i.Quantity);

            items.OwnsOne(i => i.UnitPrice, price =>
            {
                price.Property(p => p.Amount).HasColumnName("UnitPrice").HasPrecision(18, 2);
                price.Property(p => p.Currency).HasColumnName("Currency").HasMaxLength(3);
            });
        });

        builder.Navigation(o => o.Items).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(o => o.Total);
        builder.Ignore(o => o.DomainEvents);
    }
}
