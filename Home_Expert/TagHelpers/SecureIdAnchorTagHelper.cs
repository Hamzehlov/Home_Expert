using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
 
namespace Home_Expert.TagHelpers
{
    //TagHelper لتوليد روابط rid تلقائيًا

    //بدل ما تكتب Extension أو توليد يدوي، نعطي TagHelper يشتغل مع<a> ويفهم asp-route-id.

    //6 الخطوة السادسة: انشاء TagHelper لتوليد روابط rid تلقائيًا بدل ما تكتب Extension أو توليد يدوي


    using Home_Expert.Security;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using Microsoft.AspNetCore.Razor.TagHelpers;
    using Microsoft.Extensions.Options;

    //[HtmlTargetElement("a", Attributes = "asp-action,asp-controller,asp-route-id")]
    [HtmlTargetElement("a", Attributes = "asp-action,asp-controller")]

    public sealed class SecureIdAnchorTagHelper : TagHelper
    {
        private readonly ISecureIdService _svc;
        private readonly SecureIdOptions _opt;
        private readonly IUrlHelperFactory _urlFactory;

        public SecureIdAnchorTagHelper(
            ISecureIdService svc,
            IOptions<SecureIdOptions> opt,
            IUrlHelperFactory urlFactory)
        {
            _svc = svc;
            _opt = opt.Value;
            _urlFactory = urlFactory;
        }

        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        // اختياري: override TTL من الـ View
        [HtmlAttributeName("asp-secure-ttl-minutes")]
        public int? TtlMinutes { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var idAttr = context.AllAttributes
    .FirstOrDefault(a =>
        a.Name.StartsWith("asp-route-", StringComparison.OrdinalIgnoreCase)
        && a.Value != null
        && int.TryParse(a.Value.ToString(), out _));

            if (idAttr == null)
                return;

            int id = int.Parse(idAttr.Value!.ToString()!);


            //// اقرأ route values من taghelper context
            //if (!context.AllAttributes.TryGetAttribute("asp-route-id", out var idAttr))
            //    return;

            //if (idAttr.Value is null || !int.TryParse(idAttr.Value.ToString(), out var id))
            //    return;

            var action = context.AllAttributes["asp-action"]?.Value?.ToString() ?? "";
            var controller = context.AllAttributes["asp-controller"]?.Value?.ToString() ?? "";
            var scope = $"{controller}:{action}";

            string? userId = null;
            if (_opt.BindToUser)
            {
                var user = ViewContext.HttpContext.User;
                userId = user?.FindFirst("sub")?.Value
                      ?? user?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            }

            var ttl = TtlMinutes.HasValue ? TimeSpan.FromMinutes(TtlMinutes.Value) : _opt.DefaultTtl;
            var token = _svc.Protect(id, scope, userId, ttl);

            // أنشئ URL بدون id، واستبدله بـ rid
            var url = _urlFactory.GetUrlHelper(ViewContext);
            var href = url.Action(action, controller, new { rid = token });

            // ضع href النهائي
            output.Attributes.SetAttribute("href", href);

            // احذف id حتى لا ينكشف
            output.Attributes.RemoveAll("asp-route-id");
        }
    }

}
