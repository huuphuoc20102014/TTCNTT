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
            var onePageOfCourses = _dbContext.Course.ToPagedList(pageNumber, 9);

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
            var onePageOfCourses = _dbContext.Course.Where(h => h.FkProjectTypeId == model.courseType.Id).ToPagedList(pageNumber, 9);

            ViewBag.OnePageOfCourses = onePageOfCourses;

            return View(model);
        }

        [Route("chi-tiet/{id}")]
        public async Task<IActionResult> CourseDetail(string id)
        {
            CourseViewModel model = new CourseViewModel();
            model.course = await _dbContext.Course.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.courseType = await _dbContext.CourseType.FirstOrDefaultAsync(p => p.Id == model.course.FkProjectTypeId);
            model.listCourse = await _dbContext.Course.Where(h => h.FkProjectTypeId == model.courseType.Id).ToListAsync();
            model.listCourseType1 = await _dbContext.CourseType.ToListAsync();

            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);


            return View(model);
        }

        [HttpPost]
        [Route("CourseRegister")]
        public async Task<IActionResult> CourseRegister(string name, string email, string phone, int membercount, string content, string fkcourseid)
        {
            Contact contact = new Contact();

            try
            {
                contact.Id = Guid.NewGuid().ToString();
                contact.Name = name;
                contact.Email = email;
                contact.Phone = phone;
                contact.CourseMember = membercount;
                contact.Body = content;
                contact.FkCourseId = fkcourseid;
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
            var onePageOfCourses = _dbContext.Course.Where(h => h.Name.Contains(id)).ToPagedList(pageNumber, 9);

            ViewBag.OnePageOfCourses = onePageOfCourses;
            ViewBag.id = id;

            CourseViewModel model = new CourseViewModel();
            model.setting = model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);

            return View(model);
        }
    }
}