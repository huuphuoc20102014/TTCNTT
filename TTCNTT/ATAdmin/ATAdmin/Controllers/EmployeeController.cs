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
    //FkEmployeeType
    //FkEmployeeTypeId
    public class EmployeeController : AtBaseController
    {
        private readonly WebTTCNTTContext _context;

        public EmployeeController(WebTTCNTTContext context)
        {
            _context = context;
        }

        // GET: Employee
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
                    EmployeeTypeName = h.Fk_Emplyee.Name,
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


        // GET: Employee/Details/5
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.AsNoTracking()

                .Include(n => n.Fk_Emplyee)
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
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

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] EmployeeCreateViewModel vmItem)
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
            var tableName = nameof(Employee);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space
            //vmItem.Slug_Name = $"{vmItem.Slug_Name}".Trim();
            //if (vmItem.AutoSlug)
            //{
            //    vmItem.Slug_Name = NormalizeSlug($"{vmItem.Name}");
            //}
            //else
            //{
            //    vmItem.Slug_Name = NormalizeSlug($"{vmItem.Slug_Name}");
            //}

            // Check slug is existed => if existed auto get next slug
            //var listExistedSlug = await _context.Employee.AsNoTracking()
            //        .Where(h => h.Id.StartsWith(vmItem.Slug_Name))
            //        .Select(h => h.Slug_Name).ToListAsync();
            //var slug = CheckAndGenNextSlug(vmItem.Slug_Name, listExistedSlug);

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

                Fk_EmplyeeId = vmItem.Fk_EmplyeeId,
                Code = vmItem.Code,
                Name = vmItem.Name,
                Slug_Name = vmItem.Slug_Name,
                BirthDate = vmItem.BirthDate,
                Address = vmItem.Address,
                Phone = vmItem.Phone,
                Specialize = vmItem.Specialize,
                ImageSlug = vmItem.ImageSlug,
                //AutoSlug = vmItem.AutoSlug,
                ShortDescription_Html = vmItem.ShortDescription_Html,
                LongDescription_Html = vmItem.LongDescription_Html,
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

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserNewController).Replace("Controller", "");

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

            // Get list master of foreign property and set to view data
            await PrepareListMasterForeignKey(dbItem);

            return View(dbItem);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] EmployeeEditViewModel vmItem)
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
            var tableName = nameof(Employee);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Employee
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

            dbItem.Fk_EmplyeeId = vmItem.Fk_EmplyeeId;
            dbItem.Code = vmItem.Code;
            dbItem.Name = vmItem.Name;
            dbItem.Slug_Name = vmItem.Slug_Name;
            dbItem.BirthDate = vmItem.BirthDate;
            dbItem.Address = vmItem.Address;
            dbItem.Phone = vmItem.Phone;
            dbItem.Specialize = vmItem.Specialize;

            dbItem.ImageSlug = vmItem.ImageSlug;
            //dbItem.AutoSlug = vmItem.AutoSlug;
            dbItem.ShortDescription_Html = vmItem.ShortDescription_Html;
            dbItem.LongDescription_Html = vmItem.LongDescription_Html;
            dbItem.Note = vmItem.Note;


            _context.Entry(dbItem).Property(nameof(Employee.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.Employee.AsNoTracking()

                .Include(n => n.Fk_Emplyee)
                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            return View(dbItem);
        }

        // POST: Employee/Delete/5
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

                .Include(n => n.Fk_Emplyee)
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

                _context.Entry(dbItem).Property(nameof(Employee.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareListMasterForeignKey(EmployeeBaseViewModel vm = null)
        {
            ViewData["FkEmployeeTypeId"] = new SelectList(
                await _context.EmployeeType.AsNoTracking()
                    .Select(h => new { h.Id, h.Name, h.RowStatus })
                    .Where(h => h.RowStatus == (int)AtRowStatus.Normal)
                    .OrderBy(h => h.Id)
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
        public bool MyProperty { get; set; }
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
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }
        public String EmployeeTypeName { get; set; }
    }

    public class EmployeeCreateViewModel : EmployeeBaseViewModel
    {

    }

    public class EmployeeEditViewModel : EmployeeBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
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

            //RuleFor(h => h.AutoSlug)
            //    ;

            RuleFor(h => h.Address)
                        .MaximumLength(500)
                ;

            RuleFor(h => h.Phone)
                        .MaximumLength(15)
                ;

            RuleFor(h => h.Specialize)
                        .MaximumLength(100)
                ;

            RuleFor(h => h.Fk_EmplyeeId)
                        .MaximumLength(50)
                ;

            RuleFor(h => h.ImageSlug)
                        .MaximumLength(100)
                ;

            RuleFor(h => h.ShortDescription_Html)
                        .MaximumLength(1000)
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
