using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ATAdmin
{
    public class BaseEntityResources
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string RowStatus { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string Slug { get; set; }
    }
}
