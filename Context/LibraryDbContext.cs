using LibraryManage.Entity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace LibraryManage.Context
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        // Định nghĩa các DbSet cho các thực thể của bạn
        public DbSet<LibraryItem> LibraryItems { get; set; }
        public DbSet<Borrower> Borrowers { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<DVD> Dvds { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<BorrowHistory> BorrowHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Cấu hình kế thừa Table-per-Hierarchy (TPH) cho LibraryItem
            builder.Entity<LibraryItem>()
                .ToTable("LibraryItem")
                .HasDiscriminator<string>("Category")
                .HasValue<Book>("Book")
                .HasValue<DVD>("DVD")
                .HasValue<Magazine>("Magazine");

            builder.Entity<Book>()
                .Property(b => b.Genre)
                .HasColumnName("Genre");

            builder.Entity<DVD>()
                .Property(d => d.Genre)
            .HasColumnName("Genre");

            builder.Entity<Borrower>()
                .ToTable("Borrower");

            // Áp dụng cấu hình cho các thực thể
            builder.ApplyConfigurationsFromAssembly(typeof(LibraryDbContext).Assembly);
        }
    }
}