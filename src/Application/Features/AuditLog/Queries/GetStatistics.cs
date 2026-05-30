namespace Application.Features.AuditLog.Queries;

public record FavoriteProperties(
    PropertyType Type,
    District District,
    double Area,
    int NumOfRoom,
    string Address,
    double Price,
    string Url,
    string LessorName,
    string LessorPhone);

public record StatisticsResult(
    int TotalProperties,
    int TotalActiveProperties,
    int TotalLessor,
    int TotalRenter,
    List<double> RevenuePerMonth,
    List<FavoriteProperties> FavoriteProperties);

public record GetStatisticsQuery : IRequest<StatisticsResult>;

public class GetStatisticsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetStatisticsQuery, StatisticsResult>
{
    public async Task<StatisticsResult> Handle(GetStatisticsQuery request, CancellationToken cancellationToken)
    {
        int totalProperties = await context.Properties.AsNoTracking().CountAsync(cancellationToken);

        int totalActiveProperties = await context.Properties
            .AsNoTracking()
            .Where(p => p.Status == PropertyStatus.Active)
            .CountAsync(cancellationToken);

        int totalLessor = await context.Customers
            .AsNoTracking()
            .Where(c => c.Type == CustomerType.Lessor)
            .CountAsync(cancellationToken);

        int totalRenter = await context.Customers
            .AsNoTracking()
            .Where(c => c.Type == CustomerType.Renter)
            .CountAsync(cancellationToken);

        DateTimeOffset now = DateTimeOffset.UtcNow;
        DateTimeOffset startOf12MonthsAgo =
            new(now.AddMonths(-11).Year, now.AddMonths(-11).Month, 1, 0, 0, 0, TimeSpan.Zero);

        var revenueData = await context.FinancialTransactions
            .AsNoTracking()
            .Where(f =>
                (f.Type == FinancialTransactionType.CommissionReceived ||
                 f.Type == FinancialTransactionType.DepositOffset) &&
                f.Date >= startOf12MonthsAgo)
            .GroupBy(f => new { f.Date.Year, f.Date.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Total = g.Sum(f => f.Amount) })
            .ToListAsync(cancellationToken);

        List<double> revenuePerMonth = Enumerable.Range(0, 12)
            .Select(i =>
            {
                DateTimeOffset target = now.AddMonths(-11 + i);
                return revenueData
                    .FirstOrDefault(r => r.Year == target.Year && r.Month == target.Month)
                    ?.Total ?? 0;
            })
            .ToList();

        List<int> top3PropertyIds = await context.RoomRentalTransactions
            .AsNoTracking()
            .GroupBy(rrt => rrt.Room!.PropertyId)
            .Select(g => new { PropertyId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(3)
            .Select(x => x.PropertyId)
            .ToListAsync(cancellationToken);

        List<FavoriteProperties> favoriteProperties = await context.Properties
            .AsNoTracking()
            .Where(p => top3PropertyIds.Contains(p.Id))
            .Select(p => new FavoriteProperties(
                p.Type,
                p.District,
                p.Area,
                p.NumOfRoom,
                p.Address,
                p.Price,
                p.PropertyDocuments.First().Url,
                p.Customer!.Name,
                p.Customer!.Phone))
            .ToListAsync(cancellationToken);

        if (favoriteProperties.Count < 3)
        {
            List<int> existingIds = top3PropertyIds;
            int remaining = 3 - favoriteProperties.Count;

            List<FavoriteProperties> latestProperties = await context.Properties
                .AsNoTracking()
                .Where(p => !existingIds.Contains(p.Id))
                .OrderByDescending(p => p.Id)
                .Take(remaining)
                .Select(p => new FavoriteProperties(
                    p.Type,
                    p.District,
                    p.Area,
                    p.NumOfRoom,
                    p.Address,
                    p.Price,
                    p.PropertyDocuments.First().Url,
                    p.Customer!.Name,
                    p.Customer!.Phone))
                .ToListAsync(cancellationToken);

            favoriteProperties.AddRange(latestProperties);
        }

        return new StatisticsResult
        (
            totalProperties,
            totalActiveProperties,
            totalLessor,
            totalRenter,
            revenuePerMonth,
            favoriteProperties
        );
    }
}
