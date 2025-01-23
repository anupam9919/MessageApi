using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MessageAPI.Authentication
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAuthorizationFilter
    {
        private const string ApiKeyHeaderName = "X-API-Key";
        private const string ApiSecretHeaderName = "X-API-Secret";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var configuration = context.HttpContext.RequestServices
                .GetService<IConfiguration>();

            // Retrieve API key and secret from configuration
            var validApiKey = configuration["ApiKeySettings:ApiKey"];
            var validApiSecret = configuration["ApiKeySettings:ApiSecret"];

            // Check if API key and secret are present in headers
            if (!context.HttpContext.Request.Headers.ContainsKey(ApiKeyHeaderName) ||
                !context.HttpContext.Request.Headers.ContainsKey(ApiSecretHeaderName))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Message = "API Key or Secret is missing"
                });
                return;
            }

            // Get submitted API key and secret
            var submittedApiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName].ToString();
            var submittedApiSecret = context.HttpContext.Request.Headers[ApiSecretHeaderName].ToString();

            // Validate API key and secret
            if (submittedApiKey != validApiKey || submittedApiSecret != validApiSecret)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Message = "Invalid API Key or Secret"
                });
                return;
            }
        }
    }
}