using LibraryManage.Entity;

namespace LibraryManage.Service
{
    public interface ILibraryItemService
    {
        Task<LibraryItem> GetLibraryItemByIdAsync(int it);
        Task<(List<LibraryItem> items, int totalPages, string searchQuery, string sortBy, string sortOrder)> 
            GetAllItemsAsync(int pageNumber, string searchQuery, string sortBy, string sortOrder);
        Task<bool> AddItemAsync(string ItemName, string Author, DateTime PublicationDate,
            int Quantity, string Category, string Description, string Image, int NumberOfPages, int RunTime, string Genre);
        Task<bool> UpdateItemAsync(int itemId, string ItemName, string Author, DateTime PublicationDate,
            int Quantity, string Category, string Description, string Image, int NumberOfPages, int RunTime, string Genre);
        Task<bool> DeleteItemAsync(int id);
        Task<bool> BorrowItemAsync(int itemId, string BorrowerId);
        Task<bool> ReturnItemAsync(int itemId, string BorrowerId);
    }
}
