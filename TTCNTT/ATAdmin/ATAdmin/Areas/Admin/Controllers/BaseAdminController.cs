using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATAdmin.Efs.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ATAdmin.Areas.Admin.Controllers
{
    public class BaseAdminController : Controller
    {
       
        protected WebAtSolutionContext AtSolutionContext { get; }

        public BaseAdminController(WebAtSolutionContext atSolutionContext)
        {
            AtSolutionContext = atSolutionContext;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.Identity.IsAuthenticated)
            {
                AtSolutionContext.LoginUserId = User.Identity.Name;
            }
            else
            {
                AtSolutionContext.LoginUserId = null;
            }
        }
    }
}