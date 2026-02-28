using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Security;

namespace Home_Expert.Security
{
    public sealed class SecureIdMiddleware : IMiddleware
    {
        private readonly ISecureIdService _secureIdService;
        private readonly SecureIdOptions _options;

        public SecureIdMiddleware(
            ISecureIdService secureIdService,
            IOptions<SecureIdOptions> options)
        {
            _secureIdService = secureIdService;
            _options = options.Value;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // 0️⃣ إذا كان id موجود (روابط قديمة)
            if (context.Request.RouteValues.ContainsKey(_options.IdQueryKey))
            {
                await next(context);
                return;
            }

            // 1️ Route Data
            var routeData = context.GetRouteData();
            if (routeData?.Values == null)
            {
                await next(context);
                return;
            }

            var controller = routeData.Values["controller"]?.ToString();
            var action = routeData.Values["action"]?.ToString();

            if (string.IsNullOrWhiteSpace(controller) ||
                string.IsNullOrWhiteSpace(action))
            {
                await next(context);
                return;
            }

            var scope = $"{controller}:{action}";

            // 2️ User binding (اختياري)
            string? userId = null;
            if (_options.BindToUser)
            {
                userId = context.User?
                    .FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            }

            // 3️ استخراج RID (GET أو POST)
            string? token = null;

            if (context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase))
            {
                context.Request.Query.TryGetValue(_options.TokenQueryKey, out var q);
                token = string.IsNullOrWhiteSpace(q) ? null : q.ToString();
            }
            else if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) &&
                     context.Request.HasFormContentType)
            {
                var form = context.Request.Form;
                token = form.TryGetValue(_options.TokenQueryKey, out var f) &&
                        !string.IsNullOrWhiteSpace(f)
                        ? f.ToString()
                        : null;
            }

            // لا يوجد RID → كمل
            if (string.IsNullOrWhiteSpace(token))
            {
                await next(context);
                return;
            }

            // 4️ فك RID
            int id;
            try
            {
                id = _secureIdService.Unprotect(token, scope, userId, out _);
            }
            catch (SecurityException)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            // 5 حقن ID (هذا يكفي 100%)
            context.Request.RouteValues[_options.IdQueryKey] = id;

            // 6 أكمل إلى MVC
            await next(context);
        }
    }
}
