using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LibrarySystemForWeb.Models;
using LibrarySystemForWeb.Tools;
using System;
using MySql.Data.MySqlClient;

namespace LibrarySystemForWeb.Controllers
{
    public class TypeController : Controller
    {
        private LibrarySystemContext db = new LibrarySystemContext();

        public ActionResult GetListInfo(int page, int size, string BtName)
        {
            string sqlInit = "select bt_id BtId,bt_name BtName from LibrarySystem03.ls_booktype  ";
            string sql;
            List<MySqlParameter> parameters = new List<MySqlParameter>();
            if (BtName != null && BtName != "")
            {
                sql = sqlInit + "where bt_name like @BtName ORDER BY bt_id DESC limit @page,@size;";
                var likeBtName = $"%{BtName}%";
                parameters.Add(new MySqlParameter("@BtName", likeBtName));
                parameters.Add(new MySqlParameter("@page", (page - 1) * size));
                parameters.Add(new MySqlParameter("@size", size));
            }
            else
            {
                sql = sqlInit + " ORDER BY bt_id DESC limit @page,@size;";
                parameters.Add(new MySqlParameter("@page", (page - 1) * size));
                parameters.Add(new MySqlParameter("@size", size));
            }
            try
            {
                MySqlParameter[] arr = parameters.ToArray();
                List<TypeModel> typeList = db.TypeModels.SqlQuery(sql,arr).ToList();
                return Json(CommonResult.Success(typeList, db.TypeModels.ToList().Count), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed(), JsonRequestBehavior.AllowGet);
            }
        }

        // 获取所有类别信息用于下拉框
        public ActionResult GetAllInfo()
        {
            try
            {
                return Json(CommonResult.Success(db.TypeModels.ToList()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed(), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult SaveInfo(TypeModel typeModel)
        {
            try
            {
                db.TypeModels.Add(typeModel);
                db.SaveChanges();
                return Json(CommonResult.Success("添加成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed(), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult UpdateInfo(TypeModel typeModel)
        {
            try
            {
                db.Entry(typeModel).State = EntityState.Modified;
                db.SaveChanges();
                return Json(CommonResult.Success("修改成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed(), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult RemoveInfo(int? BtId)
        {

            try
            {
                db.TypeModels.Remove(db.TypeModels.Find(BtId));
                db.SaveChanges();
                return Json(CommonResult.Success("删除成功"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}