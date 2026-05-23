namespace Domain.Contracts;

public sealed record ContractClause
{
    public required string Title { get; init; }

    public required string Content { get; init; }
}
