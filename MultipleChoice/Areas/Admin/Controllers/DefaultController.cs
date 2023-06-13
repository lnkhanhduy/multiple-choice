using Microsoft.AspNetCore.Mvc;
using MultipleChoice.Models;
using System.Security.Principal;

namespace MultipleChoice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DefaultController : Controller
    {
        private readonly MultipleChoiceContext _context;

        public DefaultController(MultipleChoiceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("admin") != null)
            {
                //success
                return RedirectToAction("Index", "AdminHome");
            }
            return View();
        }

        //Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var account = _context.Admins.SingleOrDefault(x => x.Username == username && x.Password == password);

            if (account != null)
            {
                //success
                HttpContext.Session.SetString("admin", account.Username);
                return RedirectToAction("Index", "AdminHome");
            }
            else
            {
                //not success
                return View();
            }

        }
    }
}
