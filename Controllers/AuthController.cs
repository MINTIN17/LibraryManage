using Microsoft.AspNetCore.Mvc;

namespace LibraryManage.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
