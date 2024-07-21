using App.Infrastructure.Common;
using RazorLight;
using static ElasticEmailClient.Api;
using Microsoft.Extensions.Configuration;

namespace App.Infrastructure.Helpers
{
    public class EmailManager
    {
        private readonly RazorLightEngine _razorEngine;
        

        public string FromEmail { get; }
        public string FromName { get; }
        public string MailTemplatePhysicalPath { get; }

        public EmailManager(IConfiguration config)
        {
            ApiKey = config.GetValue<string>("MailSettings:APIKey");
            FromName = config.GetValue<string>("MailSettings:FromName");
            FromEmail = config.GetValue<string>("MailSettings:FromEmail");
            MailTemplatePhysicalPath = config.GetValue<string>("MailSettings:ReminderEmailPhysicalPath");

            _razorEngine = new RazorLightEngineBuilder()
                .UseFileSystemProject(MailTemplatePhysicalPath)
                .UseMemoryCachingProvider()
                .Build(); ;

        }

        
        public async Task<Result> SendEmailAsync(string subject, string[] recipients, string html)
        {
            try
            {
                await Email.SendAsync(subject, FromEmail, FromName, msgTo: recipients, bodyHtml: html);
                return Result.Ok();
            }
            catch (ApplicationException ex)
            {
                return Result.Fail($"Server error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Unexpected error: {ex.Message}");
            }
        }

        public async Task<string> RenderTemplateAsync<TModel>(string templatePath, TModel model)
        {
            if (string.IsNullOrEmpty(templatePath))
            {
                throw new ArgumentException("Template path cannot be null or empty", nameof(templatePath));
            }

            try
            {
                return await _razorEngine.CompileRenderAsync(templatePath, model);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to render template at '{templatePath}': {ex.Message}", ex);
            }
        }
    }
}
