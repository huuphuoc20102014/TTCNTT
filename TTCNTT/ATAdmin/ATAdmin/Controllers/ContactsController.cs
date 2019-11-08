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
    public class ContactsController : AtBaseController
    {
        private readonly WebAtSolutionContext _context;

        public ContactsController(WebAtSolutionContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index([FromRoute]string id)
        {
            Contact dbItem = null;
            if (!string.IsNullOrWhiteSpace(id))
            {
                dbItem = await _context.Contact.AsNoTracking().FirstOrDefaultAsync(h => h.Id == id);
                if (dbItem == null)
                {
                    return NotFound();
                }
            }
            ViewData["ParentItem"] = dbItem;

            ViewData["ControllerNameForGrid"] = nameof(ContactsController).Replace("Controller", "");
            return View();
        }

        public async Task<IActionResult> Index_Read([DataSourceRequest] DataSourceRequest request, string parentId)
        {
            var baseQuery = _context.Contact.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(parentId))
            {
                baseQuery = baseQuery.Where(h => h.Id == parentId);
            }
            var query = baseQuery
                .Where(p => p.RowStatus == (int)AtRowStatus.Normal)
                .Select(h => new ContactDetailsViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    Email = h.Email,
                    Phone = h.Phone,
                    Title = h.Title,
                    Body = h.Body,
                    IsRead = h.IsRead,
                    FkProductCommentId = h.FkProductCommentId,
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


        // GET: Contacts/Details/5
        public async Task<IActionResult> Details([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ContactCreateViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Contact);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            // Trim white space
            vmItem.Name = $"{vmItem.Name}".Trim();



            // Create save db item
            var dbItem = new Contact
            {
                Id = Guid.NewGuid().ToString(),

                CreatedBy = _loginUserId,
                CreatedDate = DateTime.Now,
                UpdatedBy = null,
                UpdatedDate = null,
                RowStatus = (int)AtRowStatus.Normal,
                RowVersion = null,

                Name = vmItem.Name,
                Email = vmItem.Email,
                Phone = vmItem.Phone,
                Title = vmItem.Title,
                Body = vmItem.Body,
                IsRead = vmItem.IsRead,
                FkProductCommentId = vmItem.FkProductCommentId,
                Note = vmItem.Note,
            };
            _context.Add(dbItem);

            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var dbItem = await _context.Contact.AsNoTracking()

    .Where(h => h.Id == id)

                .Select(h => new ContactEditViewModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    Email = h.Email,
                    Phone = h.Phone,
                    Title = h.Title,
                    Body = h.Body,
                    IsRead = h.IsRead,
                    FkProductCommentId = h.FkProductCommentId,
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

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ContactEditViewModel vmItem)
        {

            // Invalid model
            if (!ModelState.IsValid)
            {
                return View(vmItem);
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Contact);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Contact
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
            dbItem.Email = vmItem.Email;
            dbItem.Phone = vmItem.Phone;
            dbItem.Title = vmItem.Title;
            dbItem.Body = vmItem.Body;
            dbItem.IsRead = vmItem.IsRead;
            dbItem.FkProductCommentId = vmItem.FkProductCommentId;
            dbItem.Note = vmItem.Note;

            _context.Entry(dbItem).Property(nameof(Contact.RowVersion)).OriginalValue = vmItem.RowVersion;
            // Set time stamp for table to handle concurrency conflict
            tableVersion.LastModify = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = dbItem.Id });
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dbItem = await _context.Contact.AsNoTracking()

                    .Where(h => h.Id == id)
                .FirstOrDefaultAsync();
            if (dbItem == null)
            {
                return NotFound();
            }

            return View(dbItem);
        }

        // POST: Contacts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] string id, [FromForm] byte[] rowVersion)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Get time stamp for table to handle concurrency conflict
            var tableName = nameof(Contact);
            var tableVersion = await _context.TableVersion.FirstOrDefaultAsync(h => h.Id == tableName);

            var dbItem = await _context.Contact

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

                _context.Entry(dbItem).Property(nameof(Contact.RowVersion)).OriginalValue = rowVersion;
                // Set time stamp for table to handle concurrency conflict
                tableVersion.LastModify = DateTime.Now;
                await _context.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

    }



    public class ContactBaseViewModel
    {

        public String Name { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public String Title { get; set; }
        public String Body { get; set; }
        public Boolean IsRead { get; set; }
        public String FkProductCommentId { get; set; }
        public String Note { get; set; }
    }

    public class ContactDetailsViewModel : ContactBaseViewModel
    {

        public String Id { get; set; }
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public String UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public Byte[] RowVersion { get; set; }
        public AtRowStatus RowStatus { get; set; }


    }

    public class ContactCreateViewModel : ContactBaseViewModel
    {

    }

    public class ContactEditViewModel : ContactBaseViewModel
    {

        public String Id { get; set; }
        public Byte[] RowVersion { get; set; }
    }

    public class ContactBaseValidator<T> : AtBaseValidator<T> where T : ContactBaseViewModel
    {
        public ContactBaseValidator()
        {
            RuleFor(h => h.Name)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Email)
                        .NotEmpty()
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Phone)
                        .NotEmpty()
                        .MaximumLength(20)
                ;

            RuleFor(h => h.Title)
                        .NotEmpty()
                        .MaximumLength(1000)
                ;

            RuleFor(h => h.Body)
                        .NotEmpty()
                        .MaximumLength(4000)
                ;

            RuleFor(h => h.IsRead)
                ;

            RuleFor(h => h.FkProductCommentId)
                        .MaximumLength(50)
                ;

            RuleFor(h => h.Note)
                        .MaximumLength(1000)
                ;

        }
    }

    public class ContactCreateValidator : ContactBaseValidator<ContactCreateViewModel>
    {
        public ContactCreateValidator()
        {
        }
    }

    public class ContactEditValidator : ContactBaseValidator<ContactEditViewModel>
    {
        public ContactEditValidator()
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
