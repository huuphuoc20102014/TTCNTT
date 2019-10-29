using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;

namespace TTCNTT.Models
{
    public class MenuViewModel
    {
        public List<Menu> listMenu { get; set; }
        public List<Service> listService { get; set; }
        public List<Training> listTraining { get; set; }
        public List<NewsType> listNewsType { get; set; }
        public List<Category> listCategory { get; set; }
    }
}
