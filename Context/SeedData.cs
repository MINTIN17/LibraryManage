//using LibraryManage.Entity;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Bogus;
//using Microsoft.AspNetCore.Http.Features;
//using LibraryManage.Entity.Enum;
//using LibraryManage.Constants;

//namespace LibraryManage.Context;

//public static class SeedData
//{
//    public static void AddIdentitySeedData(this ModelBuilder modelBuilder)
//    {
//        var roles = GetDefaultRoles();
//        modelBuilder.Entity<IdentityRole<int>>().HasData(roles);
//        var admin = GetAdmin();
//        modelBuilder.Entity<Borrower>().HasData(admin);
//        modelBuilder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>()
//        {
//            RoleId = roles[0].Id,
//            UserId = admin.Id
//        });
//        var borrower = GetBorrower();
//        modelBuilder.Entity<Borrower>().HasData(borrower);
//        modelBuilder.Entity<IdentityUserRole<int>>().HasData(borrower.Select(b => new IdentityUserRole<int>()
//        {
//            RoleId = roles[1].Id,
//            UserId = b.Id
//        }));
//        var libraryItems = GetLibraryItem();
//        SeedType(libraryItems, modelBuilder);

//        GetBorrowingHistory(borrower, libraryItems, modelBuilder);
//    }

//    private static Borrower GetAdmin()
//    {
//        return new Borrower()
//        {
//            Id = 1,
//            Name = "Admin",
//            Email = "admin@gmail.com",
//            NormalizedEmail = "ADMIN@GMAIL.COM",
//            UserName = "admin",
//            NormalizedUserName = "ADMIN",
//            Address = "Admin Address",
//            PhoneNumber = "123456789",
//            EmailConfirmed = true,
//            SecurityStamp = Guid.NewGuid().ToString(),
//            PasswordHash = new PasswordHasher<Borrower>().HashPassword(null!, "Admin@123"),
//            LibraryCardId = Guid.NewGuid()
//        };
//    }
//    private static List<IdentityRole<int>> GetDefaultRoles()
//    {
//        var roles = new List<IdentityRole<int>>
//        {
//            new() { Id = 1, Name = LibraryManagementRole.Admin, NormalizedName = LibraryManagementRole.Admin.ToUpper() },
//            new() { Id = 2, Name = LibraryManagementRole.User, NormalizedName = LibraryManagementRole.User.ToUpper() }
//        };
//        return roles;
//    }
//    private static void SeedType(List<LibraryItem> items, ModelBuilder modelBuilder)
//    {
//        var random = new Random();
//        foreach (var item in items)
//        {
//            switch (random.Next(1, 4))
//            {
//                case 1:
//                    modelBuilder.Entity<Book>().HasData(new Book()
//                    {
//                        Id = item.Id,
//                        Author = item.Author,
//                        Title = item.Title,
//                        PublicationDate = item.PublicationDate,
//                        Quantity = item.Quantity,
//                        NumberOfPages = random.Next(50, 150)
//                    });
//                    break;
//                case 2:
//                    modelBuilder.Entity<Dvd>().HasData(new Dvd()
//                    {
//                        Id = item.Id,
//                        Author = item.Author,
//                        Title = item.Title,
//                        PublicationDate = item.PublicationDate,
//                        Quantity = item.Quantity,
//                        Runtime = random.Next(50, 150).ToString()
//                    });
//                    break;
//                case 3:
//                    modelBuilder.Entity<Magazine>().HasData(new Magazine()
//                    {
//                        Id = item.Id,
//                        Author = item.Author,
//                        Title = item.Title,
//                        PublicationDate = item.PublicationDate,
//                        Quantity = item.Quantity,
//                    });
//                    break;
//                default:
//                    break;
//            }
//            {

//            }
//        }

//    }

//    private static void GetBorrowingHistory(List<Borrower> borrower, List<LibraryItem> libraryItems,
//        ModelBuilder modelBuilder)
//    {
//        var random = new Random();
//        var BorrowingHistories = new Faker<BorrowingHistory>()
//            .RuleFor(bh => bh.Id, f => f.IndexFaker + 1)
//            .RuleFor(bh => bh.LibraryCardId, f => f.PickRandom(borrower).LibraryCardId)
//            .RuleFor(bh => bh.LibraryItemId, f => f.PickRandom(libraryItems).Id)
//            .RuleFor(bh => bh.BorrowDate, f => f.PickRandom(libraryItems).PublicationDate.AddDays(random.Next(50, 100)))
//            .RuleFor(bh => bh.DueDate, (f, bh) => bh.BorrowDate.AddDays(random.Next(10, 30)))
//            .RuleFor(bh => bh.Status, f => f.PickRandom<BorrowingHistoryStatus>())
//            .Generate(25);
//        modelBuilder.Entity<BorrowingHistory>().HasData(BorrowingHistories);
//    }


//    private static List<LibraryItem> GetLibraryItem()
//    {
//        return new Faker<LibraryItem>()
//            .RuleFor(item => item.Id, f => f.IndexFaker + 1)
//            .RuleFor(item => item.Author, f => f.Name.FullName())
//            .RuleFor(item => item.Title, f => f.Lorem.Sentence(4))
//            .RuleFor(item => item.PublicationDate, f => f.Date.Past(2, DateTime.Now.AddYears(-2)))
//            .RuleFor(item => item.Quantity, f => f.Random.Number(5, 10))
//            .Generate(40);
//    }

//    private static List<Borrower> GetBorrower()
//    {
//        return new Faker<Borrower>()
//            .RuleFor(u => u.Id, f => f.IndexFaker + 2)
//            .RuleFor(u => u.Name, f => f.Name.FullName())
//            .RuleFor(u => u.Email, f => f.Internet.Email())
//            .RuleFor(u => u.NormalizedEmail, (_,
//                u) => u.Email?.ToUpper())
//            .RuleFor(u => u.UserName, (_,
//                u) => u.Email)
//            .RuleFor(u => u.NormalizedUserName, (_,
//                u) => u.UserName?.ToUpper())
//            .RuleFor(u => u.Address, f => f.Address.StreetAddress())
//            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
//            .RuleFor(u => u.EmailConfirmed, _ => true)
//            .RuleFor(u => u.SecurityStamp, _ => Guid.NewGuid().ToString())
//            .RuleFor(u => u.PasswordHash, _ => new PasswordHasher<Borrower>().HashPassword(null!, "User@123"))
//            .RuleFor(u => u.LibraryCardId, (f, u) => {
//                var result = Guid.NewGuid();
//                return result;
//            })
//            .Generate(20);
//    }
//}