using CloudinaryDotNet;
using LibraryManage.Service;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace LibraryManage.Controllers
{
    public class BorrowerController : Controller
    {
        private readonly IBorrowerService _borrowerService;
        public BorrowerController(IBorrowerService borrowerService)
        {
            _borrowerService = borrowerService;
        }
        [Route("/home/borrowes")]
        public async Task<IActionResult> AllBorrower()
        {
            var borrowers = await _borrowerService.GetAllBorrowersAsync();
            return View(borrowers);
        }

        public async Task<IActionResult> AddBorrower(string Name, string Address, string Email)
        {
            bool isAdded = await _borrowerService.AddBorrowerAsync(Name, Address, Email);

            if (isAdded)
            {
                return RedirectToAction("AllBorrower");
            }
            else
            {
                ViewData["Name"] = Name;
                ViewData["Address"] = Address;
                ViewData["Email"] = Email;
                ViewBag.Error = "Email đã tồn tại";

                var borrowers = await _borrowerService.GetAllBorrowersAsync();
                return View("AllBorrower", borrowers);
            }
        }

        public async Task<IActionResult> DeleteBorrower(int id)
        {
            if (await _borrowerService.DeleteBorrowerAsync(id))
                return RedirectToAction("AllBorrower");
            return View("AllBorrower");
        }

        public async Task<IActionResult> UpdateBorrower(int id, string Name, string Address)
        {
            if(await _borrowerService.UpdateBorrowerAsync(id, Name, Address))
                return RedirectToAction("AllBorrower");
            return View("AllBorrower");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
