using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Entities;

namespace TTCNTT.Models
{
    public class EmployeeViewModel
    {
        public Employee employee { get; set; }
        public EmployeeType employeeType { get; set; }
        public List<Employee> listEmployee { get; set; }
    }
}
