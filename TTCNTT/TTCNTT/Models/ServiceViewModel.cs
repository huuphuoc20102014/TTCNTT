using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;
using TTCNTT.Helpers;

namespace TTCNTT.Models
{
    public class ServiceViewModel
    {
        public Menu menu { get; set; }
        public Service service { get; set; }
        public List<Service>  listService { get; set; }
        public SettingHelper setting { get; set; }
    }
}
