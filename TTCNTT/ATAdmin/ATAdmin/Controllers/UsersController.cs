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
    public class UsersController : AtBaseController
    {
        private readonly WebTTCNTTContext _context;

        public UsersController(WebTTCNTTContext context)
        {
            _context = context;
        }

        // GET: AboutUs
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            AspNetUsers dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.AspNetUsers.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(UsersController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.AspNetUsers.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Select(h => new UsersDetailsViewModel
                {
                    Id = h.Id,
                    UserName = h.UserName,
                    NormalizedUserName = h.NormalizedUserName,
                    Email = h.Email,
                    NormalizedEmail = h.NormalizedEmail,
                    EmailConfirmed = h.EmailConfirmed,
                    PasswordHash = h.PasswordHash,

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

            var user = await _context.AspNetUsers.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: AboutUs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserUsersController).Replace("Controller", "");
            return View();
        }

        // POST: AboutUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] UsersCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserUsersController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(AspNetUsers);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space



            // Create save db item
            var dbItem = new AspNetUsers
            {
                Id = Guid.NewGuid().ToString(),

                UserName = vmItem.UserName,
                NormalizedUserName = vmItem.UserName.ToUpper(),
                Email = vmItem.Email,
                NormalizedEmail = vmItem.Email.ToUpper(),
                EmailConfirmed = vmItem.EmailConfirmed,
                PasswordHash = vmItem.PasswordHash,
            };
            _context.Add(dbItem);

            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new
            {
                id = dbItem.Id
            });
        }

        // GET: AboutUs/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserUsersController).Replace("Controller", "");
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.AspNetUsers.AsNoTracking()

        .Where(h => h.Id == id)

                .Select(h => new UsersEditViewModel
                {
                    Id = h.Id,
                    UserName = h.UserName,
                    NormalizedUserName = h.NormalizedUserName,
                    Email = h.Email,
                    NormalizedEmail = h.NormalizedEmail,
                    EmailConfirmed = h.EmailConfirmed,
                    PasswordHash = h.PasswordHash,
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
        public async Task<IActionResult> Edit([FromForm] UsersEditViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserUsersController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(AspNetUsers);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.AspNetUsers
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space



            // Update db item       

            dbItem.UserName = vmItem.UserName;
            dbItem.NormalizedUserName = vmItem.UserName.ToUpper();
            dbItem.Email = vmItem.Email;
            dbItem.NormalizedEmail = vmItem.Email.ToUpper();
            dbItem.EmailConfirmed = vmItem.EmailConfirmed;
            dbItem.PasswordHash = vmItem.PasswordHash;

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

            var dbItem = await _context.AspNetUsers.AsNoTracking()

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

            var dbItem = await _context.AspNetUsers

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

    public class ImageBrowserUsersController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesUsers";
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

        public ImageBrowserUsersController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
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

    public class UsersBaseViewModel
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Img { get; set; }
    }

    public class UsersDetailsViewModel : UsersBaseViewModel
    {


    }

    public class UsersCreateViewModel : UsersBaseViewModel
    {

    }

    public class UsersEditViewModel : UsersBaseViewModel
    {

    }

    public class UsersBaseValidator<T> : AtBaseValidator<T> where T : UsersBaseViewModel
    {
        public UsersBaseValidator()
        {

            RuleFor(h => h.UserName)
                        .MaximumLength(256)
                ;

            RuleFor(h => h.NormalizedUserName)
                        .MaximumLength(256)
                ;

            RuleFor(h => h.Email)
                        .MaximumLength(256)
                ;

            RuleFor(h => h.NormalizedEmail)
                        .MaximumLength(256)
                ;

        }
    }

    public class UsersCreateValidator : UsersBaseValidator<UsersCreateViewModel>
    {
        public UsersCreateValidator()
        {
        }
    }

    public class UsersEditValidator : UsersBaseValidator<UsersEditViewModel>
    {
        public UsersEditValidator()
        {
            RuleFor(h => h.Id)
                        .NotEmpty()
                        .MaximumLength(50)
                ;


        }
    }








}
