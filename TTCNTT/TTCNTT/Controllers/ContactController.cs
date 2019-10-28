using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TTCNTT.Efs.Context;
using TTCNTT.Efs.Entities;
using TTCNTT.Models;

namespace TTCNTT.Controllers
{
    public class ContactController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        public ContactController(WebTTCNTTContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("lien-he")]
        public async Task<IActionResult> Index()
        {
            ContactViewModel model = new ContactViewModel();
            model.menu = await _dbContext.Menu.FirstOrDefaultAsync(h => h.Slug_Name == "lien-he");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewContact(string name, string email, string phone, string content)
        {
            Contact contact = new Contact();

            try
            {
                contact.Id = Guid.NewGuid().ToString();
                contact.Name = name;
                contact.Email = email;
                contact.Phone = phone;
                contact.Body = content;
                contact.IsRead = false;
                contact.CreatedBy = name;
                contact.CreatedDate = DateTime.Now;
                contact.RowStatus = 0;

                _dbContext.Contact.Add(contact);
                await _dbContext.SaveChangesAsync();
                TempData["LienHe"] = "Gửi thành công.";

                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(new { errorMessage = ex.ToString() });
            }
        }
    }
}