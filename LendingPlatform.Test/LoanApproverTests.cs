using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LendingPlatform.Test
{

    [TestClass]
    public class LoanApproverTests
    {
        private static Ruleset DefaultRuleset = new Ruleset
        {
            MaxLoanAmount = 1_500_000,
            MinLoanAmount = 100_000,
            Bands = [
                new BandingRule {
                    MaxLoanAmount = 1_000_000,
                    MinCreditScore = 750,
                    MaxLoanToValueRatio = 0.6M,
                },
                new BandingRule {
                    MaxLoanAmount = 1_000_000,
                    MinCreditScore = 800,
                    MaxLoanToValueRatio = 0.8M,
                },
                new BandingRule {
                    MaxLoanAmount = 1_000_000,
                    MinCreditScore = 900,
                    MaxLoanToValueRatio = 0.9M,
                },
                new BandingRule {
                    IsInclusive = true,
                    MaxLoanAmount = 1_500_000,
                    MinCreditScore = 950,
                    MaxLoanToValueRatio = 0.6M,
                },
            ],
        };

        [DataTestMethod]
        // Rejected
        [DataRow(400, 1, 1, false)]
        [DataRow(100, 1000000, 2000000, false)]
        [DataRow(999, 1200000, 1300000, false)]
        [DataRow(790, 350000, 500000, false)]
        [DataRow(800, 500000, 600000, false)]
        [DataRow(820, 1000000, 1200000, false)]
        [DataRow(900, 450000, 500000, false)]
        // Approved
        [DataRow(820, 350000, 500000, true)]
        [DataRow(750, 300000, 500001, true)]
        [DataRow(900, 450000, 500001, true)]
        public void IsLoanApproved_ExpectedResult(int score, int loan, int collateral, bool expected)
        {
            // Arrange

            var loanApplication = new LoanApplication
            {
                CreditScore = score,
                LoanAmount = loan,
                CollateralValue = collateral
            };
            var sut = new LoanApprovalService(DefaultRuleset);

            // Act
            var result = sut.IsLoanApproved(loanApplication);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void IsLoanApproved_LoanHigherThenThreshold_Rejected()
        {
            // Arrange

            var loanApplication = new LoanApplication
            {
                CreditScore = 999,
                LoanAmount = 2000000,
                CollateralValue = 100000
            };
            var sut = new LoanApprovalService(DefaultRuleset);

            // Act
            var result = sut.IsLoanApproved(loanApplication);

            // Assert
            Assert.IsFalse(result);
        }
    }
}