namespace financialOrganizer.Components.Models
{
    public class FinancialReport
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance {  get; set; }
        public List<CategorySummary> CategoryBreakdown { get; set; } = new();
        public List<MonthlySummary> MonthlyTrends { get; set; } = new();
        public DateTime ReportPeriodStart { get; set; }
        public DateTime ReportPeriodEnd { get; set; }
    }

    public class CategorySummary
    {
        public TransactionName CategoryName { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
        public TransactionType Type { get; set; }
    }

    public class MonthlySummary
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public decimal Balance { get; set; }
    }

    public class ReportPeriod
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ReportType Type { get; set; }
    }

    public enum ReportType
    {
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Custom
    }
}
