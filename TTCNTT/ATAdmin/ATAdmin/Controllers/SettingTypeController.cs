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
    public class SettingTypeController : AtBaseController
    {
        private readonly WebAtSolutionContext _context;

        public SettingTypeController(WebAtSolutionContext context)
        {
            _context = context;
        }

        // GET: AboutUs
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            SettingType dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.SettingType.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(SettingTypeController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.SettingType.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new SettingTypeDetailsViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
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

            var settingType = await _context.SettingType.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (settingType == null)
            {
                return NotFound();
            }

            return View(settingType);
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
        public async Task<IActionResult> Create([FromForm] SettingTypeCreateViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(SettingType);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space



            // Create save db item
            var dbItem = new SettingType
            {
                Id = Guid.NewGuid().ToString(),
                Name = vmItem.Name,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,


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


            var dbItem = await _context.SettingType.AsNoTracking()

    .Where(h => h.Id == id)

                .Select(h => new SettingTypeEditViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
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
        public async Task<IActionResult> Edit([FromForm] SettingTypeEditViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(SettingType);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.SettingType
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

                 dbItem.Name = vmItem.Name;

            _context.Entry(dbItem).Property(nameof(SettingType.RowVersion)).OriginalValue = vmItem.RowVersion;
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

            var dbItem = await _context.SettingType.AsNoTracking()

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
            var tableName = nameof(SettingType);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.SettingType

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
                _context.Entry(dbItem).Property(nameof(SettingType.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

    }



    public class SettingTypeBaseViewModel
    {
        public String Name { get; set; }
    }

    public class SettingTypeDetailsViewModel : SettingTypeBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


    }

    public class SettingTypeCreateViewModel : SettingTypeBaseViewModel
    {

    }

    public class SettingTypeEditViewModel : SettingTypeBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class SettingTypeBaseValidator<T> : AtBaseValidator<T> where T : SettingTypeBaseViewModel
    {
        public SettingTypeBaseValidator()
        {
            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(500)
                ;
        }
    }

    public class SettingTypeCreateValidator : SettingTypeBaseValidator<SettingTypeCreateViewModel>
    {
        public SettingTypeCreateValidator()
        {
        }
    }

    public class SettingTypeEditValidator : SettingTypeBaseValidator<SettingTypeEditViewModel>
    {
        public SettingTypeEditValidator()
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
