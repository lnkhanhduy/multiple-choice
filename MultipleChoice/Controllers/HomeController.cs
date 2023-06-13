using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using MultipleChoice.Models;
using System.Diagnostics;

namespace MultipleChoice.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MultipleChoiceContext _context;

        public HomeController(ILogger<HomeController> logger, MultipleChoiceContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            /*var grades = await _context.Grades.ToListAsync();
            var subjects = await _context.Subjects.ToListAsync();

            ViewBag.Grades = grades;
            ViewBag.Subjects = subjects;*/
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}