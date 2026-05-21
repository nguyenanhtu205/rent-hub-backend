namespace Infrastructure.Data.Configurations;

public class StaffConfiguration : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("staff");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(x => x.Phone)
            .HasColumnName("phone")
            .HasMaxLength(20)
            .IsRequired();
        
        builder.Property(x => x.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.ActiveWorkCount)
            .HasColumnName("active_work_count")
            .IsRequired();
        
        builder.Property(x => x.AccountId)
            .HasColumnName("account_id")
            .IsRequired();
        
        builder.HasOne(x => x.Account)
            .WithOne(x => x.Staff)
            .HasForeignKey<Staff>(x => x.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
