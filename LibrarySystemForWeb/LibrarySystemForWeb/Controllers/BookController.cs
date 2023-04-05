using System.Linq;
using System.Web.Mvc;
using LibrarySystemForWeb.Models;
using System.Data.Entity;
using LibrarySystemForWeb.Tools;
using LibrarySystemForWeb.Models.VO;
using System.IO;
using System;
using System.Web;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace LibrarySystemForWeb.Controllers
{
    public class BookController : Controller
    {
        private LibrarySystemContext db = new LibrarySystemContext();

        // 分页查询 图书模糊检索
        // 分页查询 图书模糊检索
        public ActionResult GetList(int page, int size, string BiName, int? BtId, string BiAuthor, string BiLocation, string BiPress, string BiIsbn,int ? Deleted)
        {
            // 定义初始化sql
            string sqlInit = "select bi.bi_id BiId,bi.bi_name BiName,bi.bt_id BtId,bt.bt_name BtName,bi.bi_press BiPress,bi.bi_isbn BiIsbn,bi.bi_author BiAuthor,bi.bi_location BiLocation,bi.bi_price BiPrice,bi.bi_pages BiPages,bi.bi_addtime BiAddtime,bi.bi_num BiNum,bi.bi_cover_picture BiCoverPicture,bi.deleted Deleted,bi.bi_borrow_num BiBorrowNum from LibrarySystem03.ls_bookinfo bi LEFT JOIN LibrarySystem03.ls_booktype bt on bi.bt_id = bt.bt_id where 1 = 1 ";

            List<MySqlParameter> parameters = new List<MySqlParameter>();
            string sql = sqlInit;
            // 判断有无需要模糊拼接
            if ((BiName != null && BiName != "") || (BtId != null && BtId != 0) || (BiAuthor != null && BiAuthor != "") || (BiLocation != null && BiLocation != "") || (BiPress != null && BiPress != "") || (BiIsbn != null && BiIsbn != "") || (Deleted != null))
            {
                // 书名字段
                if (BiName != null && BiName != "")
                {
                    sql = sql + " AND bi.bi_name LIKE @BiName ";
                    var likeBiName = $"%{BiName}%";
                    parameters.Add(new MySqlParameter("@BiName", likeBiName));
                }
                // 类别下拉框加载
                if (BtId != null && BtId != 0)
                {
                    sql = sql + " AND bi.bt_id =@BtId";
                    parameters.Add(new MySqlParameter("@BtId", BtId));
                }
                // 作者检索
                if (BiAuthor != null && BiAuthor != "")
                {
                    sql = sql + " AND bi.bi_author LIKE @BiAuthor ";
                    var likeBiAuthor = $"%{BiAuthor}%";
                    parameters.Add(new MySqlParameter("@BiAuthor", likeBiAuthor));
                }
                // 位置检索拼接
                if (BiLocation != null && BiLocation != "")
                {
                    sql = sql + " AND bi.bi_location LIKE @BiLocation ";
                    var likeBiLocation = $"%{BiLocation}%";
                    parameters.Add(new MySqlParameter("@BiLocation", likeBiLocation));
                }
                // 出版社拼接
                if (BiPress != null && BiPress != "")
                {
                    sql = sql + " AND bi.bi_press LIKE @BiPress ";
                    var likeBiPress = $"%{BiPress}%";
                    parameters.Add(new MySqlParameter("@BiPress", likeBiPress));
                }
                // ISBN数据sql拼接
                if (BiIsbn != null && BiIsbn != "")
                {
                    sql = sql + " AND bi.bi_isbn LIKE @BiIsbn ";
                    var likeBiIsbn = $"%{BiIsbn}%";
                    parameters.Add(new MySqlParameter("@BiIsbn", likeBiIsbn));
                }
                // 状态
                if (Deleted != null)
                {
                    sql = sql + " AND bi.deleted = @Deleted ";
                    parameters.Add(new MySqlParameter("@Deleted", Deleted));
                }
                // 分页sql拼接
                string sqlPage = sql + " ORDER BY bi.deleted ASC,bi.bi_id DESC LIMIT @page,@size;";
                parameters.Add(new MySqlParameter("@page", (page - 1) * size));
                parameters.Add(new MySqlParameter("@size", size));
                MySqlParameter[] parames = parameters.ToArray();
                try
                {
                    // 返回JSON数据
                    return Json(CommonResult.Success(db.Database.SqlQuery<BookAndTypeVO>(@sqlPage, parames).ToList(),
              db.Database.SqlQuery<BookAndTypeVO>(sql, parames).ToList().Count),
            JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(CommonResult.Failed(), JsonRequestBehavior.AllowGet);
                }
            }
            // 如果没有检索条件，则默认返回分页数据
            else
            {
                sql = sql + " ORDER BY bi.deleted ASC,bi.bi_id DESC LIMIT @page,@size;";
                parameters.Add(new MySqlParameter("@page", (page - 1) * size));
                parameters.Add(new MySqlParameter("@size", size));
                MySqlParameter[] parames = parameters.ToArray();
                try
                {
                    return Json(
                      CommonResult.Success(db.Database.SqlQuery<BookAndTypeVO>(sql, parames).ToList(),
                        db.BookModels.ToList().Count),
                      JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(CommonResult.Failed(), JsonRequestBehavior.AllowGet);
                }

            }

        }
        // 图书入库
        [HttpPost]
        public ActionResult SaveInfo(BookModel bookModel)
        {
            // 图书入库时间，获取当前系统时间
            bookModel.BiAddtime = DateTime.Today.ToString();
            try
            {
                // 调用ef6执行添加操作
                db.BookModels.Add(bookModel);
                db.SaveChanges();
                return Json(CommonResult.Success("新增成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }

        }

        // 图书信息修改
        [HttpPost]
        public ActionResult UpdateInfo(BookModel bookModel)
        {
            try
            {
                // 调用ef6执行修改操作
                db.Entry(bookModel).State = EntityState.Modified;
                db.SaveChanges();
                return Json(CommonResult.Success("修改成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }

        }

        // 图书删除操作
        [HttpPost]
        public ActionResult RemoveInfo(int? BiId)
        {
            try
            {
                // 调用ef6执行操作
                BookModel book = db.BookModels.Find(BiId);
                book.Deleted = 1;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return Json(CommonResult.Success("下架成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }
        }

        // 图书删除操作
        [HttpPost]
        public ActionResult UpInfo(int? BiId)
        {
            try
            {
                // 调用ef6执行操作
                BookModel book = db.BookModels.Find(BiId);
                book.Deleted = 0;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return Json(CommonResult.Success("上架成功"));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }
        }

        // 图片上传操作
        [HttpPost]
        public ActionResult UploadPic(HttpPostedFileBase file)
        {
            // 获取接收到的文件名
            string fileName = DateTime.Now.Ticks + file.FileName;
            // 路径定义
            string path = "/upload/" + DateTime.Now.ToString("d") + "/";
            // 将路径映射到服务器目录下
            string filePath = Server.MapPath(string.Format("~" + path));
            // 判断该路径是否存在，如不存在则创建
            if (!Directory.Exists(filePath))
            {
                // 创建路径
                Directory.CreateDirectory(filePath);
            }
            try
            {
                // 将文件保存到指定路径
                file.SaveAs(Path.Combine(filePath, fileName));
                // 拼接文件访问地址 返回到前端
                string pic_url = "http://" + Request.Url.Host + ":" + Request.Url.Port + path + fileName;
                return Json(CommonResult.Success(pic_url, 0));
            }
            catch (Exception)
            {
                return Json(CommonResult.Failed());
            }

        }
    }
}