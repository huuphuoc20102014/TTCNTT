using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TTCNTT.Efs.Context;
using TTCNTT.Efs.Entities;
using TTCNTT.Helpers;
using TTCNTT.Models;
using X.PagedList;

namespace TTCNTT.Controllers
{
    [Route("san-pham")]
    public class ProductController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public ProductController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IActionResult>  Index(int? page)
        {
            var pageNumber = page ?? 1;
            var onePageOfProducts = _dbContext.Product.ToPagedList(pageNumber, 9);

            ViewBag.OnePageOfProducts = onePageOfProducts;

            ProductViewModel model = new ProductViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);

            return View(model);
        }

        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> ProductDetail(string id)
        {
            ProductViewModel model = new ProductViewModel();
            model.product = await _dbContext.Product.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.category = await _dbContext.Category.FirstOrDefaultAsync(h => h.Id == model.product.FkProductId);
            model.listProduct = await _dbContext.Product.Where(h => h.FkProductId == model.category.Id).ToListAsync();
            model.listCategory = await _dbContext.Category.ToListAsync();
            model.listProductComment = await _dbContext.ProductComment.Where(h => h.FkProductId == model.product.Id).ToListAsync();

            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            return View(model);
        }

        [Route("loai-san-pham/{id}")]
        public async Task<IActionResult> ProductCategory(string id, int? page)
        {
            ProductViewModel model = new ProductViewModel();
            model.category = await _dbContext.Category.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            var pageNumber = page ?? 1;
            var category = await _dbContext.Category.FirstOrDefaultAsync(h => h.Slug_Name == id);
            var onePageOfProducts = _dbContext.Product.Where(h =>h.FkProductId == category.Id.ToString()).ToPagedList(pageNumber, 9);

            ViewBag.OnePageOfProducts = onePageOfProducts;

            return View(model);
        }

        [HttpPost]
        [Route("NewProductComment")]
        public async Task<IActionResult> NewProductComment(string name, string email, string phone, string content, string fkProductId)
        {
            ProductComment comment = new ProductComment();

            try
            {
                comment.Id = Guid.NewGuid().ToString();
                comment.FkProductId = fkProductId;
                comment.Name = name;
                comment.Email = email;
                comment.Phone = phone;
                comment.Comment = content;
                comment.CreatedBy = "Customer";
                comment.CreatedDate = DateTime.Now;
                comment.RowStatus = 0;

                _dbContext.ProductComment.Add(comment);
                await _dbContext.SaveChangesAsync();
                TempData["NewsComment"] = "Đăng thành công.";

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(new { errorMessage = ex.ToString() });
            }

        }

        [Route("ProductSearch")]
        public async Task<IActionResult> ProductSearch(string search)
        {
            return RedirectToAction("tim-kiem", "san-pham", new { id = search });
        }

        [Route("tim-kiem/{id}")]
        public async Task<IActionResult> Search(string id, int? page)
        {
            var pageNumber = page ?? 1;
            var onePageOfProducts = _dbContext.Product.Where(h => h.Name.Contains(id)).ToPagedList(pageNumber, 9);

            ViewBag.OnePageOfProducts = onePageOfProducts;
            ViewBag.id = id;

            ProductViewModel model = new ProductViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);

            return View(model);
        }
    }
}