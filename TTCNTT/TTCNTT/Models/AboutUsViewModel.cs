using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;

namespace TTCNTT.Models
{
    public class AboutUsViewModel
    {
        public AboutUs about { get; set; }
        public List<Employee> listEmployees { get; set; }
        public List<AboutUs> listAboutSkill { get; set; }
    }
}
