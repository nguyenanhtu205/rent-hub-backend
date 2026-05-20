namespace Infrastructure.Data.Configurations;

public class ConsignmentContractDocumentConfiguration : IEntityTypeConfiguration<ConsignmentContractDocument>
{
    public void Configure(EntityTypeBuilder<ConsignmentContractDocument> builder)
    {
        builder.ToTable("consignment_contract_documents");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(x => x.Url)
            .HasColumnName("url")
            .HasColumnType("text")
            .IsRequired();
        
        builder.Property(x => x.ConsignmentContractId)
            .HasColumnName("consignment_contract_id")
            .IsRequired();
        
        builder.HasOne(x => x.ConsignmentContract)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.ConsignmentContractId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
