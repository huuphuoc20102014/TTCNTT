using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Context;
using TTCNTT.Efs.Entities;
using TTCNTT.Models;

namespace TTCNTT.Helpers
{
    public class MenuHelper
    {
        public static async Task<MenuViewModel> GetDataMenu(WebTTCNTTContext webContext)
        {
            MenuViewModel model = new MenuViewModel();
            model.listMenu = await webContext.Menu.ToListAsync();
            model.listNewsType = await webContext.NewsType.ToListAsync();
            model.listService = await webContext.Service.ToListAsync();
            model.listTraining = await webContext.Training.ToListAsync();
            model.listCategory = await webContext.Category.ToListAsync();

            return model;
        }
    }
}
