using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Serilog;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public RequestLoggingMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _next = next;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();

            // --- تعیین CorrelationId ---
            var correlationId = context.TraceIdentifier;
            if (!context.Request.Headers.ContainsKey("X-Correlation-ID"))
            {
                context.Request.Headers["X-Correlation-ID"] = correlationId;
            }

            // --- تعیین IP ---
            var ip = context.Connection.RemoteIpAddress?.ToString();

            // --- تعیین User ---
            var user = context.User?.Identity?.IsAuthenticated == true
                ? context.User.Identity.Name
                : "Anonymous";

            // --- استخراج اکشن/کنترلر ---
            var endpoint = context.GetEndpoint();
            var actionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

            // اگر درخواست مربوط به MVC Action نباشد → لاگ نزن
            if (actionDescriptor == null)
            {
                await _next(context);
                return;
            }

            var method = context.Request.Method;
            var path = context.Request.Path;
            var controller = actionDescriptor.ControllerName;
            var action = actionDescriptor.ActionName;

            // --- لاگ شروع ---
            Log.Information(
                "START {Method} {Path} | Ctrl:{Controller} | Act:{Action} | User:{User} | IP:{IP} | CorrelationId:{CorrelationId}",
                method, path, controller, action, user, ip, correlationId);

            try
            {
                await _next(context);

                sw.Stop();

                // --- لاگ پایان ---
                Log.Information(
                    "END {Method} {Path} | Ctrl:{Controller} | Act:{Action} | User:{User} | IP:{IP} | CorrelationId:{CorrelationId} | {Duration}ms",
                    method, path, controller, action, user, ip, correlationId, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop();

                // --- لاگ خطا ---
                Log.Error(ex,
                    "ERROR {Method} {Path} | Ctrl:{Controller} | Act:{Action} | User:{User} | IP:{IP} | CorrelationId:{CorrelationId} | {Duration}ms",
                    method, path, controller, action, user, ip, correlationId, sw.ElapsedMilliseconds);

                throw;
            }
        }

    }
}
