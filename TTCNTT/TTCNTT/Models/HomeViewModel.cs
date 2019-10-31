using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;

namespace TTCNTT.Models
{
    public class HomeViewModel
    {
        public AboutUs aboutus { get; set; }
        public List<AboutUs> listAboutUs { get; set; }
        public List<News> listNews { get; set; }
        public List<Product> listProduct { get; set; }
        public List<Service> listService { get; set; }
        public List<Course> listCourse { get; set; }
    }
}
