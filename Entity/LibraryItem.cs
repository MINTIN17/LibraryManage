

using System.ComponentModel.DataAnnotations;

namespace LibraryManage.Entity;

public class LibraryItem
{
    [Key]
    public int LibraryItemId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public DateTime PublicationDate { get; set; }
    public string Image { get; set; }
    public int Quantity { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
    public ICollection<BorrowHistory> BorrowingHistories { get; set; }

}

