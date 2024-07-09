public class Ruleset
{
    public List<BandingRule> Bands { get; set; } = new List<BandingRule>();

    public int MaxLoanAmount { get; set; }

    public int MinLoanAmount { get; set; }
}