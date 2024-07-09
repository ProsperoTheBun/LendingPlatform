namespace LendingPlatform
{
    public class LoanApplication
    {
        public int CollateralValue { get; set; }
        public int CreditScore { get; set; }
        public int LoanAmount { get; set; }
        public decimal LoanToValueRatio => CollateralValue != 0 ? (decimal)LoanAmount / CollateralValue : 0M;
    }
}