using LibraryManage.Entity;

namespace LibraryManage.Service
{
    public interface IBorrowerService
    {
        Task<List<Borrower>> GetAllBorrowersAsync();
        Task<bool> AddBorrowerAsync(string name, string address, string email);
        Task<bool> DeleteBorrowerAsync(int id);
        Task<bool> UpdateBorrowerAsync(int id, string name, string email);
    }
}
