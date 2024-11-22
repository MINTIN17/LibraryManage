using LibraryManage.DTO.Responses;

namespace LibraryManage.Service
{
    public interface IBorrowHistoryService
    {
        Task<List<BorrowHistoryTitle>> GetAllHistory(string card, string status);
    }
}
