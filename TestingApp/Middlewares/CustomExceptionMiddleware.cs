using Infrastructure.Exceptions;
using System.Net;

namespace TestingApp.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (FluentValidation.ValidationException e)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                if (e.Errors.Any())
                {
                    await context.Response.WriteAsJsonAsync(e.Errors.Select(x => x.ErrorMessage));
                }
                else
                {
                    await context.Response.WriteAsJsonAsync(e.Message);
                }
            }
            catch (NotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            catch (ConcurencyUpdateException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            }
        }
    }
}
