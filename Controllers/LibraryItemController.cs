using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LibraryManage.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace LibraryManage.Controllers
{
    public class LibraryItemController : Controller
    {
        private readonly ILibraryItemService _libraryItemService;
        private readonly Cloudinary _cloudinary;

        public LibraryItemController(ILibraryItemService libraryItemService, Cloudinary cloudinary)
        {
            _libraryItemService = libraryItemService;
            _cloudinary = cloudinary;
        }
        public IActionResult Index()
        {
            return RedirectToAction("LibraryItem", "LibraryItem");
        }


        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };

                // Upload file lên Cloudinary
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return Ok(new { url = uploadResult.SecureUrl.ToString() });
                }
                else
                {
                    return BadRequest("Upload failed.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Route("/home/items")]
        public async Task<IActionResult> LibraryItem(int pageNumber = 1, string searchQuery = "", string sortBy = "all", string sortOrder = "asc")
        {
            var (items, totalPages, searchquery, sortby, sortorder) = await _libraryItemService.GetAllItemsAsync(pageNumber, searchQuery, sortBy, sortOrder);
            
            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = pageNumber;
            ViewBag.SearchQuery = searchquery;
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = sortOrder;
            return View(items);
        }

        public async Task<IActionResult> GetItemByIdAsync(int id)
        {
            var item = await _libraryItemService.GetLibraryItemByIdAsync(id);
            return PartialView("_EditItemModal", item);
        }

        public async Task<IActionResult> AddItem(string ItemName, string Author, DateTime PublicationDate,
            int Quantity, string Category, string Description, string Image, int NumberOfPages, int RunTime, string Genre)
        {
            if(await _libraryItemService.AddItemAsync(ItemName, Author, PublicationDate, Quantity, Category, 
                Description, Image, NumberOfPages, RunTime, Genre))
            {
                return RedirectToAction("LibraryItem");
            }

            //ModelState.AddModelError("", "Sản phẩm này đã có trong thư viện.");
            return View("LibraryItem");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateItemAsync(int itemId, string ItemName_edit, string Author_edit, DateTime PublicationDate_edit,
            int Quantity_edit, string Category_edit, string Description_edit, string Image_edit, int NumberOfPages_edit, int Runtime_edit, string Genre_edit)
        {
            if (await _libraryItemService.UpdateItemAsync(itemId, ItemName_edit, Author_edit, PublicationDate_edit,
             Quantity_edit, Category_edit, Description_edit, Image_edit, NumberOfPages_edit, Runtime_edit, Genre_edit))
            {
                return RedirectToAction("LibraryItem");
            }
            ViewBag.Error = "sản phẩm đã tồn tại";
            return View("LibraryItem");
        }

        public async Task<IActionResult> DeleteItem(int id)
        {
            if(await _libraryItemService.DeleteItemAsync(id))
                return RedirectToAction("LibraryItem");
            return View("LibraryItem");
        }

        public async Task<IActionResult> BorrowItem(int itemId, string borrowerCardNumber)
        {
            if (await _libraryItemService.BorrowItemAsync(itemId, borrowerCardNumber))
                return RedirectToAction("LibraryItem");
            return View("LibraryItem");
        }

        public async Task<IActionResult> ReturnItem(int itemReturnId, string borrowerCardNumber)
        {
            if (await _libraryItemService.ReturnItemAsync(itemReturnId, borrowerCardNumber))
                return RedirectToAction("LibraryItem");
            else
            {
                ViewData["numberCard"] = borrowerCardNumber;
                ViewBag.Error = "Người có số thẻ " + borrowerCardNumber  + " không có mượn sách này";
            }
            var (items, totalPages, searchquery, sortby, sortorder) = await _libraryItemService.GetAllItemsAsync(1, "", "all", "asc");

            ViewBag.TotalPages = totalPages;
            ViewBag.PageNumber = 1;
            ViewBag.SearchQuery = searchquery;
            ViewBag.SortBy = sortby;
            ViewBag.SortOrder = "asc";
            return View("LibraryItem", items);
        }

    }
}
