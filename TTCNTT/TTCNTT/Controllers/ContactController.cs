using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TTCNTT.Efs.Context;
using TTCNTT.Efs.Entities;
using TTCNTT.Helpers;
using TTCNTT.Models;

namespace TTCNTT.Controllers
{
    
    public class ContactController : Controller
    {
        private readonly WebTTCNTTContext _dbContext;
        private readonly EmailSettings _emailSettings;
        public ContactController(WebTTCNTTContext dbContext, IOptions<EmailSettings> emailSettings)
        {
            _dbContext = dbContext;
            _emailSettings = emailSettings.Value;
        }

        [Route("lien-he")]
        public async Task<IActionResult> Index()
        {
            ContactViewModel model = new ContactViewModel();
            model.setting = await SettingHelper.ReadServerOptionAsync(_dbContext);
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
                contact.CreatedBy = "Customer";
                contact.CreatedDate = DateTime.Now;
                contact.RowStatus = 0;

                _dbContext.Contact.Add(contact);
                await _dbContext.SaveChangesAsync();
                TempData["LienHe"] = "Gửi thành công.";

                ////// Phần Gửi email (1)
                string toEmail = contact.Email; //địa chỉ nhận mail
                string toSubject = "Test gửi mail"; //tiêu đề mail
                string toMessage = "<h1>Nội dung</h1>"; //nội dung mail

                try
                {
                    // Credentials
                    var credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password); //tự động lấy địa chỉ mail và password ở file appsettings.json (để đăng nhập).


                    // Mail message
                    var mail = new MailMessage()
                    {
                        From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName), //địa chỉ gửi, lấy email và username của email.
                        Subject = toSubject,
                        Body = toMessage,
                        IsBodyHtml = true
                    };

                    mail.To.Add(new MailAddress(toEmail)); //gửi mail mới
                                                           //mail.CC.Add(new MailAddress(toEmail));
                                                           //mail.Attachments.Add()  image

                    // Smtp client
                    var client = new SmtpClient()
                    {
                        Port = _emailSettings.MailPort,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Host = _emailSettings.MailServer,
                        EnableSsl = _emailSettings.EnableSSL,
                        Credentials = credentials
                    };

                    // Send it...         
                    await client.SendMailAsync(mail);
                }
                catch (Exception ex)
                {
                    // co loi khi gui
                    throw new InvalidOperationException(ex.Message);
                }


                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(new { errorMessage = ex.ToString() });
            }
        }

        public async Task<IActionResult> TestSendMail(string email = "truongpq197@gmail.com", string subject = "Test gửi mail", string message = "<h1>Nội dung</h1>")
        {
            try
            {
                // Credentials
                var credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);

                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.Sender, _emailSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mail.To.Add(new MailAddress(email));

                // Smtp client
                var client = new SmtpClient()
                {
                    Port = _emailSettings.MailPort,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = _emailSettings.MailServer,
                    EnableSsl = _emailSettings.EnableSSL,
                    Credentials = credentials
                };

                // Send it...         
                await client.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                // TODO: handle exception
                throw new InvalidOperationException(ex.Message);
            }

            return View();
        }

    }
}