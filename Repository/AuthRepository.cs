using LibraryManage.Context;
using LibraryManage.Entity;
using LibraryManage.Models; // Assuming that you have a User model
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryManage.Repository
{
    public class AuthRepository
    {
        private readonly LibraryDbContext _context;

        public AuthRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExistBorrower(string email)
        {
            var existingUser = _context.Borrowers.FirstOrDefault(u => u.Name == email);
            if (existingUser != null)
            {
                return false; 
            }
            return true;
        }

        public async Task<bool> RegisterBorrower(string name, string email, string address, string libraryCardNumber)
        {
            var borrower = new Borrower
            {
                Name = name,
                Email = email,
                LibraryCardNumber = libraryCardNumber,
                Address = address
            };
            try
            {
                await _context.Borrowers.AddAsync(borrower);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
