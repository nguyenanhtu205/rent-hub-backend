namespace Infrastructure.Data.Configurations;

public class StaffWorkingAreaConfiguration : IEntityTypeConfiguration<StaffWorkingArea>
{
    public void Configure(EntityTypeBuilder<StaffWorkingArea> builder)
    {
        builder.ToTable("staff_working_areas");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.StaffId)
            .HasColumnName("staff_id")
            .IsRequired();
        
        builder.Property(x => x.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.District)
            .HasColumnName("district")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.HasOne(x => x.Staff)
            .WithMany(s => s.WorkingAreas)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
