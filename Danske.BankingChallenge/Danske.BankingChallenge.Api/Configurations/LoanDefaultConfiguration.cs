using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Danske.BankingChallenge.Api.Configurations
{
    public class LoanDefaultConfiguration
    {
        public decimal AnnualInterestRate { get; set; }
        public InterestRateCalculationType InterestRateCalculationType { get; set; }

        public decimal AdministrationFeeFixed { get; set; }
        public decimal AdminsitrationFeePercentage { get; set; }
    }

    public enum InterestRateCalculationType
    {
        Monthly,
        Quarterly,
        SemiAnnually,
        Annually
    }
}
