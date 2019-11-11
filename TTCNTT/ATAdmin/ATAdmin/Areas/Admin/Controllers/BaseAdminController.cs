using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATAdmin.Efs.Context;
using ATAdmin.Efs.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ATAdmin.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
       
        protected WebTTCNTTContext TTCNTT_Context { get; }

        public BaseAdminController(WebTTCNTTContext _TTCNTTContext)
        {
            TTCNTT_Context = _TTCNTTContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.Identity.IsAuthenticated)
            {
                TTCNTT_Context.LoginUserId = User.Identity.Name;
            }
            else
            {
                TTCNTT_Context.LoginUserId = null;
            }
        }
    }
}