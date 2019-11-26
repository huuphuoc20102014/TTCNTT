using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;
using TTCNTT.Helpers;

namespace TTCNTT.Models
{
    public class NewsViewModel
    {
        public News news { get; set; }
        public List<NewsType> listNewsType { get; set; }
        public List<News> listNews { get; set; }
        public NewsType newsType { get; set; }
        public List<NewsComment> listNewsComment { get; set; }
        public SettingHelper setting { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public string fkNewsId { get; set; }
    }
}
