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
            // filter out loans too high or low
            if (loanApplication.LoanAmount > ruleset.MaxLoanAmount || loanApplication.LoanAmount < ruleset.MinLoanAmount)
            {
                return false;
            }

            // ASS: since some of the approval rules have "equal to or greater than" descriptions and others don't,
            // it is necessary to check the rule definition for 'IsInclusive' to determine which comparison operator to use.
            // e.g. "LTV must be 60% or less" is different to "if the LTV is less than 60%"
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