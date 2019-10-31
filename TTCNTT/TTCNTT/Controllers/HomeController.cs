using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TTCNTT.Efs.Context;
using TTCNTT.Models;

namespace TTCNTT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WebTTCNTTContext _dbContext;

        public HomeController(ILogger<HomeController> logger, WebTTCNTTContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.aboutus = await _dbContext.AboutUs.FirstOrDefaultAsync(h => h.Skill == "0");
            model.listAboutUsSkill = await _dbContext.AboutUs.Where(p => p.Skill == "1").ToListAsync();
            model.listNews = await _dbContext.News.ToListAsync();
            model.listProduct = await _dbContext.Product.ToListAsync();
            model.listService = await _dbContext.Service.ToListAsync();
            model.listCourse = await _dbContext.Course.ToListAsync();
            model.listEmployee = await _dbContext.Employee.ToListAsync();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
