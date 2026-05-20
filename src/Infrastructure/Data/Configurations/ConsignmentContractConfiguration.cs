namespace Infrastructure.Data.Configurations;

public class ConsignmentContractConfiguration : IEntityTypeConfiguration<ConsignmentContract>
{
    public void Configure(EntityTypeBuilder<ConsignmentContract> builder)
    {
        builder.ToTable("consignment_contracts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.SigningDate)
            .HasColumnName("signing_date")
            .HasColumnType("timestamptz")
            .IsRequired(false);

        builder.Property(x => x.DurationInMonths)
            .HasColumnName("duration_in_months")
            .IsRequired();

        builder.Property(x => x.RemainingDeposit)
            .HasColumnName("remaining_deposit")
            .IsRequired();

        builder.Property(x => x.CommissionRate)
            .HasColumnName("commission_rate")
            .IsRequired();

        builder.Property(x => x.Terms)
            .HasColumnName("terms")
            .HasColumnType("text")
            .IsRequired();

        builder.Property(x => x.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.PropertyId)
            .HasColumnName("property_id")
            .IsRequired();

        builder.HasOne(x => x.Property)
            .WithOne(x => x.ConsignmentContract)
            .HasForeignKey<ConsignmentContract>(x => x.PropertyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
