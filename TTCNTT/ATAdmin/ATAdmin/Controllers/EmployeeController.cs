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
    public class EmployeeController : AtBaseController
    {
        private readonly WebTTCNTTContext _context;

        public EmployeeController(WebTTCNTTContext context)
        {
            _context = context;
        }

        // GET: AboutUs
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            Employee dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.Employee.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(EmployeeController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.Employee.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new EmployeeDetailsViewModel
                {
                    Id = h.Id,
                    Code = h.Code,
                    Name = h.Name,
                    Slug_Name = h.Slug_Name,
                    BirthDate = h.BirthDate,
                    Address = h.Address,
                    Phone = h.Phone,
                    Specialize = h.Specialize,
                    Fk_EmplyeeId = h.Fk_EmplyeeId,
                    ImageSlug = h.ImageSlug,
                    ShortDescription_Html = h.ShortDescription_Html,
                    LongDescription_Html = h.LongDescription_Html,
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

            var employee = await _context.Employee.AsNoTracking()
                .Include(h => h.Fk_Emplyee)
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: AboutUs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserEmployeeController).Replace("Controller", "");

            await PrepareListMasterForeignKey();
            return View();
        }

        // POST: AboutUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] EmployeeCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserEmployeeController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Employee);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space



            // Create save db item
            var dbItem = new Employee
            {
                Id = Guid.NewGuid().ToString(),

                CreatedBy = _loginUserId,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = null,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,

                Code = vmItem.Code,
                Name = vmItem.Name,
                Slug_Name = vmItem.Slug_Name,
                BirthDate = vmItem.BirthDate,
                Address = vmItem.Address,
                Phone = vmItem.Phone,
                Specialize = vmItem.Specialize,
                Fk_EmplyeeId = vmItem.Fk_EmplyeeId,
                ImageSlug = vmItem.ImageSlug,
                ShortDescription_Html = vmItem.ShortDescription_Html,
                LongDescription_Html = vmItem.LongDescription_Html,
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
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserEmployeeController).Replace("Controller", "");
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.Employee.AsNoTracking()

            .Where(h => h.Id == id)

                .Select(h => new EmployeeEditViewModel
                {
                    Id = h.Id,
                    Code = h.Code,
                    Name = h.Name,
                    Slug_Name = h.Slug_Name,
                    BirthDate = h.BirthDate,
                    Address = h.Address,
                    Phone = h.Phone,
                    Specialize = h.Specialize,
                    Fk_EmplyeeId = h.Fk_EmplyeeId,
                    ImageSlug = h.ImageSlug,
                    ShortDescription_Html = h.ShortDescription_Html,
                    LongDescription_Html = h.LongDescription_Html,
                    Note = h.Note,

                    RowVersion = h.RowVersion,
                })
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            await PrepareListMasterForeignKey(dbItem);
            return View(dbItem);
        }

        // POST: AboutUs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] EmployeeEditViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserEmployeeController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                await PrepareListMasterForeignKey(vmItem);
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Employee);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Employee
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Update db item               
            dbItem.UpdatedBy = _loginUserId;
            dbItem.UpdatedDate = DateTime.Now;
            dbItem.RowVersion = vmItem.RowVersion;

            dbItem.Code = vmItem.Code;
            dbItem.Name = vmItem.Name;
            dbItem.Slug_Name = vmItem.Slug_Name;
            dbItem.BirthDate = vmItem.BirthDate;
            dbItem.Address = vmItem.Address;
            dbItem.Phone = vmItem.Phone;
            dbItem.Specialize = vmItem.Specialize;
            dbItem.Fk_EmplyeeId = vmItem.Fk_EmplyeeId;
            dbItem.ImageSlug = vmItem.ImageSlug;
            dbItem.ShortDescription_Html = vmItem.ShortDescription_Html;
            dbItem.LongDescription_Html = vmItem.LongDescription_Html;
            dbItem.Note = vmItem.Note;

            _context.Entry(dbItem).Property(nameof(Employee.RowVersion)).OriginalValue = vmItem.RowVersion;
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

            var dbItem = await _context.Employee.AsNoTracking()
                .Include(h => h.Fk_Emplyee)
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
            var tableName = nameof(Employee);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Employee
                .Include(h => h.Fk_Emplyee)
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

                _context.Entry(dbItem).Property(nameof(Employee.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareListMasterForeignKey(EmployeeBaseViewModel vm = null)
        {
            ViewData["FkEmplyeeId"] = new SelectList(
                await _context.EmployeeType.AsNoTracking()
                    .Select(h => new { h.Id, h.Name })
                    .OrderBy(h => h.Name)
                    .ToListAsync(),
                "Id", "Name", vm?.Fk_EmplyeeId);
        }

    }

    public class ImageBrowserEmployeeController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesEmployee";
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

        public ImageBrowserEmployeeController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
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

    public class EmployeeBaseViewModel
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public string Slug_Name { get; set; }
        public bool AutoSlug { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Specialize { get; set; }
        public string Fk_EmplyeeId { get; set; }
        public string ImageSlug { get; set; }
        public string ShortDescription_Html { get; set; }
        public string LongDescription_Html { get; set; }
        public string Note { get; set; }

    }

    public class EmployeeDetailsViewModel : EmployeeBaseViewModel
    {
        public String Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }

        public virtual EmployeeType Fk_Emplyee { get; set; }
    }

    public class EmployeeCreateViewModel : EmployeeBaseViewModel
    {

    }

    public class EmployeeEditViewModel : EmployeeBaseViewModel
    {
        public String Id { get; set; }
        public byte[] RowVersion { get; set; }

    }

    public class EmployeeBaseValidator<T> : AtBaseValidator<T> where T : EmployeeBaseViewModel
    {
        public EmployeeBaseValidator()
        {
            RuleFor(h => h.Code)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(50)
                ;


            RuleFor(h => h.Slug_Name)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Address)
                        .NotEmpty()
                        .MaximumLength(500)
                ;


            RuleFor(h => h.Specialize)
                        .NotEmpty()
                        .MaximumLength(100)
                ;

            RuleFor(h => h.ImageSlug)
                        .NotEmpty()
                        .MaximumLength(100)
                ;


            RuleFor(h => h.ShortDescription_Html)
                        .NotEmpty()
                        .MaximumLength(1000)
                ;

            RuleFor(h => h.LongDescription_Html)
                        .NotEmpty()
                ;


            RuleFor(h => h.Note)
                        .MaximumLength(1000)
                ;

        }
    }

    public class EmployeeCreateValidator : EmployeeBaseValidator<EmployeeCreateViewModel>
    {
        public EmployeeCreateValidator()
        {

        }
    }

    public class EmployeeEditValidator : EmployeeBaseValidator<EmployeeEditViewModel>
    {
        public EmployeeEditValidator()
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
