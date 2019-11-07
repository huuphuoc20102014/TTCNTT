﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;
using TTCNTT.Helpers;

namespace TTCNTT.Models
{
    public class EmployeeViewModel
    {
        public Employee employee { get; set; }
        public List<Employee> listEmployee { get; set; }
        public SettingHelper setting { get; set; }
    }
}
