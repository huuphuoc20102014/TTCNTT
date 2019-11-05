using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTCNTT.Efs.Context;
using TTCNTT.Efs.Entities;
using TTCNTT.Models;
using X.PagedList;

namespace TTCNTT.Controllers
{
    [Route("tin-tuc")]
    public class NewsController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public NewsController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IActionResult>  Index(int? page)
        {
            var pageNumber = page ?? 1;
            var onePageOfNews = _dbContext.News.ToPagedList(pageNumber, 9);
            ViewBag.onePageOfNews = onePageOfNews;

            NewsViewModel model = new NewsViewModel();
            model.menu = await _dbContext.Menu.FirstOrDefaultAsync(h => h.Slug_Name == "tin-tuc");

            return View(model);
        }

        [Route("loai-tin-tuc/{id}")]
        public async Task<IActionResult> NewsType(string id, int? page)
        {
            NewsViewModel model = new NewsViewModel();
            model.newsType = await _dbContext.NewsType.FirstOrDefaultAsync(h => h.Slug_Name == id);

            var pageNumber = page ?? 1;
            var onePageOfNews = _dbContext.News.Where(h => h.FkNewsTypeId == model.newsType.Id).ToPagedList(pageNumber, 9);
            ViewBag.OnePageOfNews = onePageOfNews;

            return View(model);
        }

        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> NewsDetail(string id)
        {
            NewsViewModel model = new NewsViewModel();
            model.news = await _dbContext.News.SingleOrDefaultAsync(h => h.Slug_Title == id);
            model.menu = await _dbContext.Menu.FirstOrDefaultAsync(h => h.Slug_Name == "tin-tuc-su-kien");
            model.newsType = await _dbContext.NewsType.Where(h => h.Id == model.news.FkNewsTypeId).FirstOrDefaultAsync();
            model.listNewsType = await _dbContext.NewsType.ToListAsync();
            model.listNews = await _dbContext.News.Where(h => h.FkNewsTypeId == model.newsType.Id).ToListAsync();
            model.listNewsComment = await _dbContext.NewsComment.Where(h => h.FkNewsId == model.news.Id).ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewsComment(string name, string email, string phone, string content, string fknewsid)
        {               
            NewsComment comment = new NewsComment();

            try
            {
                comment.Id = Guid.NewGuid().ToString();
                comment.FkNewsId = fknewsid;
                comment.Name = name;
                comment.Email = email;
                comment.Phone = phone;
                comment.Comment = content;
                comment.CreatedBy = "Customer";
                comment.CreatedDate = DateTime.Now;
                comment.RowStatus = 0;

                _dbContext.NewsComment.Add(comment);
                await _dbContext.SaveChangesAsync();
                TempData["NewsComment"] = "Đăng thành công.";

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(new { errorMessage = ex.ToString() });
            }

        }
    }
}