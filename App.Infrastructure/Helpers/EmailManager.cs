using App.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using RazorLight;
using static ElasticEmailClient.Api;
namespace App.Infrastructure.Helpers
{
    public class EmailManager
    {
        private readonly RazorLightEngine _razorEngine;

        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string MailTemplatePhysicalPath { get; set; }
       

        public EmailManager(IConfiguration config)
        {
            ApiKey = config.GetSection("MailSettings:APIKey").Value;
            FromName = config.GetSection("MailSettings:FromName").Value;
            FromEmail = config.GetSection("MailSettings:FromEmail").Value;
            MailTemplatePhysicalPath = config.GetSection("MailSettings:ReminderEmailPhysicalPath").Value;
            // Configure RazorLight with debug mode for detailed error information
            _razorEngine = new RazorLightEngineBuilder()
    .UseFileSystemProject(MailTemplatePhysicalPath)
    .UseMemoryCachingProvider()
    .Build();


        }

        public async Task<Result> SendEmail(string subject, string[] msgTo, string html)
        {
            try
            {

                await Email.SendAsync(subject, FromEmail, FromName, msgTo: msgTo, bodyHtml: html);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                // Refine error handling
                return ex is ApplicationException
                    ? Result.Fail($"Server didn't accept the request: {ex.Message}")
                    : Result.Fail($"Something unexpected happened: {ex.Message}");
            }
        }

        public async Task<string> RenderTemplateAsync<TModel>(string templatePath, TModel model)
        {
            try
            {
                // Ensure templatePath matches the fully qualified name
                return await _razorEngine.CompileRenderAsync(templatePath, model);
            }
            catch (Exception ex)
            {
                // Handle rendering errors
                throw new Exception($"Template rendering failed for path '{templatePath}': {ex.Message}", ex);
            }
        }
    }
}
