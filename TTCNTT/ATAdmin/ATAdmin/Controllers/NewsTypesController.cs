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

namespace ATAdmin.Controllers
{
    public class NewsTypesController : AtBaseController
    {
        private readonly WebAtSolutionContext _webcontext;

        public NewsTypesController(WebAtSolutionContext context)
        {
            _webcontext = context;
        }

        // GET: NewsTypes
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            NewsType dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _webcontext.NewsType.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(NewsTypesController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _webcontext.NewsType.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new NewsTypeDetailsViewModel
                {
                    Id = h.Id,
                    Code = h.Code,
                    Name = h.Name,
                    SlugName = h.SlugName,
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


        // GET: NewsTypes/Details/5
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsType = await _webcontext.NewsType.AsNoTracking()

                    .Where(h => h.SlugName == id)
                .FirstOrDefaultAsync();
            if (newsType == null)
            {
                return NotFound();
            }

            return View(newsType);
        }

        // GET: NewsTypes/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: NewsTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] NewsTypeCreateViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(NewsType);
            var tableVersion = await _webcontext.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space
            vmItem.Code = $"{vmItem.Code}".Trim();
            vmItem.Name = $"{vmItem.Name}".Trim();

            // Check code is existed
            if (await _webcontext.NewsType.AnyAsync(h => h.Code == vmItem.Code))
            {
                ModelState.AddModelError(nameof(NewsType.Code), "The code has been existed.");
                return View(vmItem);
            }


            // Create save db item
            var dbItem = new NewsType
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
                SlugName = vmItem.SlugName,
                AutoSlug = vmItem.AutoSlug,
                Tags = vmItem.Tags,
                KeyWord = vmItem.KeyWord,
                MetaData = vmItem.MetaData,
                Note = vmItem.Note,
            };
            _webcontext.Add(dbItem);

            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _webcontext.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: NewsTypes/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _webcontext.NewsType.AsNoTracking()

    .Where(h => h.SlugName == id)

                .Select(h => new NewsTypeEditViewModel
                {
                    Id = h.Id,
                    Code = h.Code,
                    Name = h.Name,
                    SlugName = h.SlugName,
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

        // POST: NewsTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] NewsTypeEditViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(NewsType);
            var tableVersion = await _webcontext.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _webcontext.NewsType
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space
            vmItem.Code = $"{vmItem.Code}".Trim();
            vmItem.Name = $"{vmItem.Name}".Trim();

            // Check code is existed
            if (await _webcontext.NewsType.AnyAsync(h => h.Id != vmItem.Id && h.Code == vmItem.Code))
            {
                ModelState.AddModelError(nameof(NewsType.Code), "The code has been existed.");
                return View(vmItem);
            }


            // Update db item               
            dbItem.UpdatedBy = _loginUserId;
            dbItem.UpdatedDate = DateTime.Now;
            dbItem.RowVersion = vmItem.RowVersion;

            dbItem.Code = vmItem.Code;
            dbItem.Name = vmItem.Name;
            dbItem.SlugName = vmItem.SlugName;
            dbItem.AutoSlug = vmItem.AutoSlug;
            dbItem.Tags = vmItem.Tags;
            dbItem.KeyWord = vmItem.KeyWord;
            dbItem.MetaData = vmItem.MetaData;
            dbItem.Note = vmItem.Note;

            _webcontext.Entry(dbItem).Property(nameof(NewsType.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _webcontext.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: NewsTypes/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _webcontext.NewsType.AsNoTracking()

                    .Where(h => h.SlugName == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            return View(dbItem);
        }

        // POST: NewsTypes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] string id, [FromForm] byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(NewsType);
            var tableVersion = await _webcontext.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _webcontext.NewsType

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

                _webcontext.Entry(dbItem).Property(nameof(NewsType.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _webcontext.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

    }



    public class NewsTypeBaseViewModel
    {

        public String Code { get; set; }
        public String Name { get; set; }
        public String SlugName { get; set; }
        public Boolean AutoSlug { get; set; }
        public String Tags { get; set; }
        public String KeyWord { get; set; }
        public String MetaData { get; set; }
        public String Note { get; set; }
    }

    public class NewsTypeDetailsViewModel : NewsTypeBaseViewModel
    {

        public String Id { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


    }

    public class NewsTypeCreateViewModel : NewsTypeBaseViewModel
    {

    }

    public class NewsTypeEditViewModel : NewsTypeBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class NewsTypeBaseValidator<T> : AtBaseValidator<T> where T : NewsTypeBaseViewModel
    {
        public NewsTypeBaseValidator()
        {
            RuleFor(h => h.Code)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(100)
                ;

            RuleFor(h => h.SlugName)
                        .NotEmpty()
                        .MaximumLength(100)
                ;

            RuleFor(h => h.AutoSlug)
                ;

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

    public class NewsTypeCreateValidator : NewsTypeBaseValidator<NewsTypeCreateViewModel>
    {
        public NewsTypeCreateValidator()
        {
        }
    }

    public class NewsTypeEditValidator : NewsTypeBaseValidator<NewsTypeEditViewModel>
    {
        public NewsTypeEditValidator()
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
