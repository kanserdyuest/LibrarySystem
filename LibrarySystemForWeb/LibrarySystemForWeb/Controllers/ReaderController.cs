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
    public class ReaderController : Controller
    {
        private LibrarySystemContext db = new LibrarySystemContext();

        public ActionResult GetListInfo(int page, int size, string RName)
        {
            string sqlInit =
                "select r_id  RId, r_name RName, r_gender RGender, r_birthday RBirthday, r_phone RPhone, r_email REmail,deleted Deleted from LibrarySystem03.ls_readers WHERE deleted = 0 ";
            string sql;
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (RName != null && RName != "")
            {
                sql = sqlInit + "AND r_name LIKE @RName ORDER BY r_id DESC limit @page, @size;";
                var likeRName = $"%{RName}%";
                parameters.Add(new MySqlParameter("@RName", likeRName));
                parameters.Add(new MySqlParameter("@page", (page - 1) * size));
                parameters.Add(new MySqlParameter("@size", size));
            }
            else
            {
                sql = sqlInit + " ORDER BY r_id DESC limit @page, @size;";
                parameters.Add(new MySqlParameter("@page", (page - 1) * size));
                parameters.Add(new MySqlParameter("@size", size));
            }
            MySqlParameter[] arr = parameters.ToArray();

            try
            {

                List<ReaderModel> readerList = db.ReaderModels.SqlQuery(sql, arr).ToList();
                return Json(CommonResult.Success(readerList, db.ReaderModels.ToList().Count), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed(),JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveInfo(ReaderModel readerModel)
        {
            try
            {
                db.ReaderModels.Add(readerModel);
                db.SaveChanges();
                return Json(CommonResult.Success("添加成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }
        }

        [HttpPost]
        public ActionResult UpdateInfo(ReaderModel readerModel)
        {

            try
            {
                db.Entry(readerModel).State = EntityState.Modified;
                db.SaveChanges();
                return Json(CommonResult.Success("修改成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }
        }

        [HttpPost]
        public ActionResult RemoveInfo(int? RId)
        {
            try
            {
                if (db.BorrowModels.Where(x => x.RId == RId).ToList().Count != 0)
                {
                    return Json(CommonResult.Failed("该读者禁止删除，请联系系统管理员"));
                }
                ReaderModel reader = db.ReaderModels.Find(RId);
                reader.Deleted = 1;
                db.Entry(reader).State = EntityState.Modified;
                db.SaveChanges();
                return Json(CommonResult.Success("删除成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }
        }
    }
}