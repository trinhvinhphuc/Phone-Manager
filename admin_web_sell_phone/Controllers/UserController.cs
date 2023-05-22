using Microsoft.AspNetCore.Mvc;

namespace admin_web_sell_phone.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
