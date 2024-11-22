using LibraryManage.Entity;
using LibraryManage.Repository;
using Microsoft.EntityFrameworkCore;

namespace LibraryManage.Service.Impl
{
    public class BorrowerServiceImpl : IBorrowerService
    {
        private readonly BorrowerRepository _repository;

        public BorrowerServiceImpl(BorrowerRepository borrowerRepository)
        {
            _repository = borrowerRepository;
        }

        public async Task<List<Borrower>> GetAllBorrowersAsync()
        {
            return await _repository.GetAllBorrowersAsync();
        }

        public async Task<bool> AddBorrowerAsync(string name, string address, string email)
        {
            if (await _repository.EmailExistsAsync(email))
            {
                return false;
            }

            var borrower = new Borrower
            {
                Name = name,
                Address = address,
                LibraryCardNumber = "000",
                Email = email
            };

            bool isAdded = await _repository.AddBorrowerAsync(borrower);
            if (!isAdded)
            {
                return false; 
            }

            borrower.LibraryCardNumber = borrower.BorrowerId.ToString("D3");
            return await _repository.UpdateCardNumberAsync(borrower);
        }

        public async Task<bool> DeleteBorrowerAsync(int id)
        {
            return await _repository.DeleteBorrowerAsync(id);
        }

        public async Task<bool> UpdateBorrowerAsync(int id, string name, string address)
        {
            return await _repository.UpdateBorrowerAsync(id, name, address);
        }
    }
}
