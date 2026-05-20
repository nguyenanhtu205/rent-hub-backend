namespace Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.Phone)
            .HasColumnName("phone")
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(x => x.CitizenId)
            .HasColumnName("citizen_id")
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.AccountId)
            .HasColumnName("account_id")
            .IsRequired();
        
        builder.HasOne(x => x.Account)
            .WithOne(a => a.Customer)
            .HasForeignKey<Customer>(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
