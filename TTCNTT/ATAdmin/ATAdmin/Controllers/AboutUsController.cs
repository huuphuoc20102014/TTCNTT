﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ATAdmin.Efs.Entities;
using FluentValidation;
using ATAdmin.Efs.Context;

namespace ATAdmin.Controllers
{
    public class AboutUsController : AtBaseController
    {
        private readonly WebTTCNTTContext _context;

        public AboutUsController(WebTTCNTTContext context)
        {
            _context = context;
        }

        // GET: AboutUs
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            AboutUs dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.AboutUs.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(AboutUsController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.AboutUs.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new AboutUsDetailsViewModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    SlugTitle = h.Slug_Title,
                    ShortDescriptionHtml = h.ShortDescription_Html,
                    LongDescriptionHtml = h.LongDescription_Html,
                    ImageSlug = h.ImageSlug,
                    Tags = h.Tags,
                    KeyWord = h.KeyWord,
                    MetaData = h.MetaData,
                    Note = h.Note,
                    CreatedBy = h.CreatedBy,
                    CreatedDate = h.CreatedDate,
                    UpdatedBy = h.UpdatedBy,
                    UpdatedDate = h.UpdatedDate,
                    RowVersion = h.RowVersion,
                    RowStatus = (AtRowStatus)h.RowStatus,

                });

