using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;
using TTCNTT.Helpers;

namespace TTCNTT.Models
{
    public class CourseViewModel
    {
        public Course course { get; set; }
        public List<Course> listCourse { get; set; }
        public CourseType courseType { get; set; }
        public List<CourseType> listCourseType1 { get; set; }
        public Training khoahoc { get; set; }
        public SettingHelper setting { get; set; }
    }
}
