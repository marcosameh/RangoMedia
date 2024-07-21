using App.Application.Reminders.Create;
using App.Infrastructure.Helpers;

namespace App.UI.BackgroundJobs
{
    public class ReminderJob
    {
        private readonly EmailManager _emailManager;

        public ReminderJob(EmailManager emailManager)
        {
            _emailManager = emailManager;
        }

        public async Task SendReminderEmail(CreateReminderCommand command)
        {
            // Render the email template
            var htmlContent = await _emailManager.RenderTemplateAsync("Views/Emails/ReminderEmail.cshtml", command);

            // Send the email
            await _emailManager.SendEmail("Reminder Notification", new string[] { "marcosameh678@gmail.com" }, htmlContent);
        }
    }
}
