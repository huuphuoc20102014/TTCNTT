using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ATAdmin.Efs.Entities;
using FluentValidation;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ATAdmin.Controllers
{
    //Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore.NavigationMetadata
    //FkNewsType
    //FkNewsTypeId
    public class SettingController : AtBaseController
    {
        private readonly WebAtSolutionContext _context;

        public SettingController(WebAtSolutionContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            Setting dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.Setting.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(SettingController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.Setting.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new SettingDetailsViewModel
                {
                    Id = h.Id,
                    Style = h.Style,
                    Value = h.Value,
                    Description = h.Description,
                    IsManual = h.IsManual,
                    ImageSlug = h.ImageSlug,

                });

            return Json(await query.ToDataSourceResultAsync(request));
        }


        // GET: News/Details/5
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var setting = await _context.Setting.AsNoTracking()
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (setting == null)
            {
                return NotFound();
            }

            return View(setting);
        }

        // GET: News/Create
        public async Task<IActionResult> Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserSettingController).Replace("Controller", "");
                // Get list master of foreign property and set to view data
                await PrepareListMasterForeignKey();

                return View();
            }
            else
            {
                return RedirectToAction(nameof(ErrorController.Index), nameof(ErrorController).Replace("Controller", ""));
            }
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] SettingCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserSettingController).Replace("Controller", "");

            // Invalid model
            if (!ModelState.IsValid)
            {
                //// Get list master of foreign property and set to view data
                //await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Setting);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            //// Trim white space
            //vmItem.SlugTitle = $"{vmItem.SlugTitle}".Trim();
            //if (vmItem.AutoSlug)
            //{
            //    vmItem.SlugTitle = NormalizeSlug($"{vmItem.Title}");
            //}
            //else
            //{
            //    vmItem.SlugTitle = NormalizeSlug($"{vmItem.SlugTitle}");
            //}

            //// Check slug is existed => if existed auto get next slug
            //var listExistedSlug = await _context.News.AsNoTracking()
            //        .Where(h => h.SlugTitle.StartsWith(vmItem.SlugTitle))
            //        .Select(h => h.SlugTitle).ToListAsync();
            //var slug = CheckAndGenNextSlug(vmItem.SlugTitle, listExistedSlug);

            // Create save db item
            var dbItem = new Setting
            {
                Id = Guid.NewGuid().ToString(),
                Style = vmItem.Style,
                Value = vmItem.Value,
                Description = vmItem.Description,
                IsManual = vmItem.IsManual,
                ImageSlug = vmItem.ImageSlug,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,

            };
            _context.Add(dbItem);

            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new{id = dbItem.Id});
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserSettingController).Replace("Controller", "");
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.Setting.AsNoTracking()
                 .Where(h => h.Id == id)
                .Select(h => new SettingEditViewModel
                {
                    Id = h.Id,
                    Style = h.Style,
                    Value = h.Value,
                    Description = h.Description,
                    IsManual = h.IsManual,
                    ImageSlug = h.ImageSlug,
                    RowVersion = h.RowVersion,
                })
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Get list master of foreign property and set to view data
            await PrepareListMasterForeignKey(dbItem);

            return View(dbItem);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] SettingEditViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserSettingController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                // Get list master of foreign property and set to view data
                //await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Setting);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Setting
                .Where(h => h.Id == vmItem.Id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space

            // Update db item               
            dbItem.Style = vmItem.Style;
            dbItem.Value = vmItem.Value;
            dbItem.Description = vmItem.Description;
            dbItem.IsManual = vmItem.IsManual;
            dbItem.ImageSlug = vmItem.ImageSlug;

            _context.Entry(dbItem).Property(nameof(Setting.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: News/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.Setting.AsNoTracking()

                .Include(n => n.Style)
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            return View(dbItem);
        }

        // POST: News/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] string id, [FromForm] byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Setting);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Setting

                .Include(n => n.Style)
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
                dbItem.RowVersion = rowVersion;

                _context.Entry(dbItem).Property(nameof(Setting.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareListMasterForeignKey(SettingBaseViewModel vm = null)
        {
            ViewData["Style"] = new SelectList(
                await _context.SettingType.AsNoTracking()
                    .Select(h => new { h.Id, h.Name, h.RowStatus })
                    .Where(h => h.RowStatus == (int)AtRowStatus.Normal)
                    .OrderBy(h => h.Id)
                    .ToListAsync(),
                "Id", "Name", vm?.Style);

        }
    }

    public class ImageBrowserSettingController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesSetting";
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

        public ImageBrowserSettingController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
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


    public class SettingBaseViewModel
    {

        public String Value { get; set; }
        public String Description { get; set; }
        public String Style { get; set; }
        public bool? IsManual { get; set; }
        public String ImageSlug { get; set; }
    }

    public class SettingDetailsViewModel : SettingBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }

    }

    public class SettingCreateViewModel : SettingBaseViewModel
    {

    }

    public class SettingEditViewModel : SettingBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class SettingBaseValidator<T> : AtBaseValidator<T> where T : SettingBaseViewModel
    {
        public SettingBaseValidator()
        {
            RuleFor(h => h.Value)
                        .NotEmpty()
                        .MaximumLength(10000)
                ;

            RuleFor(h => h.Description)
                        .MaximumLength(200)
                ;

        }
    }

    public class SettingCreateValidator : SettingBaseValidator<SettingCreateViewModel>
    {
        public SettingCreateValidator()
        {
        }
    }

    public class SettingEditValidator : SettingBaseValidator<SettingEditViewModel>
    {
        public SettingEditValidator()
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
