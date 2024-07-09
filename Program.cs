using Microsoft.Extensions.Configuration;

namespace LendingPlatform
{
    internal class Program
    {
        private static void DisplayStats(List<ApplicationResult> history)
        {
            Console.WriteLine($"There have been {history.Count} applications. {history.Count(a => a.IsApproved)} approved, {history.Count(a => !a.IsApproved)} rejected.");

            Console.WriteLine($"The total of loans written is {history.Where(a => a.IsApproved).Sum(a => a.LoanApplication.LoanAmount):C0}");
            var averageLtv = history.Average(a => a.LoanApplication.LoanToValueRatio);
            Console.WriteLine($"Mean/Average LTV of all applications is {averageLtv * 100:F2}%");
        }

        private static LoanApplication GetLoanApplication()
        {
            var result = new LoanApplication();
            Console.Write("Requested Loan amount:  ");
            result.LoanAmount = int.Parse(Console.ReadLine());
            Console.Write("Collateral value:       ");
            result.CollateralValue = int.Parse(Console.ReadLine());
            Console.Write("Credit score:           ");
            result.CreditScore = int.Parse(Console.ReadLine());

            return result;
        }

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            IConfiguration config = builder.Build();

            var ruleset = config.GetRequiredSection(nameof(Ruleset)).Get<Ruleset>();

            var loanApprover = new LoanApprovalService(ruleset!);
            var history = new List<ApplicationResult>();
            while (true)
            {
                Console.WriteLine("Lending Platform");
                Console.WriteLine("================");
                Console.WriteLine();

                var app = GetLoanApplication();
                var isApproved = loanApprover.IsLoanApproved(app);

                Console.WriteLine();
                Console.WriteLine($"This application is {(isApproved ? "APPROVED" : "REJECTED")}");
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