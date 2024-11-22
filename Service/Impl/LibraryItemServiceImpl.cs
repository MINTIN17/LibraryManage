using LibraryManage.Models;
using LibraryManage.Repository;
using LibraryManage.Entity;

namespace LibraryManage.Service.Impl
{
    public class LibraryItemServiceImpl : ILibraryItemService
    {
        private readonly LibraryItemRepository _repository;

        public LibraryItemServiceImpl(LibraryItemRepository libraryItemRepository) 
        {
            _repository = libraryItemRepository;
        }

        public async Task<LibraryItem> GetLibraryItemByIdAsync(int id)
        {
            return await _repository.GetItemByIdAsync(id);
        }

        public async Task<(List<LibraryItem> items, int totalPages, string searchQuery, string sortBy, string sortOrder)> GetAllItemsAsync(int pageNumber, string searchQuery, string sortBy, string sortOrder)
        {
            return await _repository.GetAllItemsAsync(pageNumber, searchQuery, sortBy, sortOrder);
        }

        public async Task<bool> AddItemAsync(string ItemName, string Author, DateTime PublicationDate,
            int Quantity, string Category, string Description, string Image, int NumberOfPages, int RunTime, string Genre)
        {
            if (await _repository.CheckExistItem(ItemName, Category))
            {
                LibraryItem newItem = Category switch
                {
                    "Book" => new Book
                    {
                        Title = ItemName,
                        Author = Author,
                        PublicationDate = PublicationDate,
                        Quantity = Quantity,
                        Category = Category,
                        Description = Description,
                        Image = Image,
                        Genre = Genre,
                        NumberOfPage = NumberOfPages
                    },
                    "DVD" => new DVD
                    {
                        Title = ItemName,
                        Author = Author,
                        PublicationDate = PublicationDate,
                        Quantity = Quantity,
                        Category = Category,
                        Description = Description,
                        Image = Image,
                        Genre = Genre,
                        RunTime = RunTime
                    },
                    "Magazine" => new Magazine
                    {
                        Title = ItemName,
                        Author = Author,
                        PublicationDate = PublicationDate,
                        Quantity = Quantity,
                        Category = Category,
                        Description = Description,
                        Image = Image
                    },
                    _ => throw new ArgumentException("Invalid category")
                };

                return (await _repository.AddItemAsync(newItem));
            }
            return false;
        }

        public async Task<bool> UpdateItemAsync(int itemId, string ItemName, string Author, DateTime PublicationDate,
    int Quantity, string Category, string Description, string Image, int NumberOfPages, int RunTime, string Genre)
        {
            var existingItem = await _repository.GetItemByIdAsync(itemId);
            if (existingItem == null)
            {
                return false;
            }

            existingItem.Title = ItemName;
            existingItem.Author = Author;
            existingItem.PublicationDate = PublicationDate;
            existingItem.Quantity = Quantity;
            existingItem.Category = Category;
            existingItem.Description = Description;
            existingItem.Image = Image;

            switch (Category)
            {
                case "Book":
                    var bookItem = existingItem as Book;
                    if (bookItem != null)
                    {
                        bookItem.NumberOfPage = NumberOfPages;
                        bookItem.Genre = Genre;
                    }
                    break;
                case "DVD":
                    var dvdItem = existingItem as DVD;
                    if (dvdItem != null)
                    {
                        dvdItem.RunTime = RunTime;
                        dvdItem.Genre = Genre;
                    }
                    break;
                case "Magazine":
                    break;
                default:
                    throw new ArgumentException("Danh mục không hợp lệ");
            }

            return await _repository.UpdateItemAsync(existingItem);
        }


        public async Task<bool> DeleteItemAsync(int id)
        {
            return await _repository.DeleteItemAsync(id);
        }

        public async Task<bool> BorrowItemAsync(int itemId, string borrowerId)
        {
            BorrowHistory borrowHistory = new BorrowHistory
            {
                LibraryItemId = itemId,
                BorrowerLibraryCardNumber = borrowerId,
                BorrowDate = DateTime.Now,
                ReturnDate = null,
            };
            await _repository.DecreaseQuantity(itemId);
            return (await _repository.BorrowItem(borrowHistory));
        }
        public async Task<bool> ReturnItemAsync(int itemId, string borrowerId)
        {

            await _repository.IncreaseQuantity(itemId);
            return (await _repository.ReturnItem(itemId, borrowerId));
        }
    }
}
