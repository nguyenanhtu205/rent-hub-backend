using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Account> Accounts { get; }
    
    DbSet<ConsignmentContract> ConsignmentContracts { get; }
    
    DbSet<ConsignmentContractDocument> ConsignmentContractDocuments { get; }
    
    DbSet<Customer> Customers { get; }
    
    DbSet<FinancialTransaction> FinancialTransactions { get; }
    
    DbSet<Property> Properties { get; }
    
    DbSet<PropertyDocument> PropertyDocuments { get; }
    
    DbSet<RefreshToken> RefreshTokens { get; }
    
    DbSet<RentalTransaction> RentalTransactions { get; }
    
    DbSet<Room> Rooms { get; }
    
    DbSet<RoomRentalTransaction> RoomRentalTransactions { get; }
    
    DbSet<Staff> Staffs { get; }
    
    DbSet<StaffWorkingArea> StaffWorkingAreas { get; }
    
    DbSet<WorkHistory> WorkHistories { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
