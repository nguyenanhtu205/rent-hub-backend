namespace Infrastructure.Data.Configurations;

public class RentalTransactionConfiguration : IEntityTypeConfiguration<RentalTransaction>
{
    public void Configure(EntityTypeBuilder<RentalTransaction> builder)
    {
      builder.ToTable("rental_transactions");
      
      builder.HasKey(x => x.Id);

      builder.Property(x => x.Id)
          .HasColumnName("id")
          .ValueGeneratedOnAdd();

        builder.Property(x => x.Price)
            .HasColumnName("price")
            .IsRequired();
        
        builder.Property(x => x.CommissionRate)
            .HasColumnName("commission_rate")
            .IsRequired();
        
        builder.Property(x => x.ClosedTime)
            .HasColumnName("closed_time")
            .HasColumnType("timestamptz")
            .IsRequired();
        
        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.StaffId)
            .HasColumnName("staff_id")
            .IsRequired();
        
        builder.Property(x => x.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();
        
        builder.Property(x => x.PropertyId)
            .HasColumnName("property_id")
            .IsRequired();
        
        builder.Property(x => x.CommissionAmount)
            .HasColumnName("commission_amount")
            .IsRequired();
        
        builder.HasOne(x => x.Staff)
            .WithMany(s => s.RentalTransactions)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Customer)
            .WithMany(c => c.RentalTransactions)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
