using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PassIn.Communication.Responses;
using PassIn.Exceptions;

namespace PassIn.Api.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is PassInException)
            {
                HandleProjectException(context);
                return;
            }

            ThrowUnknownError(context);
        }

        private static void HandleProjectException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case ErrorOnValidationException:
                    context.Result = new BadRequestObjectResult(new ResponseErrorJson(context.Exception.Message));
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    break;
                case NotFoundException:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                    context.Result = new NotFoundObjectResult(new ResponseErrorJson(context.Exception.Message));
                    break;
                case ConflictException:
                    context.HttpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                    context.Result = new ConflictObjectResult(new ResponseErrorJson(context.Exception.Message));
                    break;
                default:
                    ThrowUnknownError(context);
                    break;
            }
        }
        private static void ThrowUnknownError(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Result = new ObjectResult(new ResponseErrorJson("An error occurred while processing the request"));
        }
    }
}
