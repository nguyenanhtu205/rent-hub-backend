namespace Infrastructure.Data.Configurations;

public class FinancialTransactionConfiguration : IEntityTypeConfiguration<FinancialTransaction>
{
    public void Configure(EntityTypeBuilder<FinancialTransaction> builder)
    {
        builder.ToTable("financial_transactions");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Amount)
            .HasColumnName("amount")
            .IsRequired();
        
        builder.Property(x => x.Method)
            .HasColumnName("method")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Date)
            .HasColumnName("date")
            .HasColumnType("timestamptz")
            .IsRequired();
        
        builder.Property(x => x.RefType)
            .HasColumnName("ref_type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.RefId)
            .HasColumnName("ref_id")
            .IsRequired();
        
        builder.Property(x => x.StaffId)
            .HasColumnName("staff_id")
            .IsRequired();
        
        builder.HasOne(x => x.Staff)
            .WithMany(s => s.FinancialTransactions)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
