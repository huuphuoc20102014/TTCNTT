using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class ViewUserRole : AtBaseECommerceEntity
    {
        public string TenQuyen { get; set; }
        public string TenNguoiDung { get; set; }
        public string IdUser { get; set; }
        public string IdRole { get; set; }
    }
}
