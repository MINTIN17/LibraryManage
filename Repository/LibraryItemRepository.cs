using LibraryManage.Context;
using LibraryManage.Models;
using LibraryManage.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManage.Repository
{
    public class LibraryItemRepository
    {
        private readonly LibraryDbContext _context;
        public LibraryItemRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<LibraryItem> GetItemByIdAsync(int id)
        {
            return await _context.LibraryItems.FirstOrDefaultAsync(item => item.LibraryItemId == id);
        }

        public async Task<(List<LibraryItem> items, int totalPages, string searchQuery, string sortBy, string sortOrder)> GetAllItemsAsync(int pageNumber, string searchQuery, string sortBy, string sortOrder)
        {
            int pageSize = 15;

            IQueryable<LibraryItem> itemsQuery = _context.LibraryItems;

            if (!string.IsNullOrEmpty(searchQuery))
            {
                itemsQuery = itemsQuery.Where(item => item.Title.Contains(searchQuery));
            }

            if (sortBy != "all")
            {
                itemsQuery = itemsQuery.Where(item => item.Category.Equals(sortBy));
            }

            itemsQuery = sortOrder == "asc" ? itemsQuery.OrderBy(item => item.Title) : itemsQuery.OrderByDescending(item => item.Title);

            var totalItems = await itemsQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = await itemsQuery
                        .Skip((pageNumber - 1) * pageSize) 
                        .Take(pageSize)    
                        .ToListAsync();

            return (items, totalPages, searchQuery, sortBy, sortOrder);
        }

        public async Task<bool> CheckExistItem(string title, string category)
        {
            var existingItem = await _context.LibraryItems
               .FirstOrDefaultAsync(item => item.Title == title && item.Category == category);
            if (existingItem != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddItemAsync(LibraryItem item)
        {
            await _context.LibraryItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateItemAsync(LibraryItem item)
        {
            try
            {
                _context.LibraryItems.Update(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await _context.LibraryItems.FindAsync(id);

            if (item == null)
            {
                return false;
            }

            _context.LibraryItems.Remove(item);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> BorrowItem(BorrowHistory borrowHistory)
        {
            await _context.BorrowHistory.AddAsync(borrowHistory);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReturnItem(int itemId, string borrowerLibraryCardNumber)
        {
            var borrowHistory = await _context.BorrowHistory
                                      .FirstOrDefaultAsync(bh => bh.LibraryItemId == itemId && bh.ReturnDate == null && bh.BorrowerLibraryCardNumber == borrowerLibraryCardNumber);

            if (borrowHistory != null)
            {
                borrowHistory.ReturnDate = DateTime.Now;

                _context.BorrowHistory.Update(borrowHistory);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> IncreaseQuantity(int id)
        {
            var item = await _context.LibraryItems
               .FirstOrDefaultAsync(item => item.LibraryItemId == id);
            if (item != null)
            {
                item.Quantity++;
                await _context.SaveChangesAsync();
                return true;
            }
            else 
                return false;
        }

        public async Task<bool> DecreaseQuantity(int id)
        {
            var item = await _context.LibraryItems
               .FirstOrDefaultAsync(item => item.LibraryItemId == id);
            item.Quantity--;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
