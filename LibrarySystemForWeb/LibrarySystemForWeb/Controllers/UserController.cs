using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LibrarySystemForWeb.Models;
using LibrarySystemForWeb.Tools;
using MySql.Data.MySqlClient;
using System;

namespace LibrarySystemForWeb.Controllers
{
    public class UserController : Controller
    {
        private LibrarySystemContext db = new LibrarySystemContext();

        private string BaseSql = "select u_id UId,u_name UName,u_pwd UPwd,u_gender UGender, u_birthday UBirthday,u_phone UPhone,u_identity UIdentity from LibrarySystem03.ls_user ";

        public ActionResult GetListInfo(int page, int size, string UName)
        {
            string sqlInit = BaseSql;
            string sql;
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (UName != null && UName != "")
            {
                sql = sqlInit + "WHERE u_name LIKE @UName ORDER BY u_id DESC LIMIT @page, @size";
                var likeUName = $"%{UName}%";
                parameters.Add(new MySqlParameter("@UName", likeUName));
                parameters.Add(new MySqlParameter("@page", (page - 1) * size));
                parameters.Add(new MySqlParameter("@size", size));
            }
            else
            {
                sql = sqlInit + " ORDER BY u_id DESC limit @page, @size";
                parameters.Add(new MySqlParameter("@page", (page - 1) * size));
                parameters.Add(new MySqlParameter("@size", size));
            }
            MySqlParameter[] arr = parameters.ToArray();
            try
            {
                List<UserModel> userList = db.UserModels.SqlQuery(sql,arr).ToList();
                return Json(CommonResult.Success(userList, db.UserModels.ToList().Count), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveInfo(UserModel userModel)
        {
            try
            {
                List<UserModel> l = db.UserModels.SqlQuery(BaseSql + " where u_name = {0};", userModel.UName).ToList();
                if (l.Count > 0)
                {
                    return Json(CommonResult.Failed("该用户已存在"));
                }
                userModel.UPwd = StringSecurity.MD5Encrypt(userModel.UPwd + userModel.UName);
                db.UserModels.Add(userModel);
                db.SaveChanges();
                return Json(CommonResult.Success("添加成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }

        }

        [HttpPost]
        public ActionResult UpdateInfo(UserModel userModel)
        {
            try
            {
                // userModel.UPwd = StringSecurity.MD5Encrypt(userModel.UPwd + userModel.UName);
                db.Entry(userModel).State = EntityState.Modified;
                db.SaveChanges();
                return Json(CommonResult.Success("修改成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }
        }

        [HttpPost]
        public ActionResult RemoveInfo(int? UId)
        {

            try
            {
                db.UserModels.Remove(db.UserModels.Find(UId));
                db.SaveChanges();
                return Json(CommonResult.Success("删除成功"));

            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }

        }

        [HttpPost]
        public ActionResult UpdateUIdentity(int UId, int UIdentity)
        {

            try
            {
                UserModel user = db.UserModels.Find(UId);
                user.UIdentity = UIdentity;
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Json(CommonResult.Success("身份修改成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }
        }

        [HttpPost]
        public ActionResult UpdatePwd(int UId, string UPwd)
        {
            try
            {
                UserModel user = db.UserModels.Find(UId);
                user.UPwd = StringSecurity.MD5Encrypt(UPwd + user.UName);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return Json(CommonResult.Success("密码修改成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }
        }

    }
}