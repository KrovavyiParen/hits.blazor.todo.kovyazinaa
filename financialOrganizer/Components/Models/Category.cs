namespace financialOrganizer.Components.Models
{
    public class Category
    {
        public int Id { get; set; }
        public TransactionName Name { get; set; }
        public TransactionType Type { get; set; }
    }

    public enum TransactionType
    {        
        Income,
        Expense,
    }

    public enum TransactionName
    {
        Еда = 1,
        Такси = 2,
        Общественный_транспорт = 3,
        Фастфуд = 4,
        Переводы = 5,
        Цифровые_товары = 6,
        Мобильная_связь = 7,
        Супермаркеты = 8,
        Образование = 9,
    }
}
