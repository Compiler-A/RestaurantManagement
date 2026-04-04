using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APILayer
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = string.Join("; ", context.ModelState.Values
                                          .SelectMany(v => v.Errors)
                                          .Select(e => e.ErrorMessage));

                context.Result = new ObjectResult(new
                {
                    statusCode = StatusCodes.Status400BadRequest,
                    message = errors,
                    data = (object?)null
                })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
