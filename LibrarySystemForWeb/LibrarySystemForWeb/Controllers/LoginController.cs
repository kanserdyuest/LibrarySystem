using LibrarySystemForWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LibrarySystemForWeb.Tools;

namespace LibrarySystemForWeb.Controllers
{
    public class LoginController : Controller
    {
        private LibrarySystemContext db = new LibrarySystemContext();

        // 表示该控制器不需要拦截
        [LoginCheckFilterAttribute(IsChecked = false)]
        [HttpPost]
        public ActionResult UserLogin(string UName,string UPwd)
        {
            try
            { 
                List<UserModel> ul = db.UserModels.SqlQuery("select u_id UId,u_name UName,u_pwd UPwd,u_gender UGender, u_birthday UBirthday,u_phone UPhone,u_identity UIdentity from LibrarySystem03.ls_user where u_name = {0} ",UName).ToList();
                string u = StringSecurity.MD5Encrypt(UPwd + UName);
                if (ul.Count > 0) //判断用户名是否正确
                {
                    
                    if (u == ul[0].UPwd) // 判断密码是否正确
                    {
                        Session["loginUser"] = ul[0]; //记录当前登录的用户 存入session
                        return Json(CommonResult.Success(ul[0])); // 将当前登录用户对象返回给前端
                    }
                    return Json(CommonResult.Failed("密码错误"));
                }
                else
                {
                    return Json(CommonResult.Failed("用户名或密码错误"));
                }
            }
            catch (Exception ex)
            {
                 return Json(CommonResult.Failed());
            }
        }

        // 登出方法 清除session
        public void UserLogout()
        {
            Session.Clear();
        }
    }
}