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
    public class CourseController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public CourseController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("khoa-hoc")]
        public async Task<IActionResult> Index()
        {
            CourseViewModel model = new CourseViewModel();
            model.khoahoc = await _dbContext.Training.FirstOrDefaultAsync(h => h.Slug_Name == "khoa-hoc");
            model.listCourse = await _dbContext.Course.ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> CourseType(string id)
        {
            CourseViewModel model = new CourseViewModel();
            model.courseType = await _dbContext.CourseType.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.listCourse = await _dbContext.Course.Where(h => h.FkProjectTypeId == model.courseType.Id).ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> CourseDetail(string id)
        {
            CourseViewModel model = new CourseViewModel();
            model.course = await _dbContext.Course.FirstOrDefaultAsync(h => h.Slug_Name == id);
            model.courseType = await _dbContext.CourseType.FirstOrDefaultAsync(p => p.Id == model.course.FkProjectTypeId);
            model.listCourse = await _dbContext.Course.Where(h => h.FkProjectTypeId == model.courseType.Id).ToListAsync();
            model.listCourseType1 = await _dbContext.CourseType.ToListAsync();

            return View(model);
        }
    }
}