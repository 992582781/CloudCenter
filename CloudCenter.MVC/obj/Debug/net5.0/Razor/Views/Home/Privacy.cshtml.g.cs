#pragma checksum "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\Home\Privacy.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "cb57bc0b8f5d432b957648e429f579d720e45bc6"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Privacy), @"mvc.1.0.view", @"/Views/Home/Privacy.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\_ViewImports.cshtml"
using CloudCenter.MVC;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\_ViewImports.cshtml"
using CloudCenter.MVC.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\Home\Privacy.cshtml"
using Microsoft.AspNetCore.Authentication;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"cb57bc0b8f5d432b957648e429f579d720e45bc6", @"/Views/Home/Privacy.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c035e96906a91a832de8ba04288fe0a89fb5aab9", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Privacy : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
            WriteLiteral("\n<h2>Claims</h2>\n\n<dl>\n");
#nullable restore
#line 13 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\Home\Privacy.cshtml"
     foreach (var claim in User.Claims)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <dt>");
#nullable restore
#line 15 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\Home\Privacy.cshtml"
       Write(claim.Type);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dt>\n        <dd>");
#nullable restore
#line 16 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\Home\Privacy.cshtml"
       Write(claim.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\n");
#nullable restore
#line 17 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\Home\Privacy.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</dl>\n\n<h2>Properties</h2>\n\n<dl>\n");
#nullable restore
#line 23 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\Home\Privacy.cshtml"
     foreach (var prop in (await Context.AuthenticateAsync()).Properties.Items)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <dt>");
#nullable restore
#line 25 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\Home\Privacy.cshtml"
       Write(prop.Key);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dt>\n        <dd>");
#nullable restore
#line 26 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\Home\Privacy.cshtml"
       Write(prop.Value);

#line default
#line hidden
#nullable disable
            WriteLiteral("</dd>\n");
#nullable restore
#line 27 "D:\Study\CloudCenter\CloudCenter\CloudCenter.MVC\Views\Home\Privacy.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</dl>\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591