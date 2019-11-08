using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATAdmin.Efs.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Slugify;
//using AtECommerce.Efs.Entities;

namespace ATAdmin.Controllers
{
    public class AtBaseController : Controller
    {
        static string[] _arrTiengViet = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
    "đ",
    "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
    "í","ì","ỉ","ĩ","ị",
    "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
    "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
    "ý","ỳ","ỷ","ỹ","ỵ",};
        static string[] _arrNoUnicode = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
    "d",
    "e","e","e","e","e","e","e","e","e","e","e",
    "i","i","i","i","i",
    "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
    "u","u","u","u","u","u","u","u","u","u","u",
    "y","y","y","y","y",};

        static string[] _arrTiengVietUpper;
        static string[] _arrNoUnicodeUpper;

        protected static SlugHelper _slugHelper = new SlugHelper();

        static AtBaseController()
        {
            _arrTiengVietUpper = _arrTiengViet.Select(h => h.ToUpper()).ToArray();
            _arrNoUnicodeUpper = _arrNoUnicode.Select(h => h.ToUpper()).ToArray();
            //System.Reflection.Assembly.GetExecutingAssembly().FullName;
        }

        protected string _loginUserId { get; set; } = "System";
        protected string CheckAndGenNextSlug(string slug, List<string> listExistedSlug)
        {
            // Truong hop trong db da co slug trung tong 1 group roi
            if (listExistedSlug.Count > 0)
            {
                // Tang index cua slug len
                var counter = 0;
                string tempSlug;
                while (true)
                {
                    counter++;
                    tempSlug = $"{slug}_{counter}";
                    if (!listExistedSlug.Contains(tempSlug))
                    {
                        break;
                    }
                }

                slug = tempSlug;
            }

            return slug;
        }

        protected static string RemoveUnicode(string text)
        {
            for (int i = 0; i < _arrTiengViet.Length; i++)
            {
                text = text.Replace(_arrTiengViet[i], _arrNoUnicode[i]);
                text = text.Replace(_arrTiengVietUpper[i], _arrNoUnicodeUpper[i]);
            }
            return text;
        }

        protected static string NormalizeSlug(string slug)
        {
            slug = RemoveUnicode($"{slug}");
            slug = slug.ToLower().Trim();
            return _slugHelper.GenerateSlug(slug);
        }
    }

    public enum AtRowStatus
    {
        Normal,
        Deleted,
        InActive
    }

    public class AtConstValidator
    {
        public static DateTime MIN_DATE_TIME = new DateTime(1901, 01, 01);
        public static DateTime MAX_DATE_TIME = new DateTime(2901, 01, 01);

        public static DateTimeOffset MIN_DATE_TIME_OFFSET = new DateTimeOffset(new DateTime(1901, 01, 01));
        public static DateTimeOffset MAX_DATE_TIME_OFFSET = new DateTimeOffset(new DateTime(2901, 01, 01));
    }

    public class AtBaseValidator<T> : AbstractValidator<T>
    {

        public AtBaseValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
        }
    }

    public class AtScaffoldingDesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            var options = ReverseEngineerOptions.DbContextAndEntities;
            services.AddHandlebarsScaffolding(options);
        }
    }

    public class SortIndexViewModel
    {
        public string Id { get; set; }
        public int SortIndex { get; set; }
        public string Text { get; set; }
    }
}