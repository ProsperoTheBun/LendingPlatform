public class BandingRule
{
    public int MaxLoanAmount { get; set; }

    public decimal MaxLoanToValueRatio { get; set; }

    public int MinCreditScore { get; set; }   
    
    /// <summary>
    /// Flag to determine if LTV and Loan Amount values are inclusive for comparison purposes.
    /// </summary>
    /// <remarks>
    /// Use this flag to determine whether to use "equal to" or "less than or equal to".
    /// </remarks>
    public bool IsInclusive { get; set; }
}