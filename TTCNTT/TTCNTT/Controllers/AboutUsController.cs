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

namespace TTCNTT.Controllers
{
    
    public class AboutUsController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public AboutUsController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("gioi-thieu")]
        public async Task<IActionResult> Index()
        {
            AboutUsViewModel model = new AboutUsViewModel();
            model.about = await _dbContext.AboutUs.FirstOrDefaultAsync(p => p.Skill == "0");
            model.listAboutSkill = await _dbContext.AboutUs.Where(h => h.Skill == "1").ToListAsync();
            model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetListEmployee()
        {
            try
            {
                var listEmployee = await _dbContext.Employee.AsNoTracking()
                        .Where(h => h.Fk_EmplyeeId == "ET03")
                        .OrderBy(h => h.Id)
                        .Select(h => new AboutUsDto
                        {
                            Id = h.Id,
                            Name = h.Name,
                            ImageSlug = h.ImageSlug,
                            Specialize = h.Specialize,
                            LongDescription_Html = h.LongDescription_Html
                        })
                        .ToListAsync();

                return Json(new TTJsonResult(true, listEmployee));
            }
            catch (Exception ex)
            {
                return Json(new TTJsonResult(false, null));
            }
        }
    }
    public class AboutUsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription_Html { get; set; }
        public string LongDescription_Html { get; set; }
        public string ImageSlug { get; set; }


        public string Specialize { get; set; }
    }
}