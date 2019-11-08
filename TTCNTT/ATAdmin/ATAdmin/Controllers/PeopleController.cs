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
    public class PeopleController : AtBaseController
    {
        private readonly WebAtSolutionContext _context;

        public PeopleController(WebAtSolutionContext context)
        {
            _context = context;
        }

        // GET: AboutUs
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            People dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.People.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(PeopleController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.People.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new PeopleDetailsViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    BirthDay = h.BirthDay,
                    Job = h.Job,
                    JobIntroduction = h.JobIntroduction,
                    Phone = h.Phone,
                    Gmail = h.Gmail,
                    Img = h.Img,
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

            var people = await _context.People.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (people == null)
            {
                return NotFound();
            }

            return View(people);
        }

        // GET: AboutUs/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserPeopleController).Replace("Controller", "");
            return View();
        }

        // POST: AboutUs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] PeopleCreateViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserPeopleController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(People);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space



            // Create save db item
            var dbItem = new People
            {
                Id = Guid.NewGuid().ToString(),
                Name = vmItem.Name,
                BirthDay = vmItem.BirthDay,
                Job = vmItem.Job,
                JobIntroduction = vmItem.JobIntroduction,
                Phone = vmItem.Phone,
                Gmail = vmItem.Gmail,
                Img = vmItem.Img,
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
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserPeopleController).Replace("Controller", "");
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.People.AsNoTracking()

    .Where(h => h.Id == id)

                .Select(h => new PeopleEditViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    BirthDay = h.BirthDay,
                    Job = h.Job,
                    JobIntroduction = h.JobIntroduction,
                    Phone = h.Phone,
                    Gmail = h.Gmail,
                    Img = h.Img,
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
        public async Task<IActionResult> Edit([FromForm] PeopleEditViewModel vmItem)
        {
            ViewData["ControllerNameForImageBrowser"] = nameof(ImageBrowserPeopleController).Replace("Controller", "");
            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(People);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.People
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space

            dbItem.Name = vmItem.Name;
            dbItem.BirthDay = vmItem.BirthDay;
            dbItem.Job = vmItem.Job;
            dbItem.JobIntroduction = vmItem.JobIntroduction;
            dbItem.Phone = vmItem.Phone;
            dbItem.Gmail = vmItem.Gmail;
            dbItem.Img = vmItem.Img;

            // Update db item               
            dbItem.RowVersion = vmItem.RowVersion;



            _context.Entry(dbItem).Property(nameof(People.RowVersion)).OriginalValue = vmItem.RowVersion;
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

            var dbItem = await _context.People.AsNoTracking()

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
            var tableName = nameof(People);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.People

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

                _context.Entry(dbItem).Property(nameof(People.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

    }

    public class ImageBrowserPeopleController : EditorImageBrowserController
    {
        public const string FOLDER_NAME = "ImagesPeople";
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

        public ImageBrowserPeopleController(IHostingEnvironment hostingEnvironment, IConfiguration staticFileSetting)
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

    public class PeopleBaseViewModel
    {
       

    }

    public class PeopleDetailsViewModel : PeopleBaseViewModel
    {

        public String Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string Job { get; set; }
        public string JobIntroduction { get; set; }
        public string Phone { get; set; }
        public string Gmail { get; set; }
        public string Img { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


    }

    public class PeopleCreateViewModel : PeopleBaseViewModel
    {
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string Job { get; set; }
        public string JobIntroduction { get; set; }
        public string Phone { get; set; }
        public string Gmail { get; set; }
        public string Img { get; set; }
    }

    public class PeopleEditViewModel : PeopleBaseViewModel
    {

        public String Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public string Job { get; set; }
        public string JobIntroduction { get; set; }
        public string Phone { get; set; }
        public string Gmail { get; set; }
        public string Img { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class PeopleBaseValidator<T> : AtBaseValidator<T> where T : PeopleBaseViewModel
    {
        public PeopleBaseValidator()
        {
            

        }
    }

    public class PeopleCreateValidator : PeopleBaseValidator<PeopleCreateViewModel>
    {
        public PeopleCreateValidator()
        {
            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(50)
                ;


            RuleFor(h => h.Job)
                         .NotEmpty()
                        .MaximumLength(20)
                ;

            RuleFor(h => h.JobIntroduction)
                        .MaximumLength(200)
                ;


            RuleFor(h => h.Phone)
                        .MaximumLength(10)
                ;

            RuleFor(h => h.Gmail)
                        .MaximumLength(100)
                ;
        }
    }

    public class PeopleEditValidator : PeopleBaseValidator<PeopleEditViewModel>
    {
        public PeopleEditValidator()
        {
            RuleFor(h => h.Id)
                        .NotEmpty()
                        .MaximumLength(50)
                ;
            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(50)
                ;


            RuleFor(h => h.Job)
                         .NotEmpty()
                        .MaximumLength(20)
                ;

            RuleFor(h => h.JobIntroduction)
                        .MaximumLength(200)
                ;


            RuleFor(h => h.Phone)
                        .MaximumLength(10)
                ;

            RuleFor(h => h.Gmail)
                        .MaximumLength(100)
                ;
            RuleFor(h => h.RowVersion)
                        .NotNull()
                ;

        }
    }








}
