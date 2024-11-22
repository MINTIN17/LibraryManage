namespace LibraryManage.DTO.Responses
{
    public class BorrowHistoryTitle
    {
        public int BorrowHistoryId { get; set; }
        public int LibraryItemId { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string BorrowerLibraryCardNumber { get; set; }
    }
}
