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
using Microsoft.AspNetCore.Authorization;

namespace ATAdmin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : AtBaseController
    {
        private readonly WebAtSolutionContext _context;

        public RoleController(WebAtSolutionContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index([FromRoute]string id)
        {
            AspNetRoles dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.AspNetRoles.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(RoleController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.AspNetRoles.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery

                .Select(h => new RoleDetailsViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    NormalizedName = h.NormalizedName,
                    ConcurrencyStamp = h.ConcurrencyStamp,

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

            var aspNetRoles = await _context.AspNetRoles.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (aspNetRoles == null)
            {
                return NotFound();
            }

            return View(aspNetRoles);
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
        public async Task<IActionResult> Create([FromForm] RoleCreateViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(AspNetRoles);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space



            // Create save db item
            var dbItem = new AspNetRoles
            {
                Id = Guid.NewGuid().ToString(),
                Name = vmItem.Name,
                NormalizedName = vmItem.NormalizedName,
                ConcurrencyStamp = vmItem.ConcurrencyStamp,



            };
            _context.Add(dbItem);

            // Set time stamp for table to handle concurrency conflict

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


            var dbItem = await _context.AspNetRoles.AsNoTracking()

    .Where(h => h.Id == id)

                .Select(h => new RoleEditViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    NormalizedName = h.NormalizedName,
                    ConcurrencyStamp = h.ConcurrencyStamp,

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
        public async Task<IActionResult> Edit([FromForm] RoleEditViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(AspNetRoles);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.AspNetRoles
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            dbItem.Name = vmItem.Name;
            dbItem.NormalizedName = vmItem.NormalizedName;
            dbItem.ConcurrencyStamp = vmItem.ConcurrencyStamp;

            _context.Entry(dbItem);
            // Set time stamp for table to handle concurrency conflict

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

            var dbItem = await _context.AspNetRoles.AsNoTracking()

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
            var tableName = nameof(AspNetRoles);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.AspNetRoles

                .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

           

            // Update db item               

            _context.Remove(dbItem);
            // Set time stamp for table to handle concurrency conflict

            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }

    }



    public class RoleBaseViewModel
    {
        public String Name { get; set; }
        public String NormalizedName { get; set; }
        public String ConcurrencyStamp { get; set; }
    }

    public class RoleDetailsViewModel : RoleBaseViewModel
    {

        public String Id { get; set; }



    }

    public class RoleCreateViewModel : RoleBaseViewModel
    {

    }

    public class RoleEditViewModel : RoleBaseViewModel
    {

        public String Id { get; set; }


    }

    public class RoleBaseValidator<T> : AtBaseValidator<T> where T : RoleBaseViewModel
    {
        public RoleBaseValidator()
        {
            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(256)
                ;
            RuleFor(h => h.NormalizedName)
                        .MaximumLength(256)
                ;
            RuleFor(h => h.ConcurrencyStamp)
                       .MaximumLength(10000)
               ;
        }
    }

    public class RoleCreateValidator : RoleBaseValidator<RoleCreateViewModel>
    {
        public RoleCreateValidator()
        {
        }
    }

    public class RoleEditValidator : RoleBaseValidator<RoleEditViewModel>
    {
        public RoleEditValidator()
        {
            RuleFor(h => h.Id)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

        }
    }








}
