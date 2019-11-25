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
using ATAdmin.Efs.Context;
using Microsoft.Extensions.Configuration;

namespace ATAdmin.Controllers
{
    public class ImageSlideController : AtBaseController
    {
        private readonly WebTTCNTTContext _context;

        public ImageSlideController(WebTTCNTTContext context)
        {
            _context = context;
        }

        // GET: ImageSlide
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            ImageSlide dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.ImageSlide.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(ImageSlideController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.ImageSlide.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Select(h => new ImageSlideDetailsViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    SlugName = h.Slug_Name,
                    Extension = h.Extension,
                    Description = h.Description,
                    SortIndex = h.SortIndex,
                    IsYoutube = h.IsYoutube,
                    YoutubeLink = h.YoutubeLink,
                    Thumbnail = h.Thumbnail,
                    Note = h.Note,
                    Tags = h.Tags,
                    KeyWord = h.KeyWord,
                    MetaData = h.MetaData,
                    CreatedBy = h.CreatedBy,
                    CreatedDate = h.CreatedDate,
                    UpdatedBy = h.UpdatedBy,
                    UpdatedDate = h.UpdatedDate,
                    RowVersion = h.RowVersion,
                    RowStatus = (AtRowStatus)h.RowStatus,

                });

            return Json(await query.ToDataSourceResultAsync(request));
        }


        // GET: ImageSlide/Details/5
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageSlide = await _context.ImageSlide.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (imageSlide == null)
            {
                return NotFound();
            }

            return View(imageSlide);
        }

        // GET: ImageSlide/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserImageSlideController).Replace("Controller", "");

            return View();
        }

        // POST: ImageSlide/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ImageSlideCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserImageSlideController).Replace("Controller", "");

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ImageSlide);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space
            vmItem.SlugName = $"{vmItem.SlugName}".Trim();
            if (vmItem.AutoSlug)
            {
                vmItem.SlugName = NormalizeSlug($"{vmItem.Name}");
            }
            else
            {
                vmItem.SlugName = NormalizeSlug($"{vmItem.SlugName}");
            }

            // Check slug is existed => if existed auto get next slug
            var listExistedSlug = await _context.ImageSlide.AsNoTracking()
                    .Where(h => h.Id.StartsWith(vmItem.SlugName))
                    .Select(h => h.Slug_Name).ToListAsync();
            var slug = CheckAndGenNextSlug(vmItem.SlugName, listExistedSlug);


            // Create save db item
            var dbItem = new ImageSlide
            {
                Id = Guid.NewGuid().ToString(),

                CreatedBy = _loginUserId,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = null,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,

                Name = vmItem.Name,
                Slug_Name = vmItem.SlugName,
                AutoSlug = vmItem.AutoSlug,
                Extension = vmItem.Extension,
                Description = vmItem.Description,
                SortIndex = vmItem.SortIndex,
                IsYoutube = vmItem.IsYoutube,
                YoutubeLink = vmItem.YoutubeLink,
                Thumbnail = vmItem.Thumbnail,
                Note = vmItem.Note,
                Tags = vmItem.Tags,
                KeyWord = vmItem.KeyWord,
                MetaData = vmItem.MetaData,
            };
            _context.Add(dbItem);

            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: ImageSlide/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserImageSlideController).Replace("Controller", "");

            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.ImageSlide.AsNoTracking()
                .Where(h => h.Id == id)
                .Select(h => new ImageSlideEditViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    SlugName = h.Slug_Name,
                    AutoSlug = h.AutoSlug,
                    Extension = h.Extension,
                    Description = h.Description,
                    SortIndex = h.SortIndex,
                    IsYoutube = h.IsYoutube,
                    YoutubeLink = h.YoutubeLink,
                    Thumbnail = h.Thumbnail,
                    Note = h.Note,
                    Tags = h.Tags,
                    KeyWord = h.KeyWord,
                    MetaData = h.MetaData,
                    RowVersion = h.RowVersion,
                })
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }


            return View(dbItem);
        }

        // POST: ImageSlide/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ImageSlideEditViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserImageSlideController).Replace("Controller", "");

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ImageSlide);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.ImageSlide
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

            dbItem.Name = vmItem.Name;
            dbItem.Slug_Name = vmItem.SlugName;
            dbItem.AutoSlug = vmItem.AutoSlug;
            dbItem.Extension = vmItem.Extension;
            dbItem.Description = vmItem.Description;
            dbItem.SortIndex = vmItem.SortIndex;
            dbItem.IsYoutube = vmItem.IsYoutube;
            dbItem.YoutubeLink = vmItem.YoutubeLink;
            dbItem.Thumbnail = vmItem.Thumbnail;
            dbItem.Note = vmItem.Note;
            dbItem.Tags = vmItem.Tags;
            dbItem.KeyWord = vmItem.KeyWord;
            dbItem.MetaData = vmItem.MetaData;

            _context.Entry(dbItem).Property(nameof(ImageSlide.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: ImageSlide/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.ImageSlide.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            return View(dbItem);
        }

        // POST: ImageSlide/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] string id, [FromForm] byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ImageSlide);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.ImageSlide

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

                _context.Entry(dbItem).Property(nameof(ImageSlide.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

    }

    public class ImageBrowserImageSlideController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesImageSlide";
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

        public ImageBrowserImageSlideController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
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

    public class ImageSlideBaseViewModel
    {

        public String Name { get; set; }
        public String SlugName { get; set; }
        public Boolean AutoSlug { get; set; }
        public String Extension { get; set; }
        public String Description { get; set; }
        public Int32 SortIndex { get; set; }
        public Boolean IsYoutube { get; set; }
        public String YoutubeLink { get; set; }
        public String Thumbnail { get; set; }
        public String Note { get; set; }
        public String Tags { get; set; }
        public String KeyWord { get; set; }
        public String MetaData { get; set; }
    }

    public class ImageSlideDetailsViewModel : ImageSlideBaseViewModel
    {

        public String Id { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


    }

    public class ImageSlideCreateViewModel : ImageSlideBaseViewModel
    {

    }

    public class ImageSlideEditViewModel : ImageSlideBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class ImageSlideBaseValidator<T> : AtBaseValidator<T> where T : ImageSlideBaseViewModel
    {
        public ImageSlideBaseValidator()
        {
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

            RuleFor(h => h.Extension)
                        .NotEmpty()
                        .MaximumLength(5)
                ;

            RuleFor(h => h.Description)
                        .MaximumLength(1000)
                ;

            RuleFor(h => h.SortIndex)
                        .NotEmpty()
                ;

            RuleFor(h => h.IsYoutube)
                ;

            RuleFor(h => h.YoutubeLink)
                        .MaximumLength(500)
                ;

            RuleFor(h => h.Thumbnail)
                        .NotEmpty()
                        .MaximumLength(500)
                ;

            RuleFor(h => h.Note)
                        .MaximumLength(1000)
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

        }
    }

    public class ImageSlideCreateValidator : ImageSlideBaseValidator<ImageSlideCreateViewModel>
    {
        public ImageSlideCreateValidator()
        {
        }
    }

    public class ImageSlideEditValidator : ImageSlideBaseValidator<ImageSlideEditViewModel>
    {
        public ImageSlideEditValidator()
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
