using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;

namespace TTCNTT.Models
{
    public class NewsViewModel
    {
        public Menu menu { get; set; }
        public News news { get; set; }
        public List<NewsType> listNewsType { get; set; }
        public List<News> listNews { get; set; }
        public NewsType newsType { get; set; }
        public List<NewsComment> listNewsComment { get; set; }
    }
}
