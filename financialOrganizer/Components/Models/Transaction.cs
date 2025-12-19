namespace financialOrganizer.Components.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public TransactionType Type { get; set; }
        public TransactionName TransactionName { get; set; }
    }
}
