using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Projeto.Alfa12.Views.Modulos.Creates
{
    public static class ModNavPages
    {
        public static string ActivePageKey => "ActivePage";

        public static string Create => "Create";
        public static string Create2 => "Create2";
        public static string Create3 => "Create3";
        public static string Create4 => "Create4";
        public static string Create5 => "Create5";

        public static string CreateNavClass(ViewContext viewContext) => PageNavClass(viewContext, Create);
        public static string Create2NavClass(ViewContext viewContext) => PageNavClass(viewContext, Create2);
        public static string Create3NavClass(ViewContext viewContext) => PageNavClass(viewContext, Create3);
        public static string Create4NavClass(ViewContext viewContext) => PageNavClass(viewContext, Create4);
        public static string Create5NavClass(ViewContext viewContext) => PageNavClass(viewContext, Create5);

        public static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage) => viewData[ActivePageKey] = activePage;
    }
}
