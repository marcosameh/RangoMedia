using App.Application.Reminders.Create;
using App.Infrastructure.Helpers;
using App.UI.Helpers;

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
            try
            {
                // Ensure template path is correct
                var htmlContent = await _emailManager.RenderTemplateAsync("/Emails/ReminderEmail.cshtml", command);

                // Send the email
                await _emailManager.SendEmail("Reminder Notification", new[] { "marcosameh678@gmail.com" }, htmlContent);
            }
            catch (Exception ex)
               
            {
                // Log or handle exceptions
                throw new Exception($"Failed to send reminder email: {ex.Message}", ex);
            }
        }
    }
}
