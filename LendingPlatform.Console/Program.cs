using Microsoft.Extensions.Configuration;

namespace LendingPlatform
{
    internal class Program
    {
        /// <summary>
        /// Output some running calculations and statistics to the console.
        /// </summary>
        private static void DisplayStats(List<ApplicationResult> history)
        {
            Console.WriteLine($"There have been {history.Count} applications. {history.Count(a => a.IsApproved)} approved, {history.Count(a => !a.IsApproved)} declined.");
            Console.WriteLine($"The total of loans written is {history.Where(a => a.IsApproved).Sum(a => a.LoanApplication.LoanAmount):C0}");
            Console.WriteLine($"Mean/Average LTV of all applications is {history.Average(a => a.LoanApplication.LoanToValueRatio) * 100:F2}%");
        }

        /// <summary>
        /// Prompt the user to enter the loan application details in the console.
        /// </summary>
        private static LoanApplication GetLoanApplication()
        {
            // TODO: validation of all these inputs, and user-friendly error handling
            var result = new LoanApplication();
            Console.Write("Requested Loan amount:  ");
            result.LoanAmount = int.Parse(Console.ReadLine());
            Console.Write("Collateral value:       ");
            result.CollateralValue = int.Parse(Console.ReadLine());
            Console.Write("Credit score:           ");
            result.CreditScore = int.Parse(Console.ReadLine());

            return result;
        }

        /// <summary>
        /// Read and parse the configuration file to obtain the ruleset
        /// </summary>
        private static Ruleset LoadRulesetFromConfig()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            var ruleset = config.GetRequiredSection(nameof(Ruleset)).Get<Ruleset>();

            // TODO: handle empty or invalid configuration gracefully
            return ruleset!;
        }

        private static void Main(string[] args)
        {
            var ruleset = LoadRulesetFromConfig();
            var loanApprover = new LoanApprovalService(ruleset);
            var history = new List<ApplicationResult>();

            while (true)
            {
                Console.WriteLine("Lending Platform");
                Console.WriteLine("================");
                Console.WriteLine();

                var app = GetLoanApplication();
                var isApproved = loanApprover.IsLoanApproved(app);

                Console.WriteLine();
                Console.WriteLine($"This application is {(isApproved ? "APPROVED" : "DECLINED")}");
                Console.WriteLine();

                history.Add(new ApplicationResult { LoanApplication = app, IsApproved = isApproved });
                DisplayStats(history);

                Console.WriteLine();
                Console.WriteLine("Press RETURN to start another loan application.");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }
}