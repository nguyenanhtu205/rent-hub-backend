namespace Infrastructure.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.RequestName)
            .HasColumnName("request_name")
            .HasMaxLength(255);
        
        builder.Property(x => x.UserId)
            .HasColumnName("user_id")
            .HasMaxLength(255);
        
        builder.Property(x => x.RequestPayload)
            .HasColumnName("request_payload")
            .HasColumnType("text");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamptz");
    }
}