            return Json(await query.ToDataSourceResultAsync(request));
        }


        // GET: AboutUs/Details/5
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aboutUs = await _context.AboutUs.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (aboutUs == null)
            {
                return NotFound();
            }

            return View(aboutUs);
        }

        // GET: AboutUs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserAboutUsController).Replace("Controller", "");

            return View();
        }

        // POST: AboutUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] AboutUsCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserAboutUsController).Replace("Controller", "");

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(AboutUs);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space
            vmItem.SlugTitle = $"{vmItem.SlugTitle}".Trim();
            if (vmItem.AutoSlug)
            {
                vmItem.SlugTitle = NormalizeSlug($"{vmItem.Title}");
            }
            else
            {
                vmItem.SlugTitle = NormalizeSlug($"{vmItem.SlugTitle}");
            }

            // Check slug is existed => if existed auto get next slug
            var listExistedSlug = await _context.AboutUs.AsNoTracking()
                    .Where(h => h.Id.StartsWith(vmItem.SlugTitle))
                    .Select(h => h.Slug_Title).ToListAsync();
            var slug = CheckAndGenNextSlug(vmItem.SlugTitle, listExistedSlug);

            // Create save db item
            var dbItem = new AboutUs
            {
                Id = Guid.NewGuid().ToString(),

                CreatedBy = _loginUserId,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = null,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,

                Title = vmItem.Title,
                Slug_Title = vmItem.SlugTitle,
                AutoSlug = vmItem.AutoSlug,
                ShortDescription_Html = vmItem.ShortDescriptionHtml,
                LongDescription_Html = vmItem.LongDescriptionHtml,
                ImageSlug = vmItem.ImageSlug,
                Tags = vmItem.Tags,
                KeyWord = vmItem.KeyWord,
                MetaData = vmItem.MetaData,
                Note = vmItem.Note,
            };
            _context.Add(dbItem);

            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: AboutUs/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserAboutUsController).Replace("Controller", "");

            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.AboutUs.AsNoTracking()

    .Where(h => h.Id == id)

                .Select(h => new AboutUsEditViewModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    SlugTitle = h.Slug_Title,
                    AutoSlug = h.AutoSlug,
                    ShortDescriptionHtml = h.ShortDescription_Html,
                    LongDescriptionHtml = h.LongDescription_Html,
                    ImageSlug = h.ImageSlug,
                    Tags = h.Tags,
                    KeyWord = h.KeyWord,
                    MetaData = h.MetaData,
                    Note = h.Note,
                    RowVersion = h.RowVersion,
                })
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }


            return View(dbItem);
        }

        // POST: AboutUs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] AboutUsEditViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserAboutUsController).Replace("Controller", "");

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(AboutUs);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.AboutUs
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space
            vmItem.SlugTitle = $"{vmItem.SlugTitle}".Trim();
            if (vmItem.AutoSlug)
            {
                vmItem.SlugTitle = NormalizeSlug($"{vmItem.Title}");
            }
            else
            {
                vmItem.SlugTitle = NormalizeSlug($"{vmItem.SlugTitle}");
            }

            // Check slug is existed => if existed auto get next slug
            var listExistedSlug = await _context.AboutUs.AsNoTracking()
                    .Where(h => h.Id.StartsWith(vmItem.SlugTitle))
                    .Select(h => h.Slug_Title).ToListAsync();
            var slug = CheckAndGenNextSlug(vmItem.SlugTitle, listExistedSlug);

            // Update db item               
            dbItem.UpdatedBy = _loginUserId;
            dbItem.UpdatedDate = DateTime.Now;
            dbItem.RowVersion = vmItem.RowVersion;

            dbItem.Title = vmItem.Title;
            dbItem.Slug_Title = vmItem.SlugTitle;
            dbItem.AutoSlug = vmItem.AutoSlug;
            dbItem.ShortDescription_Html = vmItem.ShortDescriptionHtml;
            dbItem.LongDescription_Html = vmItem.LongDescriptionHtml;
            dbItem.ImageSlug = vmItem.ImageSlug;
            dbItem.Tags = vmItem.Tags;
            dbItem.KeyWord = vmItem.KeyWord;
            dbItem.MetaData = vmItem.MetaData;
            dbItem.Note = vmItem.Note;

            _context.Entry(dbItem).Property(nameof(AboutUs.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: AboutUs/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.AboutUs.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            return View(dbItem);
        }

        // POST: AboutUs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] string id, [FromForm] byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(AboutUs);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.AboutUs

                .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            if (rowVersion == null)
            {
                ModelState.AddModelError("RowVersion", "Invalid row version, please try again.");
                return View(dbItem);
            }

            // Update db item               
            if (dbItem.RowStatus != (int)AtRowStatus.Deleted)
            {
                dbItem.RowStatus = (int)AtRowStatus.Deleted;
                dbItem.UpdatedBy = _loginUserId;
                dbItem.UpdatedDate = DateTime.Now;
                dbItem.RowVersion = rowVersion;

                _context.Entry(dbItem).Property(nameof(AboutUs.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

    }

    public class ImageBrowserAboutUsController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesAboutUs";
        public string FOLDER_ROOTPATH;

        /// <summary>
        /// Gets the base paths from which content will be served.
        /// </summary>
        public override string ContentPath
        {
            get
            {
                return CreateUserFolder();
            }
        }

        public ImageBrowserAboutUsController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
           : base(hostingEnvironment)
        {
            FOLDER_ROOTPATH = staticFileSetting.GetValue<string>("StaticFileSetting");
        }
        private string CreateUserFolder()
        {
            var virtualPath = System.IO.Path.Combine(FOLDER_NAME);
            //var path = HostingEnvironment.WebRootFileProvider.GetFileInfo(virtualPath).PhysicalPath;
            var path = System.IO.Path.Combine(FOLDER_ROOTPATH, FOLDER_NAME);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return path;
        }
    }


    public class AboutUsBaseViewModel
    {

        public String Title { get; set; }
        public String SlugTitle { get; set; }
        public Boolean AutoSlug { get; set; }
        public String ShortDescriptionHtml { get; set; }
        public String LongDescriptionHtml { get; set; }
        public String ImageSlug { get; set; }
        public String Tags { get; set; }
        public String KeyWord { get; set; }
        public String MetaData { get; set; }
        public String Note { get; set; }
    }

    public class AboutUsDetailsViewModel : AboutUsBaseViewModel
    {

        public String Id { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


    }

    public class AboutUsCreateViewModel : AboutUsBaseViewModel
    {

    }

    public class AboutUsEditViewModel : AboutUsBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class AboutUsBaseValidator<T> : AtBaseValidator<T> where T : AboutUsBaseViewModel
    {
        public AboutUsBaseValidator()
        {
            RuleFor(h => h.Title)
                        .NotEmpty()
                        .MaximumLength(500)
                ;

            RuleFor(h => h.SlugTitle)
                        .NotEmpty()
                        .MaximumLength(500)
                ;

            RuleFor(h => h.AutoSlug)
                ;

            RuleFor(h => h.ShortDescriptionHtml)
                        .MaximumLength(1000)
                ;

            RuleFor(h => h.LongDescriptionHtml)
                        .MaximumLength(1000)
                ;

            RuleFor(h => h.ImageSlug)
                        .MaximumLength(100)
                ;

            RuleFor(h => h.Tags)
                        .MaximumLength(1000)
                ;

            RuleFor(h => h.KeyWord)
                        .MaximumLength(1000)
                ;

            RuleFor(h => h.MetaData)
                        .MaximumLength(1000)
                ;

            RuleFor(h => h.Note)
                        .MaximumLength(1000)
                ;

        }
    }

    public class AboutUsCreateValidator : AboutUsBaseValidator<AboutUsCreateViewModel>
    {
        public AboutUsCreateValidator()
        {
        }
    }

    public class AboutUsEditValidator : AboutUsBaseValidator<AboutUsEditViewModel>
    {
        public AboutUsEditValidator()
        {
            RuleFor(h => h.Id)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.RowVersion)
                        .NotNull()
                ;

        }
    }








}
