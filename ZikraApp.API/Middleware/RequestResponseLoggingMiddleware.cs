using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ZikraApp.API.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var request = context.Request;
            _logger.LogInformation($"HTTP {request.Method} {request.Path}{request.QueryString}");
            await _next(context);
            sw.Stop();
            var response = context.Response;
            _logger.LogInformation($"HTTP {request.Method} {request.Path} responded {response.StatusCode} in {sw.ElapsedMilliseconds} ms");
        }
    }
} 