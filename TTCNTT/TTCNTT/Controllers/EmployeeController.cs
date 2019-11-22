using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTCNTT.Efs.Context;
using TTCNTT.Efs.Entities;
using TTCNTT.Helpers;
using TTCNTT.Models;
using X.PagedList;

namespace TTCNTT.Controllers
{
    [Route("giang-vien")]
    public class EmployeeController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public EmployeeController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IActionResult> Index(int? page)
        {
            EmployeeViewModel model = new EmployeeViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            var pageNumber = page ?? 1;
            var onePageOfEmployees = _dbContext.Employee.Where(p => p.Fk_EmplyeeId == "ET03").ToPagedList(pageNumber, 6);

            ViewBag.OnePageOfEmployees = onePageOfEmployees;

            return View(model);
        }

        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> EmployeeDetail(string id)
        {
            EmployeeViewModel model = new EmployeeViewModel();
            model.employee = await _dbContext.Employee.FirstOrDefaultAsync(h => h.Id == id);
            model.listEmployee = await _dbContext.Employee.Where(p => p.Fk_EmplyeeId == "ET03").ToListAsync();

            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            return View(model);
        }

        [Route("EmployeeSearch")]
        public async Task<IActionResult> EmployeeSearch(string search)
        {
            return RedirectToAction("tim-kiem", "giang-vien", new { id = search });
        }

        [Route("tim-kiem/{id}")]
        public async Task<IActionResult> Search(string id, int? page)
        {
            var pageNumber = page ?? 1;
            var onePageOfEmployees = _dbContext.Employee.Where(h => h.Name.Contains(id) && h.Fk_EmplyeeId == "ET03").ToPagedList(pageNumber, 2);

            ViewBag.OnePageOfEmployees = onePageOfEmployees;
            ViewBag.id = id;

            EmployeeViewModel model = new EmployeeViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);

            return View(model);
        }
    }
}