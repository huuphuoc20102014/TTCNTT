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
    public class ImageSlidesController : AtBaseController
    {
        private readonly WebAtSolutionContext _context;

        public ImageSlidesController(WebAtSolutionContext context)
        {
            _context = context;
        }

        // GET: ImageSlides
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            ImageSlide dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.ImageSlide.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(ImageSlidesController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.ImageSlide.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Select(h => new ImageSlideDetailsViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    SlugName = h.SlugName,
                    Extension = h.Extension,
                    Description = h.Description,
                    SortIndex = h.SortIndex,
                    IsYoutube = h.IsYoutube,
                    YoutubeLink = h.YoutubeLink,
                    Thumbnail = h.Thumbnail,
                    Note = h.Note,
                    Tags = h.Tags,
                    KeyWord = h.KeyWord,
                    MetaData = h.MetaData,
                    CreatedBy = h.CreatedBy,
                    CreatedDate = h.CreatedDate,
                    UpdatedBy = h.UpdatedBy,
                    UpdatedDate = h.UpdatedDate,
                    RowVersion = h.RowVersion,
                    RowStatus = (AtRowStatus)h.RowStatus,

                });

            return Json(await query.ToDataSourceResultAsync(request));
        }


        // GET: ImageSlides/Details/5
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageSlide = await _context.ImageSlide.AsNoTracking()

                    .Where(h => h.SlugName == id)
                .FirstOrDefaultAsync();
            if (imageSlide == null)
            {
                return NotFound();
            }

            return View(imageSlide);
        }

        // GET: ImageSlides/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: ImageSlides/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ImageSlideCreateViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ImageSlide);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space
            vmItem.Name = $"{vmItem.Name}".Trim();



            // Create save db item
            var dbItem = new ImageSlide
            {
                Id = Guid.NewGuid().ToString(),

                CreatedBy = _loginUserId,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = null,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,

                Name = vmItem.Name,
                SlugName = vmItem.SlugName,
                AutoSlug = vmItem.AutoSlug,
                Extension = vmItem.Extension,
                Description = vmItem.Description,
                SortIndex = vmItem.SortIndex,
                IsYoutube = vmItem.IsYoutube,
                YoutubeLink = vmItem.YoutubeLink,
                Thumbnail = vmItem.Thumbnail,
                Note = vmItem.Note,
                Tags = vmItem.Tags,
                KeyWord = vmItem.KeyWord,
                MetaData = vmItem.MetaData,
            };
            _context.Add(dbItem);

            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: ImageSlides/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.ImageSlide.AsNoTracking()
                .Where(h => h.SlugName == id)
                .Select(h => new ImageSlideEditViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    SlugName = h.SlugName,
                    AutoSlug = h.AutoSlug,
                    Extension = h.Extension,
                    Description = h.Description,
                    SortIndex = h.SortIndex,
                    IsYoutube = h.IsYoutube,
                    YoutubeLink = h.YoutubeLink,
                    Thumbnail = h.Thumbnail,
                    Note = h.Note,
                    Tags = h.Tags,
                    KeyWord = h.KeyWord,
                    MetaData = h.MetaData,
                    RowVersion = h.RowVersion,
                })
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }


            return View(dbItem);
        }

        // POST: ImageSlides/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ImageSlideEditViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ImageSlide);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.ImageSlide
                .Where(h => h.Id == vmItem.Id)

                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            // Trim white space
            vmItem.Name = $"{vmItem.Name}".Trim();



            // Update db item               
            dbItem.UpdatedBy = _loginUserId;
            dbItem.UpdatedDate = DateTime.Now;
            dbItem.RowVersion = vmItem.RowVersion;

            dbItem.Name = vmItem.Name;
            dbItem.SlugName = vmItem.SlugName;
            dbItem.AutoSlug = vmItem.AutoSlug;
            dbItem.Extension = vmItem.Extension;
            dbItem.Description = vmItem.Description;
            dbItem.SortIndex = vmItem.SortIndex;
            dbItem.IsYoutube = vmItem.IsYoutube;
            dbItem.YoutubeLink = vmItem.YoutubeLink;
            dbItem.Thumbnail = vmItem.Thumbnail;
            dbItem.Note = vmItem.Note;
            dbItem.Tags = vmItem.Tags;
            dbItem.KeyWord = vmItem.KeyWord;
            dbItem.MetaData = vmItem.MetaData;

            _context.Entry(dbItem).Property(nameof(ImageSlide.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: ImageSlides/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.ImageSlide.AsNoTracking()

                    .Where(h => h.SlugName == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            return View(dbItem);
        }

        // POST: ImageSlides/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] string id, [FromForm] byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(ImageSlide);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.ImageSlide

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

                _context.Entry(dbItem).Property(nameof(ImageSlide.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

    }



    public class ImageSlideBaseViewModel
    {

        public String Name { get; set; }
        public String SlugName { get; set; }
        public Boolean AutoSlug { get; set; }
        public String Extension { get; set; }
        public String Description { get; set; }
        public Int32 SortIndex { get; set; }
        public Boolean IsYoutube { get; set; }
        public String YoutubeLink { get; set; }
        public String Thumbnail { get; set; }
        public String Note { get; set; }
        public String Tags { get; set; }
        public String KeyWord { get; set; }
        public String MetaData { get; set; }
    }

    public class ImageSlideDetailsViewModel : ImageSlideBaseViewModel
    {

        public String Id { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


    }

    public class ImageSlideCreateViewModel : ImageSlideBaseViewModel
    {

    }

    public class ImageSlideEditViewModel : ImageSlideBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class ImageSlideBaseValidator<T> : AtBaseValidator<T> where T : ImageSlideBaseViewModel
    {
        public ImageSlideBaseValidator()
        {
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

            RuleFor(h => h.Extension)
                        .NotEmpty()
                        .MaximumLength(5)
                ;

            RuleFor(h => h.Description)
                        .MaximumLength(1000)
                ;

            RuleFor(h => h.SortIndex)
                ;

            RuleFor(h => h.IsYoutube)
                ;

            RuleFor(h => h.YoutubeLink)
                        .MaximumLength(500)
                ;

            RuleFor(h => h.Thumbnail)
                        .MaximumLength(500)
                ;

            RuleFor(h => h.Note)
                        .MaximumLength(1000)
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

        }
    }

    public class ImageSlideCreateValidator : ImageSlideBaseValidator<ImageSlideCreateViewModel>
    {
        public ImageSlideCreateValidator()
        {
        }
    }

    public class ImageSlideEditValidator : ImageSlideBaseValidator<ImageSlideEditViewModel>
    {
        public ImageSlideEditValidator()
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
