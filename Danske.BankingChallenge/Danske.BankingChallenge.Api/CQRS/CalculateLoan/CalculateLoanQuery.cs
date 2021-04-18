using Danske.BankingChallenge.Api.Model;
using MediatR;

namespace Danske.BankingChallenge.Api.CQRS.CalculateLoan
{
    public class CalculateLoanQuery: IRequest<CalculateLoanQueryResult>
    {
        public decimal Amount { get; set; }
        public int DurationInYears { get; set; }
        public double AnnualInterestRate { get; set; } = 5.0;
        public InterestRateCalculationType InterestRateType { get; set; } = InterestRateCalculationType.Monthly;
        public decimal AdministrationFeeFixed { get; set; } = 10_000M;
        public double AdministrationFeePercentage { get; set; } = 1.0;
    }
}
