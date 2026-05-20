namespace Infrastructure.Data.Configurations;

public class PropertyConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.ToTable("properties");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Area)
            .HasColumnName("area")
            .IsRequired();
        
        builder.Property(x => x.Direction)
            .HasColumnName("direction")
            .HasMaxLength(150)
            .IsRequired();
        
        builder.Property(x => x.NumOfRoom)
            .HasColumnName("num_of_room")
            .IsRequired();
        
        builder.Property(x => x.Address)
            .HasColumnName("address")
            .HasColumnType("text")
            .IsRequired();
        
        builder.Property(x => x.District)
            .HasColumnName("district")
            .HasConversion<string>()
            .HasMaxLength(50)
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
        
        builder.HasOne(x => x.Staff)
            .WithMany(s => s.Properties)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Customer)
            .WithMany(c => c.Properties)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
