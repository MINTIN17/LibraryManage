using LibraryManage.Context;
using LibraryManage.DTO.Responses;
using LibraryManage.Entity;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace LibraryManage.Repository
{
    public class BorrowHistoryRepository
    {
        private readonly LibraryDbContext _context;
        public BorrowHistoryRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<List<BorrowHistoryTitle>> GetAllHistory(string card, string status)
        {
            var query = from history in _context.BorrowHistory
                        join item in _context.LibraryItems on history.LibraryItemId equals item.LibraryItemId
                        select new BorrowHistoryTitle
                        {
                            BorrowHistoryId = history.BorrowHistoryId,
                            LibraryItemId = history.LibraryItemId,
                            Title = item.Title,
                            Category = item.Category,
                            BorrowDate = history.BorrowDate,
                            ReturnDate = history.ReturnDate,
                            BorrowerLibraryCardNumber = history.BorrowerLibraryCardNumber
                        };

            if (!string.IsNullOrEmpty(card))
            {
                query = query.Where(h => h.BorrowerLibraryCardNumber == card);
            }

            if (status == "notReturned")
            {
                query = query.Where(h => h.ReturnDate == null);
            }
            else if (status == "returned")
            {
                query = query.Where(h => h.ReturnDate != null);
            }

            return await query.ToListAsync();
        }

    }
}
