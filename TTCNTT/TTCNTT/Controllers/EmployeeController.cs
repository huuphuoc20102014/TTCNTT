using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTCNTT.Efs.Context;
using TTCNTT.Efs.Entities;
using TTCNTT.Models;

namespace TTCNTT.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public EmployeeController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("doi-ngu-giang-vien")]
        public async Task<IActionResult> Index()
        {
            EmployeeViewModel model = new EmployeeViewModel();
            model.employeeType = await _dbContext.EmployeeType.FirstOrDefaultAsync(h => h.Id == "ET03");
            model.listEmployee = await _dbContext.Employee.Where(p => p.Fk_EmplyeeId == "ET03").ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> EmployeeDetail(string id)
        {
            EmployeeViewModel model = new EmployeeViewModel();
            model.employee = await _dbContext.Employee.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.employeeType = await _dbContext.EmployeeType.FirstOrDefaultAsync(h => h.Id == "ET03");
            model.listEmployee = await _dbContext.Employee.Where(p => p.Fk_EmplyeeId == "ET03").ToListAsync();
            return View(model);
        }
    }
}