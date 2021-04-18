using System;
using System.Threading;
using System.Threading.Tasks;
using Danske.BankingChallenge.Api.Configurations;
using MediatR;

namespace Danske.BankingChallenge.Api.CQRS.CalculateLoan
{
    public class CalculateLoanQueryHandler: IRequestHandler<CalculateLoanQuery, CalculateLoanQueryResult>
    {
        public Task<CalculateLoanQueryResult> Handle(CalculateLoanQuery request, CancellationToken cancellationToken)
        {
            var interestInPeriod = CalculateInterestInPeriod(request.AnnualInterestRate, request.InterestRateType, request.DurationInYears);
            var fees = CalculateTotalAmountPaidInFees(request.Amount, request.AdministrationFeeFixed, request.AdministrationFeePercentage);
            var effectiveAPR = CalculateEffectiveAPR(request.AnnualInterestRate/100, interestInPeriod.Periods);
            var periodCost = CalculatePeriodCost(request.Amount, interestInPeriod);
            var totalAmountPaidInterestRate = CalculateTotalAmountPaidInterestRate(request.Amount, periodCost, interestInPeriod.Periods);

            var result = new CalculateLoanQueryResult
            {
                EffectiveAPR = Math.Round(effectiveAPR * 100, 2),
                MonthlyCost = Math.Round(periodCost, 2),
                TotalAmountPaidInterestRate = Math.Round(totalAmountPaidInterestRate, 2),
                TotalAmountPaidInFees = Math.Round(fees, 2)
            };

            return Task.FromResult(result);
        }

        private decimal CalculateTotalAmountPaidInFees(decimal amount, decimal administrationFeeFixed, double administrationFeePercentage)
        {
            var administrationFeeCalculated = amount * (decimal)administrationFeePercentage / 100;

            return Math.Min(administrationFeeFixed, administrationFeeCalculated);
        }

        private double CalculateEffectiveAPR(double annualInterestRateFraction, int periods)
        {
            var effectiveAPR = Math.Pow(1 + (annualInterestRateFraction / periods), periods) - 1;
            return effectiveAPR;
        }

        private decimal CalculateTotalAmountPaidInterestRate(decimal amount, decimal periodCost, int periods)
        {
            var interestsAmount = periodCost * periods - amount;
            return interestsAmount;
        }

        private decimal CalculatePeriodCost(decimal amount, InterestInPeriod interestInPeriod)
        {
            var compound = (decimal) Math.Pow(1 + interestInPeriod.PeriodInterestRateFraction, interestInPeriod.Periods);
            var periodCost = amount * ((decimal)interestInPeriod.PeriodInterestRateFraction * compound) / (compound - 1);
            return periodCost;
        }

        private InterestInPeriod CalculateInterestInPeriod(double annualInterestRatePercentage, InterestRateCalculationType interestRateType, int durationInYears)
        {
            var result = new InterestInPeriod();
            var annualInterestRate = annualInterestRatePercentage / 100;

            switch (interestRateType)
            {
                case InterestRateCalculationType.Annually:
                    result.Periods = durationInYears;
                    result.PeriodInterestRateFraction = annualInterestRate;
                    break;
                case InterestRateCalculationType.SemiAnnually:
                    result.Periods = durationInYears * 2;
                    result.PeriodInterestRateFraction = annualInterestRate / 2;
                    break;
                case InterestRateCalculationType.Quarterly:
                    result.Periods = durationInYears * 4;
                    result.PeriodInterestRateFraction = annualInterestRate / 4;
                    break;
                case InterestRateCalculationType.Monthly:
                    result.Periods = durationInYears * 12;
                    result.PeriodInterestRateFraction = annualInterestRate / 12;
                    break;
            }

            return result;
        }
    }

    internal class InterestInPeriod
    {
        public int Periods { get; set; }
        public double PeriodInterestRateFraction { get; set; }
    }
}
