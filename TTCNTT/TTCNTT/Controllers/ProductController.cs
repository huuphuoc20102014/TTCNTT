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
    public class ProductController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public ProductController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("san-pham")]
        //[HttpGet("san-pham")]
        public async Task<IActionResult>  Index()
        {
            ProductViewModel model = new ProductViewModel();
            model.menu = await _dbContext.Menu.FirstOrDefaultAsync(h => h.Slug_Name == "san-pham");
            model.listProduct = await _dbContext.Product.ToListAsync();

            return View(model);
        }
        [HttpGet("san-pham/chi-tiet-san-pham/{id}")]
        public async Task<IActionResult> ProductDetail(string id)
        {
            ProductViewModel model = new ProductViewModel();
            model.product = await _dbContext.Product.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.category = await _dbContext.Category.FirstOrDefaultAsync(h => h.Id == model.product.FkProductId);
            model.listProduct = await _dbContext.Product.Where(h => h.FkProductId == model.category.Id).ToListAsync();
            model.listCategory = await _dbContext.Category.ToListAsync();
            model.listProductComment = await _dbContext.ProductComment.Where(h => h.FkProductId == model.product.Id).ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> ProductCategory(string id)
        {
            ProductViewModel model = new ProductViewModel();
            model.category = await _dbContext.Category.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.listProduct = await _dbContext.Product.Where(h => h.FkProductId == model.category.Id).ToListAsync();

            return View(model);
        }

        [HttpPost]
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

        
        public async Task<IActionResult> ProductSearch(string search)
        {
            ProductViewModel model = new ProductViewModel();
            model.listProduct = await _dbContext.Product.Where(h => h.Name.Contains(search)).ToListAsync();

            return View(model);
        }

    }
}