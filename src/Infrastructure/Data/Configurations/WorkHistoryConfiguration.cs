namespace Infrastructure.Data.Configurations;

public class WorkHistoryConfiguration : IEntityTypeConfiguration<WorkHistory>
{
    public void Configure(EntityTypeBuilder<WorkHistory> builder)
    {
        builder.ToTable("work_histories");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Time)
            .HasColumnName("time")
            .HasColumnType("timestamptz")
            .IsRequired();
        
        builder.Property(x => x.Note)
            .HasColumnName("note")
            .HasColumnType("text")
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
            .WithMany(x => x.WorkHistories)
            .HasForeignKey(x => x.StaffId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(x => x.Customer)
            .WithMany(x => x.WorkHistories)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
