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
using ATAdmin.Efs.Context;

namespace ATAdmin.Controllers
{

    public class ProductImageController : AtBaseController
    {
        private readonly WebTTCNTTContext _context;

        public ProductImageController(WebTTCNTTContext context)
        {
            _context = context;
        }

        // GET: ProductImage
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            ProductImage dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.ProductImage.FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(ProductImageController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.ProductImage.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new ProductImageDetailsViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    Slug_Name = h.Slug_Name,
                    AutoSlug = h.AutoSlug,
                    FkProductId = h.FkProductId,
                    FkProductName = h.FkProduct.Name,
                   ImageSlug = h.ImageSlug,

                    Extension = h.Extension,
                    Description = h.Description,
                    SortIndex = h.SortIndex,
                    IsYoutube = h.IsYoutube,
                    YoutubeLink = h.YoutubeLink,
                    Thumbnail = h.Thumbnail,
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


        // GET: ProductImage/Details/5
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.ProductImage.AsNoTracking()

                .Include(p => p.FkProduct)
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: ProductImage/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserProductImageController).Replace("Controller", "");

            // Get list master of foreign property and set to view data
            await PrepareListMasterForeignKey();

            return View();
        }

        // POST: ProductImage/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductImageCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserProductImageController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                // Get list master of foreign property and set to view data
                await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ProductImage);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space

            vmItem.Slug_Name = $"{vmItem.Slug_Name}".Trim();
            if (vmItem.AutoSlug)
            {
                vmItem.Slug_Name = NormalizeSlug($"{vmItem.Name}");
            }
            else
            {
                vmItem.Slug_Name = NormalizeSlug($"{vmItem.Slug_Name}");
            }

            // Check slug is existed => if existed auto get next slug
            var listExistedSlug = await _context.ProductImage.AsNoTracking()
                    .Where(h => h.Id.StartsWith(vmItem.Slug_Name))
                    .Select(h => h.Slug_Name).ToListAsync();
            var slug = CheckAndGenNextSlug(vmItem.Slug_Name, listExistedSlug);

            // Create save db item
            var dbItem = new ProductImage
            {
                Id = Guid.NewGuid().ToString(),

                CreatedBy = _loginUserId,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = null,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,

                FkProductId = vmItem.FkProductId,
                Name = vmItem.Name,
                Slug_Name = vmItem.Slug_Name,
                AutoSlug = vmItem.AutoSlug,
                ImageSlug = vmItem.ImageSlug,

                Extension = vmItem.Extension,
                Description = vmItem.Description,
                SortIndex = vmItem.SortIndex,
                IsYoutube = vmItem.IsYoutube,
                YoutubeLink = vmItem.YoutubeLink,
                Thumbnail = vmItem.Thumbnail,
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

        // GET: ProductImage/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserProductImageController).Replace("Controller", "");
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.ProductImage.AsNoTracking()

    .Where(h => h.Id == id)

                .Select(h => new ProductImageEditViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    Slug_Name = h.Slug_Name,
                    AutoSlug = h.AutoSlug,
                    ImageSlug = h.ImageSlug,
                    FkProductId = h.FkProductId,
                    FkProductName = h.FkProduct.Name,
                    Extension = h.Extension,
                    Description = h.Description,
                    SortIndex = h.SortIndex,
                    IsYoutube = h.IsYoutube,
                    YoutubeLink = h.YoutubeLink,
                    Thumbnail = h.Thumbnail,
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

        // POST: ProductImage/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProductImageEditViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserProductImageController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                // Get list master of foreign property and set to view data
                await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ProductImage);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.ProductImage
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space
            vmItem.Slug_Name = $"{vmItem.Slug_Name}".Trim();
            if (vmItem.AutoSlug)
            {
                vmItem.Slug_Name = NormalizeSlug($"{vmItem.Name}");
            }
            else
            {
                vmItem.Slug_Name = NormalizeSlug($"{vmItem.Slug_Name}");
            }

            // Check slug is existed => if existed auto get next slug
            var listExistedSlug = await _context.ProductImage.AsNoTracking()
                    .Where(h => h.Id.StartsWith(vmItem.Slug_Name))
                    .Select(h => h.Slug_Name).ToListAsync();
            var slug = CheckAndGenNextSlug(vmItem.Slug_Name, listExistedSlug);


            // Update db item               
            dbItem.UpdatedBy = _loginUserId;
            dbItem.UpdatedDate = DateTime.Now;
            dbItem.RowVersion = vmItem.RowVersion;

            dbItem.FkProductId = vmItem.FkProductId;
            dbItem.Name = vmItem.Name;
            dbItem.Slug_Name = vmItem.Slug_Name;
            dbItem.AutoSlug = vmItem.AutoSlug;
            dbItem.ImageSlug = vmItem.ImageSlug;
            dbItem.Extension = vmItem.Extension;
            dbItem.Description = vmItem.Description;
            dbItem.SortIndex = vmItem.SortIndex;
            dbItem.IsYoutube = vmItem.IsYoutube;
            dbItem.YoutubeLink = vmItem.YoutubeLink;
            dbItem.Thumbnail = vmItem.Thumbnail;
            dbItem.Tags = vmItem.Tags;
            dbItem.KeyWord = vmItem.KeyWord;
            dbItem.MetaData = vmItem.MetaData;
            dbItem.Note = vmItem.Note;


            _context.Entry(dbItem).Property(nameof(ProductImage.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: ProductImage/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.ProductImage.AsNoTracking()

                .Include(p => p.FkProduct)
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            return View(dbItem);
        }

        // POST: ProductImage/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] string id, [FromForm] byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ProductImage);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.ProductImage

                .Include(p => p.FkProduct)
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

                _context.Entry(dbItem).Property(nameof(ProductImage.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareListMasterForeignKey(ProductImageBaseViewModel vm = null)
        {
            ViewData["FkProductId"] = new SelectList(
                await _context.Product.AsNoTracking()
                    .Select(h => new { h.Id, h.Name })
                    .OrderBy(h => h.Name)
                    .ToListAsync(),
                "Id", "Name", vm?.FkProductId);
        }
    }


    public class ImageBrowserProductImageController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesProductImage";
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

        public ImageBrowserProductImageController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
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
    public class ProductImageBaseViewModel
    {
        public string Name { get; set; }
        public string Slug_Name { get; set; }
        public bool AutoSlug { get; set; }
        public string ImageSlug { get; set; }
        public string FkProductId { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public int SortIndex { get; set; }
        public bool IsYoutube { get; set; }
        public string YoutubeLink { get; set; }
        public string Thumbnail { get; set; }
        public string Tags { get; set; }
        public string KeyWord { get; set; }
        public string MetaData { get; set; }
        public string Note { get; set; }

        public String FkProductName { get; set; }

    }

    public class ProductImageDetailsViewModel : ProductImageBaseViewModel
    {

        public String Id { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


        public virtual Product FkProduct { get; set; }


    }

    public class ProductImageCreateViewModel : ProductImageBaseViewModel
    {

    }

    public class ProductImageEditViewModel : ProductImageBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class ProductImageBaseValidator<T> : AtBaseValidator<T> where T : ProductImageBaseViewModel
    {
        public ProductImageBaseValidator()
        {
            RuleFor(h => h.FkProductId)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Slug_Name)
                        .NotEmpty()
                        .MaximumLength(100)
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

            RuleFor(h => h.YoutubeLink)
                        .MaximumLength(500)
                ;

            RuleFor(h => h.Thumbnail)
                        .MaximumLength(500)
                ;

            RuleFor(h => h.Note)
                        .MaximumLength(1000)
                ;

        }
    }

    public class ProductImageCreateValidator : ProductImageBaseValidator<ProductImageCreateViewModel>
    {
        public ProductImageCreateValidator()
        {
        }
    }

    public class ProductImageEditValidator : ProductImageBaseValidator<ProductImageEditViewModel>
    {
        public ProductImageEditValidator()
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
