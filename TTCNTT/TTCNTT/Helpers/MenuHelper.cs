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
    public class FooterHelper
    {
        public static async Task<FooterViewModel> GetFooterSetting(TTCNTT.Efs.Context.WebTTCNTTContext context)
        {
            var footerViewModel = new FooterViewModel();

            footerViewModel.setting = await SettingHelper.ReadServerOptionAsync(context);

            return footerViewModel;

        }
    }
}
