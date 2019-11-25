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
    public class EmployeeTypeController : AtBaseController
    {
        private readonly WebTTCNTTContext _context;

        public EmployeeTypeController(WebTTCNTTContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            EmployeeType dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.EmployeeType.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(EmployeeTypeController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.EmployeeType.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new EmployeeTypeDetailsViewModel
                {
                    Id = h.Id,
                    Code = h.Code,
                    Name = h.Name,
                    SlugName = h.Slug_Name,
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

            var employeetype = await _context.EmployeeType.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (employeetype == null)
            {
                return NotFound();
            }

            return View(employeetype);
        }

        // GET: AboutUs/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: AboutUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm]  EmployeeTypeCreateViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(EmployeeType);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space

            //auto slug
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
            var listExistedSlug = await _context.EmployeeType.AsNoTracking()
                    .Where(h => h.Id.StartsWith(vmItem.SlugName))
                    .Select(h => h.Slug_Name).ToListAsync();
            var slug = CheckAndGenNextSlug(vmItem.SlugName, listExistedSlug);


            // Create save db item
            var dbItem = new EmployeeType
            {
                Id = Guid.NewGuid().ToString(),
                Code = vmItem.Code,
                Name = vmItem.Name,
                Slug_Name = vmItem.SlugName,
                AutoSlug = vmItem.AutoSlug,
                CreatedBy = _loginUserId,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = null,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,


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
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.EmployeeType.AsNoTracking()

    .Where(h => h.Id == id)

                .Select(h => new EmployeeTypeEditViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    Code = h.Code,
                    SlugName = h.Slug_Name,
                    AutoSlug = h.AutoSlug,
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
        public async Task<IActionResult> Edit([FromForm]  EmployeeTypeEditViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(EmployeeType);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.EmployeeType
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space
            //auto slug
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
            var listExistedSlug = await _context.EmployeeType.AsNoTracking()
                    .Where(h => h.Id.StartsWith(vmItem.SlugName))
                    .Select(h => h.Slug_Name).ToListAsync();
            var slug = CheckAndGenNextSlug(vmItem.SlugName, listExistedSlug);

            //save db
            dbItem.Name = vmItem.Name;
            dbItem.Code = vmItem.Code;
            dbItem.Slug_Name = vmItem.SlugName;
            dbItem.AutoSlug = vmItem.AutoSlug;

            // Update db item               
            dbItem.UpdatedBy = _loginUserId;
            dbItem.UpdatedDate = DateTime.Now;
            dbItem.RowVersion = vmItem.RowVersion;


            dbItem.Tags = vmItem.Tags;
            dbItem.KeyWord = vmItem.KeyWord;
            dbItem.MetaData = vmItem.MetaData;
            dbItem.Note = vmItem.Note;

            _context.Entry(dbItem).Property(nameof(EmployeeType.RowVersion)).OriginalValue = vmItem.RowVersion;
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

            var dbItem = await _context.EmployeeType.AsNoTracking()

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
            var tableName = nameof(EmployeeType);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.EmployeeType

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

                _context.Entry(dbItem).Property(nameof(EmployeeType.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }
    }

    public class EmployeeTypeBaseViewModel
    {

        public string Code { get; set; }
        public string Name { get; set; }
        public String SlugName { get; set; }
        public Boolean AutoSlug { get; set; }
        public String Tags { get; set; }
        public String KeyWord { get; set; }
        public String MetaData { get; set; }
        public String Note { get; set; }
    }

    public class EmployeeTypeDetailsViewModel : EmployeeTypeBaseViewModel
    {

        public String Id { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


    }

    public class EmployeeTypeCreateViewModel : EmployeeTypeBaseViewModel
    {

    }

    public class EmployeeTypeEditViewModel : EmployeeTypeBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class EmployeeTypeBaseValidator<T> : AtBaseValidator<T> where T : EmployeeTypeBaseViewModel
    {
        public EmployeeTypeBaseValidator()
        {

            RuleFor(h => h.AutoSlug)
                ;

            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(100)
  ;
            RuleFor(h => h.Code)
                        .NotEmpty()
                        .MaximumLength(50);

            RuleFor(h => h.SlugName)
                        .NotEmpty()
                        .MaximumLength(100);

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

    public class EmployeeTypeCreateValidator : EmployeeTypeBaseValidator<EmployeeTypeCreateViewModel>
    {
        public EmployeeTypeCreateValidator()
        {


        }
    }

    public class EmployeeTypeEditValidator : EmployeeTypeBaseValidator<EmployeeTypeEditViewModel>
    {
        public EmployeeTypeEditValidator()
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