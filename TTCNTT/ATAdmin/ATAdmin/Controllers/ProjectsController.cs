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
    //FkProjectType
    //FkProjectTypeId
    public class ProjectsController : AtBaseController
    {
        private readonly WebAtSolutionContext _context;

        public ProjectsController(WebAtSolutionContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            Project dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.Project.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(ProjectsController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.Project.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new ProjectDetailsViewModel
                {
                    Id = h.Id,
                    FkProjectTypeId = h.FkProjectTypeId,
                    // Ford
                    Name = h.Name,
                    SlugName = h.SlugName,
                    ImageSlug = h.ImageSlug,
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

                });

            return Json(await query.ToDataSourceResultAsync(request));
        }


        // GET: Projects/Details/5
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Project.AsNoTracking()

                .Include(p => p.FkProjectType)
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserProjectController).Replace("Controller", "");

            // Get list master of foreign property and set to view data
            await PrepareListMasterForeignKey();

            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProjectCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserProjectController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                // Get list master of foreign property and set to view data
                await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Project);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space
            vmItem.Name = $"{vmItem.Name}".Trim();



            // Create save db item
            var dbItem = new Project
            {
                Id = Guid.NewGuid().ToString(),

                CreatedBy = _loginUserId,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = null,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,

                FkProjectTypeId = vmItem.FkProjectTypeId,
                Name = vmItem.Name,
                SlugName = vmItem.SlugName,
                AutoSlug = vmItem.AutoSlug,
                ImageSlug = vmItem.ImageSlug,
                ShortDescriptionHtml = vmItem.ShortDescriptionHtml,
                LongDescriptionHtml = vmItem.LongDescriptionHtml,
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

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserProjectController).Replace("Controller", "");
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.Project.AsNoTracking()

    .Where(h => h.Id == id)

                .Select(h => new ProjectEditViewModel
                {
                    Id = h.Id,
                    FkProjectTypeId = h.FkProjectTypeId,
                    Name = h.Name,
                    SlugName = h.SlugName,
                    AutoSlug = h.AutoSlug,
                    ImageSlug = h.ImageSlug,
                    ShortDescriptionHtml = h.ShortDescriptionHtml,
                    LongDescriptionHtml = h.LongDescriptionHtml,
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

            // Get list master of foreign property and set to view data
            await PrepareListMasterForeignKey(dbItem);

            return View(dbItem);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProjectEditViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserProjectController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                // Get list master of foreign property and set to view data
                await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Project);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Project
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space
            vmItem.Name = $"{vmItem.Name}".Trim();



            // Update db item               
            dbItem.UpdatedBy = _loginUserId;
            dbItem.UpdatedDate = DateTime.Now;
            dbItem.RowVersion = vmItem.RowVersion;

            dbItem.FkProjectTypeId = vmItem.FkProjectTypeId;
            dbItem.Name = vmItem.Name;
            dbItem.SlugName = vmItem.SlugName;
            dbItem.AutoSlug = vmItem.AutoSlug;
            dbItem.ImageSlug = vmItem.ImageSlug;
            dbItem.ShortDescriptionHtml = vmItem.ShortDescriptionHtml;
            dbItem.LongDescriptionHtml = vmItem.LongDescriptionHtml;
            dbItem.Tags = vmItem.Tags;
            dbItem.KeyWord = vmItem.KeyWord;
            dbItem.MetaData = vmItem.MetaData;
            dbItem.Note = vmItem.Note;

            _context.Entry(dbItem).Property(nameof(Project.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.Project.AsNoTracking()

                .Include(p => p.FkProjectType)
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            return View(dbItem);
        }

        // POST: Projects/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] string id, [FromForm] byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Project);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Project

                .Include(p => p.FkProjectType)
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

                _context.Entry(dbItem).Property(nameof(Project.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareListMasterForeignKey(ProjectBaseViewModel vm = null)
        {
            ViewData["FkProjectTypeId"] = new SelectList(
                await _context.ProjectType.AsNoTracking()
                    .Select(h => new { h.Id, h.Name })
                    .OrderBy(h => h.Name)
                    .ToListAsync(),
                "Id", "Name", vm?.FkProjectTypeId);
        }
    }


    public class ImageBrowserProjectController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesProject";
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

        public ImageBrowserProjectController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
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
    public class ProjectBaseViewModel
    {

        public String FkProjectTypeId { get; set; }
        public String Name { get; set; }
        public String SlugName { get; set; }
        public Boolean AutoSlug { get; set; }
        public String ImageSlug { get; set; }
        public String ShortDescriptionHtml { get; set; }
        public String LongDescriptionHtml { get; set; }
        public String Tags { get; set; }
        public String KeyWord { get; set; }
        public String MetaData { get; set; }
        public String Note { get; set; }
    }

    public class ProjectDetailsViewModel : ProjectBaseViewModel
    {

        public String Id { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


        public string FkProjectType_Code { get; set; }
        public string FkProjectType_Name { get; set; }

    }

    public class ProjectCreateViewModel : ProjectBaseViewModel
    {

    }

    public class ProjectEditViewModel : ProjectBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class ProjectBaseValidator<T> : AtBaseValidator<T> where T : ProjectBaseViewModel
    {
        public ProjectBaseValidator()
        {
            RuleFor(h => h.FkProjectTypeId)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(100)
                ;

            RuleFor(h => h.SlugName)
                        .NotEmpty()
                        .MaximumLength(100)
                ;

            RuleFor(h => h.AutoSlug)
                ;

            RuleFor(h => h.ImageSlug)
                        .MaximumLength(100)
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

        }
    }

    public class ProjectCreateValidator : ProjectBaseValidator<ProjectCreateViewModel>
    {
        public ProjectCreateValidator()
        {
        }
    }

    public class ProjectEditValidator : ProjectBaseValidator<ProjectEditViewModel>
    {
        public ProjectEditValidator()
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
