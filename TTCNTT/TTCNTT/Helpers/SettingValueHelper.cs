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
    public class SettingValueHelper
    {
        public static async Task<SettingViewModel> GetValueSetting(TTCNTT.Efs.Context.WebTTCNTTContext context)
        {
            var footerViewModel = new SettingViewModel();

            footerViewModel.setting = await SettingHelper.ReadServerOptionAsync(context);

            return footerViewModel;

        }
    }
}
