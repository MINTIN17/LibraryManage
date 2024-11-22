using LibraryManage.DTO.Responses;
using LibraryManage.Repository;

namespace LibraryManage.Service.Impl
{
    public class BorrowHistoryServiceImpl : IBorrowHistoryService
    {
        private readonly BorrowHistoryRepository _repository;

        public BorrowHistoryServiceImpl(BorrowHistoryRepository borrowHistoryRepository)
        {
            _repository = borrowHistoryRepository;
        }
        public async Task<List<BorrowHistoryTitle>> GetAllHistory(string card, string status)
        {
            return await _repository.GetAllHistory(card, status);
        }
    }
}
