using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Threading.Tasks;

public class FormController : Controller
{
    [HttpPost]
    public async Task<IActionResult> SubmitForm(FormModel model)
    {
        if (ModelState.IsValid)
        {
            // Send form data to x@gmail.com
            await SendEmailAsync("x@gmail.com", "New Form Submission", $"User Email: {model.UserEmail}\nDetail1: {model.Detail1}\nDetail2: {model.Detail2}");

            // Send confirmation to the user
            await SendEmailAsync(model.UserEmail, "Form Submitted", "Please confirm by clicking Yes or No");

            return RedirectToAction("Confirmation");
        }

        return View(model);
    }

    private async Task SendEmailAsync(string email, string subject, string message)
    {
        var mailMessage = new MailMessage("your-email@example.com", email, subject, message);
        using (var smtpClient = new SmtpClient("smtp.your-email-provider.com"))
        {
            smtpClient.Port = 587;
            smtpClient.Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-password");
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }

    public IActionResult Confirmation()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmSubmission(bool accepted)
    {
        if (accepted)
        {
            await SendEmailAsync("x@gmail.com", "User Confirmed", "User has confirmed the submission.");
            await SendEmailAsync("y@gmail.com", "Form Verification", "Please confirm by clicking Yes or No.");
        }
        else
        {
            // Handle rejection
        }

        return RedirectToAction("FinalConfirmation");
    }

    [HttpPost]
    public async Task<IActionResult> FinalConfirmation(bool accepted, string userEmail)
    {
        if (accepted)
        {
            await SendEmailAsync(userEmail, "Form Accepted", "Your form has been accepted.");
        }
        else
        {
            // Handle rejection
        }

        return View();
    }
}
