using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace DoctorAppointmentBooking.Middlewares
{
    public class BookingNotAllowedMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public BookingNotAllowedMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var bookingNotAllowed = _configuration["BookingNotAllowed"];

            if (bookingNotAllowed != null && bool.TryParse(bookingNotAllowed, out bool isNotAllowed) && isNotAllowed)
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Currently t is impossible to book.");
                return;
            }

            await _next(context);
        }
    }
}
