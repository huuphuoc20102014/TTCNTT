using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;

namespace TTCNTT.Models
{
    public class ServiceViewModel
    {
        public Menu menu { get; set; }
        public Service service { get; set; }
        public List<Service>  listService { get; set; }
    }
}
