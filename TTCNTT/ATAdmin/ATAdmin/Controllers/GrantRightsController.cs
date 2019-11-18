using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
using ATAdmin.Efs.Context;
using Newtonsoft.Json;

namespace ATAdmin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GrantRightsController : AtBaseController
    {
        private readonly WebTTCNTTContext _context;

        public GrantRightsController(WebTTCNTTContext context)
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
            var baseQuery = _context.View_Users_Roles.AsNoTracking();
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

            var aspNetUserRoles = await _context.View_Users_Roles.AsNoTracking()
                    .Where(h => h.IdUser == id)
                .FirstOrDefaultAsync();
            if (aspNetUserRoles == null)
            {
                return NotFound();
            }

            return View(aspNetUserRoles);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> PhanQuyen([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.View_Users_Roles.AsNoTracking()
                 .Where(h => h.IdUser == id)
                 .Select(h => new AspNetUserRolesEditViewModel 
                 { 
                     UserId = h.IdUser,
                     RoleId = h.IdRole,
                     TenNguoiDung = h.TenNguoiDung
                 })
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }
            

            var listChuaQuyen = _context.AspNetRoles.ToList();
            var listCoQuyen = _context.View_Roles.Where(h => h.IdTaiKhoan == id).ToList();

            var listQuyenNguoiDung = _context.AspNetUserRoles.Where(h => h.UserId == id).ToList();

            foreach (var item in listQuyenNguoiDung)
            {
                var roles = _context.AspNetRoles.FirstOrDefault(h => h.Id == item.RoleId);
                listChuaQuyen.Remove(roles);
            }

            ViewBag.ListChuaQuyen = listChuaQuyen;
            ViewBag.ListCoQuyen = listCoQuyen;

            return View(dbItem);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> PhanQuyen([FromBody] List<arrayRoles> listRoles, [FromRoute] string ID)
        {
            if (listRoles.Count() == 0)
            {
                return NotFound();
            }

            var listQuyenNguoiDung = _context.AspNetUserRoles.Where(h => h.UserId == ID).ToList();

            if (listQuyenNguoiDung == null)
            {
                return NotFound();
            }
            //nhớ kiểm tra nếu list cũ bằng list mới thì không thay đổi gì.
            foreach (var item in listQuyenNguoiDung)
            {
                var roles = _context.AspNetUserRoles.FirstOrDefault(h => h.UserId == item.UserId);
                _context.AspNetUserRoles.Remove(roles);
                await _context.SaveChangesAsync();
            }


            var dbItem = new AspNetUserRoles();
            foreach (var item in listRoles)
            {
                dbItem = new AspNetUserRoles
                {
                    UserId = ID,
                    RoleId = item.IDroles,

                };
                _context.Add(dbItem);
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Details), new { id = ID });
            //return RedirectToAction(nameof(Index));
        }

        public class arrayRoles
        {
            public String IDroles { get; set; }
        }

        // GET: News/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.View_Users_Roles.AsNoTracking()

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
        public string TenQuyen { get; set; }
        public string TenNguoiDung { get; set; }


    }

    public class AspNetUserRolesDetailsViewModel : AspNetUserRolesBaseViewModel
    {


    }

    public class AspNetUserRolesCreateViewModel : AspNetUserRolesBaseViewModel
    {

    }

    public class AspNetUserRolesEditViewModel : AspNetUserRolesBaseViewModel
    {

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
