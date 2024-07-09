namespace LendingPlatform
{
    public class LoanApprovalService
    {
        private readonly Ruleset ruleset;

        public LoanApprovalService(Ruleset ruleset)
        {
            this.ruleset = ruleset;
        }

        public bool IsLoanApproved(LoanApplication loanApplication)
        {
            if (loanApplication.LoanAmount > ruleset.MaxLoanAmount || loanApplication.LoanAmount < ruleset.MinLoanAmount)
            {
                return false;
            }

            return ruleset.Bands
                .Where(r => r.IsInclusive 
                    ? r.MaxLoanAmount >= loanApplication.LoanAmount 
                    : r.MaxLoanAmount > loanApplication.LoanAmount)
                .Where(r => r.MinCreditScore <= loanApplication.CreditScore)
                .Where(r => r.IsInclusive 
                    ? r.MaxLoanToValueRatio >= loanApplication.LoanToValueRatio 
                    : r.MaxLoanToValueRatio > loanApplication.LoanToValueRatio)
                .Any();
        }
    }
}