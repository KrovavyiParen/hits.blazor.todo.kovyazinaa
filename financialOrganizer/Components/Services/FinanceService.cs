using financialOrganizer.Components.Models;

namespace financialOrganizer.Components.Services
{
    public interface IFinanceService
    {
        Task<List<Transaction>> GetTransactionsAsync(DateTime? startDate, DateTime? endDate);
        Task<Transaction> AddTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(Transaction transaction);
        Task<FinancialReport> GenerateReportAsync(ReportPeriod period);
        Task<List<Category>> GetCategoriesAsync();
        Task<Transaction> UpdateTransactionAsync(Transaction transaction);
    }

    public class FinanceService: IFinanceService
    {
        private readonly List<Transaction> _transactions = new();
        private readonly List<Category> _categories = new();

        public async Task<FinancialReport> GenerateReportAsync(ReportPeriod period)
        {
            var filteredTransactions = await GetTransactionsAsync(period.StartDate, period.EndDate);

            var report = new FinancialReport
            {
                ReportPeriodStart = period.StartDate,
                ReportPeriodEnd = period.EndDate,
                TotalIncome = filteredTransactions
                    .Where(t => t.Type == TransactionType.Income)
                    .Sum(t => t.Amount),
                TotalExpenses = filteredTransactions
                    .Where(t => t.Type == TransactionType.Expense)
                    .Sum(t => t.Amount)
            };

            var categoryGroups = filteredTransactions
                .GroupBy(t => t.CategoryId);

            foreach (var group in categoryGroups)
            {
                var category = _categories.First(c => c.Id == group.Key);
                var totalAmount = group.Sum(t => t.Amount);
                var totalTypeAmount = filteredTransactions
                    .Where(t => t.Type == category.Type)
                    .Sum(t => t.Amount);

                report.CategoryBreakdown.Add(new CategorySummary
                {
                    CategoryName = category.Name,
                    Amount = totalAmount,
                    Percentage = totalTypeAmount > 0 ? (totalAmount / totalTypeAmount) * 100 : 0,
                    Type = category.Type
                });
            }

            var sixMonthsAgo = DateTime.Now.AddMonths(-6);
            var monthlyTransactions = _transactions
                .Where(t => t.Date >= sixMonthsAgo)
                .ToList();

            var monthlyGroups = monthlyTransactions
                .GroupBy(t => new { t.Date.Year, t.Date.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month);

            foreach (var group in monthlyGroups)
            {
                report.MonthlyTrends.Add(new MonthlySummary
                {
                    Year = group.Key.Year,
                    Month = group.Key.Month,
                    Income = group.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
                    Expenses = group.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount)
                });
            }

            return report;
        }

        public async Task<List<Transaction>> GetTransactionsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {

            var query = _transactions.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(t => t.Date.Date >= startDate.Value.Date);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.Date.Date <= endDate.Value.Date);
            }

            var result = query.OrderByDescending(t => t.Date).ToList();

            return await Task.FromResult(result);
        }

        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction.Amount <= 0)
                throw new ArgumentException("Сумма должна быть больше 0");

            if (string.IsNullOrWhiteSpace(transaction.Description))
                throw new ArgumentException("Описание не может быть пустым");

            if (transaction.Date == default)
                transaction.Date = DateTime.Now;

            if (transaction.Id == 0)
            {
                var maxId = _transactions.Count > 0 ? _transactions.Max(t => t.Id) : 0;
                transaction.Id = maxId + 1;
            }

            _transactions.Add(transaction);

            return await Task.FromResult(transaction);
        }

        public async Task<Transaction> UpdateTransactionAsync(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            var existingTransaction = _transactions.FirstOrDefault(t => t.Id == transaction.Id);
            if (existingTransaction == null)
                throw new KeyNotFoundException($"Транзакция с ID {transaction.Id} не найдена");

            if (transaction.Amount <= 0)
                throw new ArgumentException("Сумма должна быть больше 0");

            if (string.IsNullOrWhiteSpace(transaction.Description))
                throw new ArgumentException("Описание не может быть пустым");

            existingTransaction.Amount = transaction.Amount;
            existingTransaction.Date = transaction.Date;
            existingTransaction.Description = transaction.Description;
            existingTransaction.CategoryId = transaction.CategoryId;
            existingTransaction.Type = transaction.Type;

            return await Task.FromResult(existingTransaction);
        }

        public async Task DeleteTransactionAsync(Transaction transaction)
        {
            _transactions.Remove(transaction);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await Task.FromResult(_categories);
        }
    }
}
