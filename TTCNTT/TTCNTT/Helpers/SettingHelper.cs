using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TTCNTT.Efs.Context;
using TTCNTT.Efs.Entities;

namespace TTCNTT.Helpers
{
    public class SettingHelper
    {
        public string FOOTER_CONTACT { get; set; }
        public string FOOTER_CONTACT_ADDRESS { get; set; }
        public string FOOTER_CONTACT_EMAIL { get; set; }
        public string FOOTER_CONTACT_FAX { get; set; }
        public string FOOTER_CONTACT_PHONE { get; set; }
        public string FOOTER_CONTACT_WEB { get; set; }
        public string FOOTER_COPYRIGHT { get; set; }
        public string FOOTER_GIOITHIEU { get; set; }
        public string FOOTER_INFORM { get; set; }
        public string FOOTER_INFORM_DESCRIPTION { get; set; }
        public string FOOTER_LOGO { get; set; }
        public string FOOTER_YOUTUBE { get; set; }

        public string BANNER { get; set; }
        public string TOP_LOGO { get; set; }

        public string CONTACT { get; set; }
        public string CONTACT_SLOGAN { get; set; }


        public string NEWS { get; set; }
        public string NEWS_SLOGAN { get; set; }
        public string NEWS_SLOGAN_2 { get; set; }
        public string NEWS_EVENT { get; set; }
        public string NEWS_TYPE { get; set; }
        public string NEWS_DETAIL { get; set; }

        public string COURSE { get; set; }
        public string COURSE_SLOGAN { get; set; }
        public string COURSE_SLOGAN_2 { get; set; }
        public string COURSE_TYPE { get; set; }
        public string COURSE_DETAIL { get; set; }

        public string EMPLOYEE { get; set; }
        public string EMPLOYEE_SLOGAN { get; set; }
        public string EMPLOYEE_SLOGAN_2 { get; set; }
        public string EMPLOYEE_DETAIL { get; set; }

        public string PRODUCT { get; set; }
        public string PRODUCT_SLOGAN { get; set; }
        public string PRODUCT_DETAIL { get; set; }
        public string PRODUCT_SLOGAN_2 { get; set; }
        public string PRODUCT_CATEGORY_SLOGAN { get; set; }
        

        public string SERVICE { get; set; }
        public string SERVICE_SLOGAN { get; set; }
        public string SERVICE_SLOGAN_2 { get; set; }
        public string SERVICE_TYPE { get; set; }

        public string ABOUTUS { get; set; }
        public string ABOUTUS_SLOGAN { get; set; }

        public string SEARCH { get; set; }


        public static async Task<SettingHelper> ReadServerOptionAsync(WebTTCNTTContext context)

        {
            List<Setting> lsSetting = await context.Setting.ToListAsync();
            SettingHelper serverSetting = new SettingHelper();
            try
            {
                Setting setting;
                foreach (var prop in typeof(SettingHelper).GetProperties())
                {
                    switch (prop.PropertyType.Name)
                    {
                        case "Boolean":
                            setting = lsSetting.FirstOrDefault(u => u.Id == prop.Name);
                            if (setting != null)
                            {
                                bool value;
                                if (!bool.TryParse(setting.Value, out value))
                                {
                                    value = false;
                                }
                                prop.SetValue(serverSetting, value, null);
                            }
                            else
                            {
                                prop.SetValue(serverSetting, false, null);
                            }
                            break;
                        case "Int32":
                            setting = lsSetting.FirstOrDefault(u => u.Id == prop.Name);
                            if (setting != null)
                            {
                                int value;
                                if (!int.TryParse(setting.Value, out value))
                                {
                                    value = 0;
                                }
                                prop.SetValue(serverSetting, value, null);
                            }
                            else
                            {
                                prop.SetValue(serverSetting, 0, null);
                            }
                            break;
                        case "Decimal":
                            setting = lsSetting.FirstOrDefault(u => u.Id == prop.Name);
                            if (setting != null)
                            {
                                decimal value;
                                if (!decimal.TryParse(setting.Value, out value))
                                {
                                    value = 0;
                                }
                                prop.SetValue(serverSetting, value, null);
                            }
                            else
                            {
                                prop.SetValue(serverSetting, Convert.ToDecimal(0), null);
                            }
                            break;
                        case "Double":
                            setting = lsSetting.FirstOrDefault(u => u.Id == prop.Name);
                            if (setting != null)
                            {
                                double value;
                                if (!double.TryParse(setting.Value, out value))
                                {
                                    value = 0;
                                }
                                prop.SetValue(serverSetting, value, null);
                            }
                            else
                            {
                                prop.SetValue(serverSetting, Convert.ToDouble(0), null);
                            }
                            break;
                        case "String":
                            setting = lsSetting.FirstOrDefault(u => u.Id == prop.Name);
                            if (setting != null)
                            {
                                string value = setting.Value;
                                prop.SetValue(serverSetting, value, null);
                            }
                            else
                            {
                                prop.SetValue(serverSetting, string.Empty, null);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serverSetting;
        }
        public async Task SaveServerOptionAsync(WebTTCNTTContext context)
        {
            try
            {
                foreach (var prop in typeof(SettingHelper).GetProperties())
                {
                    Setting setting = context.Setting.SingleOrDefault(u => u.Id == prop.Name);
                    if (setting != null)
                    {
                        setting.Value = prop.GetValue(this, null).ToString();
                    }
                    else
                    {
                        setting = new Setting();
                        setting.Id = prop.Name;
                        setting.Value = prop.GetValue(this, null).ToString();
                        context.Setting.Add(setting);
                    }
                }
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
