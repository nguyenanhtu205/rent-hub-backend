using System.Reflection;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    
    public DbSet<ConsignmentContract> ConsignmentContracts => Set<ConsignmentContract>();
    
    public DbSet<ConsignmentContractDocument> ConsignmentContractDocuments => Set<ConsignmentContractDocument>();
    
    public DbSet<Customer> Customers => Set<Customer>();
    
    public DbSet<FinancialTransaction> FinancialTransactions => Set<FinancialTransaction>();
    
    public DbSet<Property> Properties => Set<Property>();
    
    public DbSet<PropertyDocument> PropertyDocuments => Set<PropertyDocument>();
    
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    
    public DbSet<RentalTransaction> RentalTransactions => Set<RentalTransaction>();
    
    public DbSet<Room> Rooms => Set<Room>();
    
    public DbSet<RoomRentalTransaction> RoomRentalTransactions => Set<RoomRentalTransaction>();
    
    public DbSet<Staff> Staffs => Set<Staff>();
    
    public DbSet<StaffWorkingArea> StaffWorkingAreas => Set<StaffWorkingArea>();
    
    public DbSet<WorkHistory> WorkHistories => Set<WorkHistory>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
