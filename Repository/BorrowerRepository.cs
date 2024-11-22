using LibraryManage.Context;
using LibraryManage.Entity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManage.Repository
{
    public class BorrowerRepository
    {
        private readonly LibraryDbContext _context;
        public BorrowerRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Borrowers.AnyAsync(b => b.Email == email);
        }


        public async Task<List<Borrower>> GetAllBorrowersAsync()
        {

            IQueryable<Borrower> itemsQuery = _context.Borrowers;
            var borrowers = await itemsQuery.ToListAsync();

            return borrowers;
        }

        public async Task<bool> AddBorrowerAsync(Borrower borrower)
        {
            await _context.Borrowers.AddAsync(borrower);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteBorrowerAsync(int id)
        {
            var borrower = await _context.Borrowers.FindAsync(id);

            if (borrower == null)
            {
                return false;
            }

            _context.Borrowers.Remove(borrower);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateBorrowerAsync(int id, string name, string address)
        {
            var existingBorrower = await _context.Borrowers
                .FirstOrDefaultAsync(b => b.BorrowerId == id);

            if (existingBorrower == null)
            {
                return false;
            }

            existingBorrower.Name = name;
            existingBorrower.Address = address;

            _context.Borrowers.Update(existingBorrower);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCardNumberAsync(Borrower borrower)
        {

            _context.Borrowers.Update(borrower);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
