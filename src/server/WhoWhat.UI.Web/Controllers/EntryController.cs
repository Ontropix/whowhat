using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WhoWhat.UI.Web.Controllers
{
    public class EntryController : Controller
    {
        public ActionResult Entry()
        {
            return View("Entry");
        }

        public ActionResult Success()
        {
            return View("Success");
        }

        public async Task<ActionResult> Contact(string name, string email, string message)
        {
            string contactEmail = ConfigurationManager.AppSettings["contactEmail"];

            SmtpClient client = new SmtpClient();
            MailMessage mailMessage = new MailMessage(contactEmail, contactEmail)
            {
                Subject = "Contact Formt",
                Body = string.Format("Name: {0}, \nEmail: {1}, \n\nMessage: {2}", name, email, message),
                IsBodyHtml = false
            };

            await client.SendMailAsync(mailMessage);

            return Json(new
            {
                Message = "Thank you for contacting us!"
            });
        }
    }
}
