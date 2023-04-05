using LibrarySystemForWeb.Models;
using System.Web;
using System.Web.Mvc;

namespace LibrarySystemForWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoginCheckFilterAttribute() { IsChecked = true});
        }
    }
}
