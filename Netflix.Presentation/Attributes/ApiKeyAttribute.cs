using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Netflix.Presentation.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";
    private const string HardcodedApiKey = "secret123";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedObjectResult(new { Message = "API Key was not provided." });
            return;
        }

        if (!extractedApiKey.Equals(HardcodedApiKey))
        {
            context.Result = new UnauthorizedObjectResult(new { Message = "Unauthorized: Invalid API Key." });
            return;
        }

        await next();
    }
}
