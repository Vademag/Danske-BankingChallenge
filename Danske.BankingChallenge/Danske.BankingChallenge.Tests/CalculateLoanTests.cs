using System.Threading;
using System.Threading.Tasks;
using Danske.BankingChallenge.Api.CQRS.CalculateLoan;
using FluentAssertions;
using Xunit;

namespace Danske.BankingChallenge.Tests
{
    public class CalculateLoanTests
    {
        [Theory]
        [InlineData(500_000, 10, 5.13, 5_303.28, 136_393.09, 5_000)]
        [InlineData(500_000, 1, 5.12, 42_803.74, 13_644.89, 5_000)]
        public async Task CalculateLoan_IfDefaultLoanParameters_ShouldProveTheory(decimal amount, short durationInYears, double expectedEffectiveAPR, decimal expectedMonthlyCost, decimal expectedInterestsPaid, decimal expectedFee)
        {
            // Arrange
            var query = new CalculateLoanQuery
            {
                Amount = amount,
                DurationInYears = durationInYears
            };

            var handler = new CalculateLoanQueryHandler();

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            result.EffectiveAPR.Should().Be(expectedEffectiveAPR);
            result.MonthlyCost.Should().Be(expectedMonthlyCost);
            result.TotalAmountPaidInterestRate.Should().Be(expectedInterestsPaid);
            result.TotalAmountPaidInFees.Should().Be(expectedFee);
        }
    }
}
