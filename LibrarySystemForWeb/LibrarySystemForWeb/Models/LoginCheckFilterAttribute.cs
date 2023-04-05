using System.Web.Mvc;

namespace LibrarySystemForWeb.Models
{
    public class LoginCheckFilterAttribute : ActionFilterAttribute 
      { 
          /// 是否校验，默认为true
        public bool IsChecked { get; set; }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
         {
            base.OnActionExecuted(filterContext);
             //校验用户是否已登录
            if (IsChecked)
            {
                 if (filterContext.HttpContext.Session["loginUser"] == null)
                 {
                     filterContext.HttpContext.Response.Redirect("/Login/UserLogin");
                 }
             }
        }
    }
}