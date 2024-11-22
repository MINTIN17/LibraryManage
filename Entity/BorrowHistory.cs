namespace LibraryManage.Entity
{
    public class BorrowHistory
    {
        public int BorrowHistoryId { get; set; }
        public int LibraryItemId { get; set; }
        public string BorrowerLibraryCardNumber { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }

    }
}
