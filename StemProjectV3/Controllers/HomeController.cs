using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StemProjectV3.Models;
using Microsoft.EntityFrameworkCore;
using StemProjectV3.Data;
using StemProjectV3.Models.SchoolViewModels;

namespace StemProjectV3.Controllers
{
    public class HomeController : Controller
    {
        private readonly SchoolContext _context;

        public HomeController(SchoolContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            IQueryable<GradDateGroup> data =
            from student in _context.Students
            group student by student.GradDate into dateGroup
            select new GradDateGroup()
            {
                GradDate = dateGroup.Key,
                StudentCount = dateGroup.Count()
            };

            return View(await data.AsNoTracking().ToListAsync());
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
