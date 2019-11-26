using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;
using TTCNTT.Helpers;

namespace TTCNTT.Models
{
    public class ContactViewModel
    {
        public SettingHelper setting { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Body { get; set; }
    }
}
