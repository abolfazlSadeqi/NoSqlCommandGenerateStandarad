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

            var method = context.Request.Method;
            var path = context.Request.Path;

            // در اینجا اطلاعات کنترلر و اکشن را پیدا می‌کنیم
            string controller = null;
            string action = null;

            var routeData = context.GetRouteData();
            if (routeData?.Values.ContainsKey("controller") == true)
            {
                controller = routeData.Values["controller"]?.ToString();
            }
            if (routeData?.Values.ContainsKey("action") == true)
            {
                action = routeData.Values["action"]?.ToString();
            }

            if (string.IsNullOrEmpty(controller) || string.IsNullOrEmpty(action))
            {
                // اگر اطلاعات کنترلر یا اکشن خالی بود، از ActionDescriptor استفاده می‌کنیم
                var endpoint = context.GetEndpoint();
                if (endpoint != null)
                {
                    var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                    if (actionDescriptor != null)
                    {
                        controller = actionDescriptor.ControllerName;
                        action = actionDescriptor.ActionName;
                    }
                }
            }

            // لاگ شروع درخواست
            Log.Information(
                "Step:START Request {Method} {Path} | Controller: {Controller} | Action: {Action} | Time: {Time}",
                method, path, controller, action, DateTime.UtcNow);

            try
            {
                await _next(context); // اجرای درخواست

                sw.Stop(); // زمان‌سنجی بعد از پایان درخواست
                // لاگ پایان درخواست
                Log.Information(
                    "Step:END Request {Method} {Path} | Controller: {Controller} | Action: {Action} | Duration: {Duration}ms",
                    method, path, controller, action, sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                sw.Stop(); // زمان‌سنجی در صورت بروز خطا
                // لاگ خطا
                Log.Error(
                    ex,
                    "Step:ERROR in Request {Method} {Path} | Controller: {Controller} | Action: {Action} | Duration: {Duration}ms",
                    method, path, controller, action, sw.ElapsedMilliseconds);

                throw; // باز انداختن خطا
            }
        }
    }
}
