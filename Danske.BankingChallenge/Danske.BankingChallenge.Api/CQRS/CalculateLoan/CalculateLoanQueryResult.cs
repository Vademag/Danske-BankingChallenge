namespace Danske.BankingChallenge.Api.CQRS.CalculateLoan
{
    public class CalculateLoanQueryResult
    {
        public double EffectiveAPR { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal TotalAmountPaidInterestRate { get; set; }
        public decimal TotalAmountPaidInFees { get; set; }
    }
}