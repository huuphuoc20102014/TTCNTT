using System;
using System.Collections.Generic;

namespace ATAdmin.Efs.Entities
{
    public partial class View_Roles : AtBaseECommerceEntity
    {
        public string IdTaiKhoan { get; set; }
        public string RoleId { get; set; }
        public string Quyen { get; set; }
    }
}
