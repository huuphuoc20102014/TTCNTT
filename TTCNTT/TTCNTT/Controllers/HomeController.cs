using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TTCNTT.Efs.Context;
using TTCNTT.Efs.Entities;
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
            //model.listAboutUsSkill = await _dbContext.AboutUs.Where(p => p.Skill == "1").ToListAsync();
            model.listNews = await _dbContext.News.ToListAsync();
            model.listProduct = await _dbContext.Product.ToListAsync();
            model.listService = await _dbContext.Service.ToListAsync();
            model.listCourse = await _dbContext.Course.ToListAsync();
            model.listEmployee = await _dbContext.Employee.ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewCustomerRegister(string email)
        {
            CustomerRegister customer = new CustomerRegister();

            try
            {
                TempData["Customer"] = null;
                ViewData["Customer"] = null;
                ViewBag.Customer = null;

                customer.Id = Guid.NewGuid().ToString();
                customer.Email = email;
                customer.CreatedBy = "Customer";
                customer.CreatedDate = DateTime.Now;
                customer.RowStatus = 0;

                _dbContext.CustomerRegister.Add(customer);
                await _dbContext.SaveChangesAsync();

                ViewBag.Customer = "Gửi thành công";
                ViewData["Customer"] = "Gửi thành công";
                TempData["Customer"] = "Gửi thành công.";

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(new { errorMessage = ex.ToString() });
            }
        }
    }
}
