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
    [Route("khoa-hoc")]
    public class CourseController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public CourseController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var onePageOfCourses = _dbContext.Course.OrderByDescending(h => h.CreatedDate).ToPagedList(pageNumber, 9);

            ViewBag.OnePageOfCourses = onePageOfCourses;

            CourseViewModel model = new CourseViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);

            return View(model);
        }

        [Route("loai-khoa-hoc/{id}")]
        public async Task<IActionResult> CourseType(string id, int? page)
        {
            CourseViewModel model = new CourseViewModel();
            model.courseType = await _dbContext.CourseType.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            var pageNumber = page ?? 1;
            var onePageOfCourses = _dbContext.Course.Where(h => h.FkCourseTypeId == model.courseType.Id).OrderByDescending(h => h.CreatedDate).ToPagedList(pageNumber, 9);

            ViewBag.OnePageOfCourses = onePageOfCourses;

            return View(model);
        }

        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> CourseDetail(string id)
        {
            CourseViewModel model = new CourseViewModel();
            model.course = await _dbContext.Course.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.courseType = await _dbContext.CourseType.FirstOrDefaultAsync(p => p.Id == model.course.FkCourseTypeId);
            model.listCourse = await _dbContext.Course.Where(h => h.FkCourseTypeId == model.courseType.Id && h.Id != model.course.Id).OrderByDescending(h => h.CreatedDate).ToListAsync();
            model.listCourseType1 = await _dbContext.CourseType.ToListAsync();

            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            return View(model);
        }

        public class RegisterInform
        {
            public String Name { get; set; }
            public String Email { get; set; }
            public String Phone { get; set; }
            public String Member { get; set; }
            public String Content { get; set; }
            public String CourseId { get; set; }
        }

        [HttpPost]
        [Route("CourseRegister")]
        public async Task<IActionResult> CourseRegister([FromBody] RegisterInform register)
        {
            Contact contact = new Contact();

            try
            {
                contact.Id = Guid.NewGuid().ToString();
                contact.Name = register.Name;
                contact.Email = register.Email;
                contact.Phone = register.Phone;
                contact.CourseMember = Int32.Parse(register.Member);
                contact.Body = register.Content;
                contact.FkCourseId = register.CourseId;
                contact.CreatedBy = "Customer";
                contact.CreatedDate = DateTime.Now;
                contact.RowStatus = 0;

                _dbContext.Contact.Add(contact);
                await _dbContext.SaveChangesAsync();
                TempData["CourseRegister"] = "Gửi đăng ký thành công.";

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(new { errorMessage = ex.ToString() });
            }

        }

        [Route("CourseSearch")]
        public async Task<IActionResult> CourseSearch(string search)
        {
            return RedirectToAction("tim-kiem", "khoa-hoc", new { id = search });
        }

        [Route("tim-kiem/{id}")]
        public async Task<IActionResult> Search(string id, int? page)
        {
            var pageNumber = page ?? 1;
            var onePageOfCourses = _dbContext.Course.Where(h => h.Name.Contains(id)).OrderByDescending(h => h.CreatedDate).ToPagedList(pageNumber, 9);

            ViewBag.OnePageOfCourses = onePageOfCourses;
            ViewBag.id = id;

            CourseViewModel model = new CourseViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);

            return View(model);
        }
    }
}