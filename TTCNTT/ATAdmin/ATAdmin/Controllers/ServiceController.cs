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
    public class ServiceController : AtBaseController
    {
        private readonly WebAtSolutionContext _context;

        public ServiceController(WebAtSolutionContext context)
        {
            _context = context;
        }

        // GET: AboutUs
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            Service dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.Service.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(ServiceController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.Service.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new ServiceDetailsViewModel
                {
                    Id = h.Id,
                    ServiceName = h.ServiceName,
                    SlugName = h.SlugName,
                    ShortDescriptionHtml = h.ShortDescriptionHtml,
                    LongDescriptionHtml = h.LongDescriptionHtml,
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

            var service = await _context.Service.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: AboutUs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserServiceController).Replace("Controller", "");
            return View();
        }

        // POST: AboutUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ServiceCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserServiceController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Service);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space



            // Create save db item
            var dbItem = new Service
            {
                Id = Guid.NewGuid().ToString(),

                CreatedBy = _loginUserId,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = null,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,
                ServiceName = vmItem.ServiceName,
                SlugName = vmItem.SlugName,
                AutoSlug = vmItem.AutoSlug,
                ShortDescriptionHtml = vmItem.ShortDescriptionHtml,
                LongDescriptionHtml = vmItem.LongDescriptionHtml,
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

            return RedirectToAction(nameof(Details), new
            {
                id = dbItem.Id
            });
        }

        // GET: AboutUs/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserServiceController).Replace("Controller", "");
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.Service.AsNoTracking()

        .Where(h => h.Id == id)

                .Select(h => new ServiceEditViewModel
                {
                    Id = h.Id,
                    
                    AutoSlug = h.AutoSlug,
                    ShortDescriptionHtml = h.ShortDescriptionHtml,
                    LongDescriptionHtml = h.LongDescriptionHtml,
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
        public async Task<IActionResult> Edit([FromForm] ServiceEditViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserServiceController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Service);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Service
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space



            // Update db item       
            dbItem.ServiceName = vmItem.ServiceName;
            dbItem.SlugName = vmItem.SlugName;
            dbItem.UpdatedBy = _loginUserId;
            dbItem.UpdatedDate = DateTime.Now;
            dbItem.RowVersion = vmItem.RowVersion;
            dbItem.AutoSlug = vmItem.AutoSlug;
            dbItem.ShortDescriptionHtml = vmItem.ShortDescriptionHtml;
            dbItem.LongDescriptionHtml = vmItem.LongDescriptionHtml;
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

            var dbItem = await _context.Service.AsNoTracking()

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
            var tableName = nameof(Service);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Service

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

                _context.Entry(dbItem).Property(nameof(Service.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

    }

    public class ImageBrowserServiceController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesService";
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

        public ImageBrowserServiceController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
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

    public class ServiceBaseViewModel
    {

        public Boolean AutoSlug { get; set; }
        public String ShortDescriptionHtml { get; set; }
        public String LongDescriptionHtml { get; set; }
        public String ImageSlug { get; set; }
        public String Tags { get; set; }
        public String KeyWord { get; set; }
        public String MetaData { get; set; }
        public String Note { get; set; }
    }

    public class ServiceDetailsViewModel : ServiceBaseViewModel
    {

        public String Id { get; set; }
        public String SlugName { get; set; }
        public String ServiceName { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


    }

    public class ServiceCreateViewModel : ServiceBaseViewModel
    {
        public String ServiceName { get; set; }
        public String SlugName { get; set; }
    }

    public class ServiceEditViewModel : ServiceBaseViewModel
    {

        public String Id { get; set; }
        public String ServiceName { get; set; }
        public String SlugName { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class ServiceBaseValidator<T> : AtBaseValidator<T> where T : ServiceBaseViewModel
    {
        public ServiceBaseValidator()
        {

            RuleFor(h => h.AutoSlug)
                ;

            RuleFor(h => h.ShortDescriptionHtml)
                        .MaximumLength(1000)
                ;

            RuleFor(h => h.LongDescriptionHtml)
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

    public class ServiceCreateValidator : ServiceBaseValidator<ServiceCreateViewModel>
    {
        public ServiceCreateValidator()
        {
        }
    }

    public class ServiceEditValidator : ServiceBaseValidator<ServiceEditViewModel>
    {
        public ServiceEditValidator()
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
