namespace Infrastructure.Data.Configurations;

public class RoomRentalTransactionConfiguration : IEntityTypeConfiguration<RoomRentalTransaction>
{
    public void Configure(EntityTypeBuilder<RoomRentalTransaction> builder)
    {
        builder.ToTable("room_rental_transactions");
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.RoomId)
            .HasColumnName("room_id")
            .IsRequired();
        
        builder.Property(x => x.RentalTransactionId)
            .HasColumnName("rental_transaction_id")
            .IsRequired();
        
        builder.HasOne(x => x.Room)
            .WithMany(r => r.RoomRentalTransactions)
            .HasForeignKey(x => x.RoomId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.RentalTransaction)
            .WithMany(rt => rt.RoomRentalTransactions)
            .HasForeignKey(x => x.RentalTransactionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
