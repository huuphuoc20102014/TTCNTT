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
    public class GrantRightsController : AtBaseController
    {
        private readonly WebAtSolutionContext _context;

        public GrantRightsController(WebAtSolutionContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            AspNetUserRoles dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.AspNetUserRoles.AsNoTracking().FirstOrDefaultAsync(h => h.UserId == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(GrantRightsController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.ViewUserRole.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.IdUser == parentId);
            }
            var query = baseQuery
                .Select(h => new AspNetUserRolesDetailsViewModel
                {
                    UserId = h.IdUser,
                    RoleId = h.IdRole,
                    TenNguoiDung = h.TenNguoiDung,
                    TenQuyen = h.TenQuyen
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

            var aspNetUserRoles = await _context.ViewUserRole.AsNoTracking()
                    .Where(h => h.IdUser == id)
                .FirstOrDefaultAsync();
            if (aspNetUserRoles == null)
            {
                return NotFound();
            }

            return View(aspNetUserRoles);
        }

        // GET: News/Create
        public async Task<IActionResult> Create()
        {
            if (User.Identity.IsAuthenticated)
            {
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
        public async Task<IActionResult> Create([FromForm] AspNetUserRolesCreateViewModel vmItem)
        {

            if (!ModelState.IsValid)
            {

                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(AspNetUserRoles);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = new AspNetUserRoles
            {
                UserId = vmItem.UserId,
                RoleId = vmItem.RoleId,

            };
            _context.Add(dbItem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.UserId });
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {

            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.AspNetUserRoles.AsNoTracking()
                 .Where(h => h.UserId == id)
                .Select(h => new AspNetUserRolesEditViewModel
                {
                    UserId = h.UserId,
                    RoleId = h.RoleId,
                    
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
        public async Task<IActionResult> Edit([FromForm] AspNetUserRolesEditViewModel vmItem, [FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(AspNetUserRoles);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.AspNetUserRoles

                .Where(h => h.UserId == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }
            _context.Remove(dbItem);
            await _context.SaveChangesAsync();

           
            var dbItem1 = new AspNetUserRoles
            {
                UserId = id,
                RoleId = vmItem.RoleId,

            };
            _context.Add(dbItem1);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem1.UserId });
        }

        // GET: News/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.ViewUserRole.AsNoTracking()

                .Where(h => h.IdUser == id)
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
            var tableName = nameof(AspNetUserRoles);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.AspNetUserRoles

                .Where(h => h.UserId == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }
            _context.Remove(dbItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task PrepareListMasterForeignKey(AspNetUserRolesBaseViewModel vm = null)
        {
            ViewData["UserId"] = new SelectList(
                await _context.AspNetUsers.AsNoTracking()
                    .Select(h => new {h.Id, h.UserName})
                    .ToListAsync(),
                 "Id", "UserName", vm?.UserId);
            ViewData["RoleId"] = new SelectList(
               await _context.AspNetRoles.AsNoTracking()
                   .Select(h => new {h.Name,h.Id })
                   .ToListAsync(),
                  "Id","Name", vm?.RoleId);

        }
    }

    


    public class AspNetUserRolesBaseViewModel
    {
        public String UserId { get; set; }
        public String RoleId { get; set; }
       
    }

    public class AspNetUserRolesDetailsViewModel : AspNetUserRolesBaseViewModel
    {
        public String TenQuyen { get; set; }
        public String TenNguoiDung { get; set; }
    }

    public class AspNetUserRolesCreateViewModel : AspNetUserRolesBaseViewModel
    {

    }

    public class AspNetUserRolesEditViewModel : AspNetUserRolesBaseViewModel
    {
        public virtual AspNetRoles Role { get; set; }
        public virtual AspNetUsers User { get; set; }
    }

    public class AspNetUserRolesBaseValidator<T> : AtBaseValidator<T> where T : AspNetUserRolesBaseViewModel
    {
        public AspNetUserRolesBaseValidator()
        {

        }
    }

    public class AspNetUserRolesCreateValidator : AspNetUserRolesBaseValidator<AspNetUserRolesCreateViewModel>
    {
        public AspNetUserRolesCreateValidator()
        {
        }
    }

    public class AspNetUserRolesEditValidator : AspNetUserRolesBaseValidator<AspNetUserRolesEditViewModel>
    {
        public AspNetUserRolesEditValidator()
        {
           

        }
    }








}
