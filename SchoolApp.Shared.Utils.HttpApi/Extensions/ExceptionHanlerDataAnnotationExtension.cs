using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;

namespace SchoolApp.Shared.Utils.HttpApi.Extensions;

public static class ExceptionHanlerDataAnnotationExtension
{
    public static IMvcBuilder HandleDataAnnotationExceptions(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
                new BadRequestObjectResult(context.ModelState)
                {
                    ContentTypes =
                    {
                        Application.Json
                    },
                    Value = new { errorMessage = context.ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)) }
                };
        });
    }
}
