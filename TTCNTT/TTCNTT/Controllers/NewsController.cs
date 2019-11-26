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
            var onePageOfNews = _dbContext.News.OrderByDescending(h => h.CreatedDate).ToPagedList(pageNumber, 9);
            ViewBag.onePageOfNews = onePageOfNews;

            NewsViewModel model = new NewsViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);

            return View(model);
        }

        [Route("loai-tin-tuc/{id}")]
        public async Task<IActionResult> NewsType(string id, int? page)
        {
            NewsViewModel model = new NewsViewModel();
            model.newsType = await _dbContext.NewsType.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            var pageNumber = page ?? 1;
            var onePageOfNews = _dbContext.News.Where(h => h.FkNewsTypeId == model.newsType.Id).OrderByDescending(h => h.CreatedDate).ToPagedList(pageNumber, 9);
            ViewBag.OnePageOfNews = onePageOfNews;

            return View(model);
        }

        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> NewsDetail(string id)
        {
            NewsViewModel model = new NewsViewModel();
            model.news = await _dbContext.News.SingleOrDefaultAsync(h => h.Slug_Title == id);
            model.newsType = await _dbContext.NewsType.Where(h => h.Id == model.news.FkNewsTypeId).FirstOrDefaultAsync();
            model.listNewsType = await _dbContext.NewsType.ToListAsync();
            model.listNews = await _dbContext.News.Where(h => h.FkNewsTypeId == model.newsType.Id).OrderByDescending(h => h.CreatedDate).ToListAsync();
            model.listNewsComment = await _dbContext.NewsComment.Where(h => h.FkNewsId == model.news.Id).OrderBy(h => h.CreatedDate).ToListAsync();

            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            return View(model);
        }

        [HttpPost]
        [Route("NewsComment")]
        public async Task<IActionResult> NewsComment([FromForm] NewsViewModel vmItem)
        {               
            NewsComment comment = new NewsComment();

            try
            {
                comment.Id = Guid.NewGuid().ToString();
                comment.FkNewsId = vmItem.fkNewsId;
                comment.Name = vmItem.Name;
                comment.Email = vmItem.Email;
                comment.Phone = vmItem.Phone;
                comment.Comment = vmItem.Content;
                comment.IsRead = false;
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

        [Route("NewsSearch")]
        public async Task<IActionResult> NewsSearch(string search)
        {
            return RedirectToAction("tim-kiem", "tin-tuc", new { id = search });
        }

        [Route("tim-kiem/{id}")]
        public async Task<IActionResult> Search(string id, int? page)
        {
            var pageNumber = page ?? 1;
            var onePageOfNews = _dbContext.News.Where(h => h.Title.Contains(id)).OrderByDescending(h => h.CreatedDate).ToPagedList(pageNumber, 9);

            ViewBag.OnePageOfNews = onePageOfNews;
            ViewBag.id = id;

            NewsViewModel model = new NewsViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);

            return View(model);
        }
    }
}