namespace LibraryManage.Entity
{
    public class Borrower
    {
        public int BorrowerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? LibraryCardNumber { get; set; }
        public string Address { get; set; }
    }
}
