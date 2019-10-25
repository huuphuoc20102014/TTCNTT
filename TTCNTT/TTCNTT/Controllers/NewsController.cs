using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTCNTT.Efs.Context;
using TTCNTT.Models;

namespace TTCNTT.Controllers
{
    public class NewsController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public NewsController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("tin-tuc-su-kien")]
        public async Task<IActionResult>  Index()
        {
            NewsViewModel model = new NewsViewModel();
            model.listNews = await _dbContext.News.ToListAsync();
            model.menu = await _dbContext.Menu.FirstOrDefaultAsync(h => h.Slug_Name == "tin-tuc-su-kien");
            return View(model);
        }

        public async Task<IActionResult> NewsDetail(string id)
        {
            NewsViewModel model = new NewsViewModel();
            model.news = await _dbContext.News.Where(h => h.Id == id).FirstOrDefaultAsync();
            model.menu = await _dbContext.Menu.FirstOrDefaultAsync(h => h.Slug_Name == "tin-tuc-su-kien");
            model.newsType = await _dbContext.NewsType.Where(h => h.Id == model.news.FkNewsTypeId).FirstOrDefaultAsync();
            model.listNewsType = await _dbContext.NewsType.ToListAsync();
            model.listNews = await _dbContext.News.Where(h => h.FkNewsTypeId == model.newsType.Id).ToListAsync();

            return View(model);
        }
    }
}