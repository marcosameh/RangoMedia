using App.Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using RazorLight;

namespace App.UI.Helpers
{
    public class EmailManager
    {
       
        private readonly RazorLightEngine _razorEngine;

        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string ApiKey { get; set; }

        public EmailManager(IConfiguration config)
        {
            
            ApiKey = config.GetSection("MailSettings:APIKey").Value;
            FromName = config.GetSection("MailSettings:FromName").Value;
            FromEmail = config.GetSection("MailSettings:FromEmail").Value;

            _razorEngine = new RazorLightEngineBuilder()
                .UseEmbeddedResourcesProject(typeof(EmailManager))
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<Result> SendEmail(string subject, string[] msgTo, string html)
        {
            ApiKey = ApiKey;
            try
            {
                await ElasticEmailClient.Api.Email.SendAsync(subject, FromEmail, FromName, msgTo: msgTo, bodyHtml: html);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                if (ex is ApplicationException)
                {
                    return Result.Fail("Server didn't accept the request: " + ex.Message);
                }
                else
                {
                    return Result.Fail("Something unexpected happened: " + ex.Message);
                }
            }
        }

        public async Task<string> RenderTemplateAsync<TModel>(string templatePath, TModel model)
        {
            return await _razorEngine.CompileRenderAsync(templatePath, model);
        }
    }
}
