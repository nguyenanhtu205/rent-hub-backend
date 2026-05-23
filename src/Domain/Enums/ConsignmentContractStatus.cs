namespace Domain.Enums;

public enum ConsignmentContractStatus
{
    PendingLessorApproval,
    PendingLegalReview,
    PendingFinanceReview,
    PendingManagerApproval,
    Completed,
    Cancelled
}
