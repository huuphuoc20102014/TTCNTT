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
    [Route("dich-vu")]
    public class ServiceController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public ServiceController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var onePageOfServices = _dbContext.Service.ToPagedList(pageNumber, 6);

            ViewBag.OnePageOfServices = onePageOfServices;

            ServiceViewModel model = new ServiceViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            return View(model);
        }

        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> ServiceDetail(string id)
        {
            ServiceViewModel model = new ServiceViewModel();
            model.service = await _dbContext.Service.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.listService = await _dbContext.Service.ToListAsync();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);

            return View(model);
        }

        [Route("ServiceSearch")]
        public async Task<IActionResult> ServiceSearch(string search)
        {
            return RedirectToAction("tim-kiem", "dich-vu", new { id = search });
        }

        [Route("tim-kiem/{id}")]
        public async Task<IActionResult> Search(string id, int? page)
        {
            var pageNumber = page ?? 1;
            var onePageOfServices = _dbContext.Service.Where(h => h.ServiceName.Contains(id)).ToPagedList(pageNumber, 6);

            ViewBag.OnePageOfServices = onePageOfServices;
            ViewBag.id = id;

            ServiceViewModel model = new ServiceViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            return View(model);
        }
    }
}