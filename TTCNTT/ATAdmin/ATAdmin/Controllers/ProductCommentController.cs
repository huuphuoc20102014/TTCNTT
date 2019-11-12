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
    //Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore.NavigationMetadata
    //FkProductCommentType
    //FkProductCommentTypeId
    public class ProductCommentController : AtBaseController
    {
        private readonly WebTTCNTTContext _context;

        public ProductCommentController(WebTTCNTTContext context)
        {
            _context = context;
        }

        // GET: ProductComment
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            ProductComment dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.ProductComment.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(ProductCommentController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.ProductComment.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new ProductCommentDetailsViewModel
                {
                    Id = h.Id,
                    FkProductId = h.FkProductId,
                    Name = h.Name,
                    Email = h.Email,
                    Phone = h.Phone,
                    Comment = h.Comment,
                    IsRead = h.IsRead,
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


        // GET: ProductComment/Details/5
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.ProductComment.AsNoTracking()

                .Include(n => n.FkProduct)
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: ProductComment/Create
        public async Task<IActionResult> Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserProductCommentController).Replace("Controller", "");
                // Get list master of foreign property and set to view data
                await PrepareListMasterForeignKey();

                return View();
            }
            else
            {
                return RedirectToAction(nameof(ErrorController.Index), nameof(ErrorController).Replace("Controller", ""));
            }
        }

        // POST: ProductComment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ProductCommentCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserProductCommentController).Replace("Controller", "");

            // Invalid model
            if (!ModelState.IsValid)
            {
                // Get list master of foreign property and set to view data
                await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ProductComment);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Create save db item
            var dbItem = new ProductComment
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
                Email = vmItem.Email,
                Phone = vmItem.Phone,
                Comment = vmItem.Comment,
                IsRead = vmItem.IsRead,
                Note = vmItem.Note,
            };
            _context.Add(dbItem);

            // Set time stamp for table to handle concurrency conflict
            if (tableVersion != null)
            {
                tableVersion.LastModify = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: ProductComment/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.ProductComment.AsNoTracking()
                .Where(h => h.Id == id)
                .Select(h => new ProductCommentEditViewModel
                {
                    Id = h.Id,
                    FkProductId = h.FkProductId,
                    Name = h.Name,
                    Email = h.Email,
                    Phone = h.Phone,
                    Comment = h.Comment,
                    IsRead = h.IsRead,
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

        // POST: ProductComment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProductCommentEditViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                // Get list master of foreign property and set to view data
                await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ProductComment);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.ProductComment
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

            dbItem.FkProductId = vmItem.FkProductId;
            dbItem.Name = vmItem.Name;
            dbItem.Email = vmItem.Email;
            dbItem.Phone = vmItem.Phone;
            dbItem.Comment = vmItem.Comment;
            dbItem.IsRead = vmItem.IsRead;
            dbItem.Note = vmItem.Note;

            _context.Entry(dbItem).Property(nameof(ProductComment.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: ProductComment/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.ProductComment.AsNoTracking()

                .Include(n => n.FkProduct)
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            return View(dbItem);
        }

        // POST: ProductComment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] string id, [FromForm] byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ProductComment);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.ProductComment

                .Include(n => n.FkProduct)
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

                _context.Entry(dbItem).Property(nameof(ProductComment.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareListMasterForeignKey(ProductCommentBaseViewModel vm = null)
        {
            ViewData["FkProductId"] = new SelectList(
                await _context.Product.AsNoTracking()
                    .Select(h => new { h.Id, h.Name, h.RowStatus })
                    .Where(h => h.RowStatus == (int)AtRowStatus.Normal)
                    .OrderBy(h => h.Id)
                    .ToListAsync(),
                "Id", "Name", vm?.FkProductId);

        }
    }

    public class ImageBrowserProductCommentController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesProductComment";
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

        public ImageBrowserProductCommentController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
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


    public class ProductCommentBaseViewModel
    {

        public string FkProductId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public int? Rating { get; set; }
        public bool? IsRead { get; set; }
        public string FkProductCommentId { get; set; }
        public string Note { get; set; }
    }

    public class ProductCommentDetailsViewModel : ProductCommentBaseViewModel
    {

        public String Id { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


    }

    public class ProductCommentCreateViewModel : ProductCommentBaseViewModel
    {

    }

    public class ProductCommentEditViewModel : ProductCommentBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class ProductCommentBaseValidator<T> : AtBaseValidator<T> where T : ProductCommentBaseViewModel
    {
        public ProductCommentBaseValidator()
        {
            RuleFor(h => h.FkProductId)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Email)
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Phone)
                        .MaximumLength(20)
                ;

            RuleFor(h => h.Comment)
                        .NotEmpty()
                        .MaximumLength(1000)
                ;
            RuleFor(h => h.Note)
                        .MaximumLength(1000)
                ;

        }
    }

    public class ProductCommentCreateValidator : ProductCommentBaseValidator<ProductCommentCreateViewModel>
    {
        public ProductCommentCreateValidator()
        {
        }
    }

    public class ProductCommentEditValidator : ProductCommentBaseValidator<ProductCommentEditViewModel>
    {
        public ProductCommentEditValidator()
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
