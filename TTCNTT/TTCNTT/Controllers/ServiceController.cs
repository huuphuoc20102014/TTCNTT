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
    public class ServiceController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public ServiceController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("dich-vu")]
        public async Task<IActionResult> Index()
        {
            ServiceViewModel model = new ServiceViewModel();
            model.menu = await _dbContext.Menu.FirstOrDefaultAsync(p => p.Slug_Name == "dich-vu");
            model.listService = await _dbContext.Service.ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> ServiceDetail(string id)
        {
            ServiceViewModel model = new ServiceViewModel();
            model.service = await _dbContext.Service.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.listService = await _dbContext.Service.ToListAsync();
            return View(model);
        }
    }
}