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
    public class NewsController : AtBaseController
    {
        private readonly WebAtSolutionContext _context;

        public NewsController(WebAtSolutionContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            News dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.News.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(NewsController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.News.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new NewsDetailsViewModel
                {
                    Id = h.Id,
                    FkNewsTypeId = h.FkNewsTypeId,
                    // Ford
                    Title = h.Title,
                    SlugTitle = h.SlugTitle,
                    ShortDescriptionHtml = h.ShortDescriptionHtml,
                    LongDescriptionHtml = h.LongDescriptionHtml,
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

            var news = await _context.News.AsNoTracking()

                .Include(n => n.FkNewsType)
                    .Where(h => h.SlugTitle == id)
                .FirstOrDefaultAsync();
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public async Task<IActionResult> Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserNewController).Replace("Controller", "");
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
        public async Task<IActionResult> Create([FromForm] NewsCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserNewController).Replace("Controller", "");

            // Invalid model
            if (!ModelState.IsValid)
            {
                // Get list master of foreign property and set to view data
                await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(News);
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
            var listExistedSlug = await _context.News.AsNoTracking()
                    .Where(h => h.SlugTitle.StartsWith(vmItem.SlugTitle))
                    .Select(h => h.SlugTitle).ToListAsync();
            var slug = CheckAndGenNextSlug(vmItem.SlugTitle, listExistedSlug);

            // Create save db item
            var dbItem = new News
            {
                Id = Guid.NewGuid().ToString(),

                CreatedBy = _loginUserId,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = null,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,

                FkNewsTypeId = vmItem.FkNewsTypeId,
                Title = vmItem.Title,
                SlugTitle = vmItem.SlugTitle,
                AutoSlug = vmItem.AutoSlug,
                ShortDescriptionHtml = vmItem.ShortDescriptionHtml,
                LongDescriptionHtml = vmItem.LongDescriptionHtml,
                Tags = vmItem.Tags,
                KeyWord = vmItem.KeyWord,
                MetaData = vmItem.MetaData,
                Note = vmItem.Note,
                ImageSlug = vmItem.ImageSlug,
            };
            _context.Add(dbItem);

            // Set time stamp for table to handle concurrency conflict
            if (tableVersion != null)
            {
                tableVersion.LastModify = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = dbItem.SlugTitle });
        }
        
        // GET: News/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.News.AsNoTracking()
                .Where(h => h.SlugTitle == id)
                .Select(h => new NewsEditViewModel
                {
                    Id = h.Id,
                    FkNewsTypeId = h.FkNewsTypeId,
                    Title = h.Title,
                    SlugTitle = h.SlugTitle,
                    AutoSlug = h.AutoSlug,
                    ShortDescriptionHtml = h.ShortDescriptionHtml,
                    LongDescriptionHtml = h.LongDescriptionHtml,
                    Tags = h.Tags,
                    KeyWord = h.KeyWord,
                    MetaData = h.MetaData,
                    Note = h.Note,
                    RowVersion = h.RowVersion,
                    ImageSlug = h.ImageSlug,
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
        public async Task<IActionResult> Edit([FromForm] NewsEditViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                // Get list master of foreign property and set to view data
                await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(News);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.News
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space



            // Update db item               
            dbItem.UpdatedBy = _loginUserId;
            dbItem.UpdatedDate = DateTime.Now;
            dbItem.RowVersion = vmItem.RowVersion;

            dbItem.FkNewsTypeId = vmItem.FkNewsTypeId;
            dbItem.Title = vmItem.Title;
            dbItem.SlugTitle = vmItem.SlugTitle;
            dbItem.AutoSlug = vmItem.AutoSlug;
            dbItem.ShortDescriptionHtml = vmItem.ShortDescriptionHtml;
            dbItem.LongDescriptionHtml = vmItem.LongDescriptionHtml;
            dbItem.Tags = vmItem.Tags;
            dbItem.KeyWord = vmItem.KeyWord;
            dbItem.MetaData = vmItem.MetaData;
            dbItem.Note = vmItem.Note;
            dbItem.ImageSlug = vmItem.ImageSlug;

            _context.Entry(dbItem).Property(nameof(News.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.SlugTitle });
        }

        // GET: News/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.News.AsNoTracking()

                .Include(n => n.FkNewsType)
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
            var tableName = nameof(News);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.News

                .Include(n => n.FkNewsType)
                .Where(h => h.SlugTitle == id)
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

                _context.Entry(dbItem).Property(nameof(News.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareListMasterForeignKey(NewsBaseViewModel vm = null)
        {
            ViewData["FkNewsTypeId"] = new SelectList(
                await _context.NewsType.AsNoTracking()
                    .Select(h => new { h.Id, h.Name, h.RowStatus})
                    .Where(h=>h.RowStatus == (int)AtRowStatus.Normal)
                    .OrderBy(h => h.Id)
                    .ToListAsync(),
                "Id", "Name", vm?.FkNewsTypeId);

                  }
    }

    public class ImageBrowserNewController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesNews";
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

        public ImageBrowserNewController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
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


    public class NewsBaseViewModel
    {

        public String FkNewsTypeId { get; set; }
        public String Title { get; set; }
        public String SlugTitle { get; set; }
        public Boolean AutoSlug { get; set; }
        public String ShortDescriptionHtml { get; set; }
        public String LongDescriptionHtml { get; set; }
        public String Tags { get; set; }
        public String KeyWord { get; set; }
        public String MetaData { get; set; }
        public String Note { get; set; }
        public String ImageSlug { get; set; }
    }

    public class NewsDetailsViewModel : NewsBaseViewModel
    {

        public String Id { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


        public string FkNewsType_Code { get; set; }
        public string FkNewsType_Name { get; set; }

    }

    public class NewsCreateViewModel : NewsBaseViewModel
    {

    }

    public class NewsEditViewModel : NewsBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class NewsBaseValidator<T> : AtBaseValidator<T> where T : NewsBaseViewModel
    {
        public NewsBaseValidator()
        {
            RuleFor(h => h.FkNewsTypeId)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Title)
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

            RuleFor(h => h.ImageSlug)
                        .MaximumLength(100)
                ;

        }
    }

    public class NewsCreateValidator : NewsBaseValidator<NewsCreateViewModel>
    {
        public NewsCreateValidator()
        {
        }
    }

    public class NewsEditValidator : NewsBaseValidator<NewsEditViewModel>
    {
        public NewsEditValidator()
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
