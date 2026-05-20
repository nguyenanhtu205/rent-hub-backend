namespace Infrastructure.Data.Configurations;

public class PropertyDocumentConfiguration : IEntityTypeConfiguration<PropertyDocument>
{
    public void Configure(EntityTypeBuilder<PropertyDocument> builder)
    {
        builder.ToTable("property_documents");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Url)
            .HasColumnName("url")
            .HasColumnType("text")
            .IsRequired();
        
        builder.Property(x => x.PropertyId)
            .HasColumnName("property_id")
            .IsRequired();
        
        builder.HasOne(x => x.Property)
            .WithMany(p => p.PropertyDocuments)
            .HasForeignKey(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
