using LibraryManage.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManage.Controllers
{
    public class BorrowHistoryController : Controller
    {
        private readonly IBorrowHistoryService _borrowHistoryService;

        public BorrowHistoryController(IBorrowHistoryService borrowHistoryService)
        {
            _borrowHistoryService = borrowHistoryService;
        }

        [Route("/home/history")]
        public async Task<IActionResult> AllHistory(string card = "", string status = "all")
        {
            var historys =await _borrowHistoryService.GetAllHistory(card, status);
            ViewBag.Status = status;
            ViewBag.Card = card;
            return View(historys);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
