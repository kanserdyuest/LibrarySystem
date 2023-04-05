using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LibrarySystemForWeb.Models;
using System;
using LibrarySystemForWeb.Tools;
using System.Data.Entity;
using MySql.Data.MySqlClient;

namespace LibrarySystem.Controllers
{
    public class BorrowController : Controller
    {
        //调用数据库封装类
        private LibrarySystemContext db = new LibrarySystemContext();
        //模糊查询  用于页面显示和查询
        public ActionResult GetBorrowList(int page, int size, string RName)
        {
            //全查询sql
            //如果输入读者名字进行sql拼接否则直接页面查询
            List<MySqlParameter> listSql = new List<MySqlParameter>();
            string sqlInit = " select ls_readers.r_name as RName,ls_readers.r_phone as RPhone,ls_bookinfo.bi_name as BiName,ls_bookinfo.bi_isbn as BiIsbn,ls_bookinfo.bi_num as BiNum,ls_borrow.bb_id as BbId,ls_borrow.bb_lendtime as BbLendtime ,ls_borrow.bb_returntime as BbReturntime,ls_borrow.bb_real_returntime as BbRealReturntime,ls_borrow.bb_isreborrow as BbIsreborrow,ls_borrow.bb_reborrow_days as BbReborrowDays,ls_borrow.bb_isrenewbook as  BbIsrenewBook from ls_borrow inner join ls_readers on ls_borrow.r_id = ls_readers.r_id inner join ls_bookinfo on ls_borrow.bi_id = ls_bookinfo.bi_id  ";
            string sql;
            int sqlPage = (page - 1) * size;
            int likeCount = 0;
            //如果输入读者名字进行sql拼接否则直接页面查询
            if (RName != null && RName != "")
            {
                var likeRName = $"%{RName}%";
                sql = sqlInit + " where ls_readers.r_name like @RName order by ls_borrow.bb_id desc ";
                likeCount = db.BorrowAndBooks.SqlQuery(sql, new MySqlParameter("@RName", likeRName)).Count();
                sql = sql + "limit @page,@size;";
                listSql.Add(new MySqlParameter("@RName", likeRName));
            }
            else
            {
                sql = sqlInit + " order by ls_borrow.bb_id desc  limit @page,@size;";
            }
            listSql.Add(new MySqlParameter("@page", sqlPage));
            listSql.Add(new MySqlParameter("@size", size));
            //返回借阅数据存入List集合
            MySqlParameter[] a = listSql.ToArray();
            try
            {
                List<BorrowAndBook> borrowsList = db.BorrowAndBooks.SqlQuery(sql, a).ToList();
                //修改节约数据时间参数的新集合
                List<BorrowNewList> borrowsNewList = new List<BorrowNewList>();
                //便利旧集合数据存入新集合
                foreach (BorrowAndBook borrow in borrowsList)
                {
                    BorrowNewList newBorrow = new BorrowBookResult().timeShift(borrow);
                    borrowsNewList.Add(newBorrow);
                }
                if (RName != null && RName != "")
                {
                    return Json(CommonResult.Success(borrowsNewList, likeCount), JsonRequestBehavior.AllowGet);
                }
                //返回结果集
                return Json(CommonResult.Success(borrowsNewList, db.BorrowModels.ToList().Count), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(CommonResult.Failed("出错了，刷新试试"));
            }
        }
        //ISBN下拉框
        [HttpPost]
        public ActionResult selectionIsbns(string Name)
        {
            try
            {
                List<BookModel> IsbnList = db.BookModels.Where(x => x.BiName == Name).ToList();
                if (IsbnList.Count == 0 || IsbnList == null)
                {
                    return Json(CommonResult.Failed("查询失败,书籍不存在"));
                }
                List<string> Isbns = new List<string>();
                foreach (BookModel book in IsbnList)
                {
                    if (book.Deleted == 0)
                    {
                        Isbns.Add(book.BiIsbn);
                    }
                }
                if (IsbnList.Count == Isbns.Count)
                {
                    return Json(CommonResult.Success(Isbns));
                }
                return Json(CommonResult.Success(Isbns, "这个名字的书有的被下架了"));
            }
            catch (MySqlException)
            {
                return Json(CommonResult.Failed("查询失败"));
            }
        }

        //Phone下拉框
        [HttpPost]
        public ActionResult selectionPhones(string Name)
        {
            List<ReaderModel> PhoneList = db.ReaderModels.Where(x => x.RName == Name).ToList();
            if (PhoneList.Count == 0 || PhoneList == null)
            {
                return Json(CommonResult.Failed("查询失败,读者不存在,请先添加读者"));
            }

            List<string> Phones = new List<string>();
            foreach (ReaderModel reader in PhoneList)
            {
                if (reader.Deleted == 0)
                {
                    Phones.Add(reader.RPhone);
                }
            }
            if (PhoneList.Count == Phones.Count)
            {
                return Json(CommonResult.Success(Phones));
            }
            return Json(CommonResult.Success(Phones, "有读者不存在请注意"));
        }
        //新增借阅
        [HttpPost]
        public ActionResult AddBorrow(BorrowAndBook borrowBook)
        {
            try
            {
                BorrowBookResult result = new BorrowBookResult();
                BookModel book = result.selectIbsn(borrowBook.BiIsbn)[0];
                if (result.CompareTime(DateTime.Now, borrowBook.BbReturntime))
                {
                    return Json(CommonResult.Failed("归还时间不对哦，请重新输入"));
                };
                //书籍数量为0
                if (book.BiBorrowNum == book.BiNum)
                {
                    return Json(CommonResult.Failed("这本书没了哦,可以看看其他书"));
                }
                //存在则将数据放入借阅实体类进行添加
                BorrowModel borrow = result.NewBorrow(DateTime.Now, borrowBook.BbReturntime, null, 1, 0, 2, borrowBook.BiIsbn, borrowBook.RPhone);
                //添加借阅
                db.BorrowModels.Add(borrow);
                //该图书借阅数量+1
                book.BiBorrowNum = book.BiBorrowNum + 1;
                db.Entry(book).State = EntityState.Modified;
                //刷新数据库
                db.SaveChanges();
                //返回json数据-->成功
                return Json(CommonResult.Success("新增成功"));
            }
            catch
            {
                return Json(CommonResult.Failed("新增失败"));
            }
        }
        //罚款
        public ActionResult Fine(BorrowModel borrow, DateTime BbRealReturntime)
        {
            try
            {
                //如果实际归还时间==归还时间 罚款为0
                if (borrow.BbReturntime == BbRealReturntime || borrow.BbReturntime > BbRealReturntime)
                {
                    return Json(CommonResult.Success(0, "按时归还"));
                }
                else //！= 超时
                {
                    TimeSpan ts = BbRealReturntime - borrow.BbReturntime;
                    string s = ((ts.Days * 24) + ts.Minutes).ToString();
                    int fine = int.Parse(s);
                    return Json(CommonResult.Success(Math.Round(fine * 0.1, 2), "该读者超时了" + s + "小时"));
                }
            }
            catch
            {
                return Json(CommonResult.Failed("出错了，刷新试试"));
            }

        }
        //还书
        public ActionResult UpdateInfo(BorrowAndBook borrowBook)
        {
            try
            {
                string NewTime = borrowBook.BbRealReturntime.ToString();
                if (DateTime.Compare(borrowBook.BbLendtime, DateTime.Now) > 0)
                {
                    return Json(CommonResult.Failed("归还时间或实际归还时间不对哦，请重新输入"));
                };
                //拿到值赋值给实体类
                //修改数据
                BorrowModel borrow = new BorrowModel();
                borrow.BbId = borrowBook.BbId;
                var newborrow = db.BorrowModels.Attach(borrow);
                newborrow.BbRealReturntime = DateTime.Now;
                newborrow.BbIsrenewBook = 1;
                db.SaveChanges();
                //该图书借阅数量-1
                BookModel book = db.BookModels.Where(x => x.BiIsbn == borrowBook.BiIsbn).ToList()[0];
                if (book.BiBorrowNum == 0)
                {
                    return Json(CommonResult.Failed("出错了，刷新试试"));
                }
                book.BiBorrowNum = book.BiBorrowNum - 1;
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                //返回json数据
                return Json(CommonResult.Success("还书成功"));
            }
            catch
            {
                return Json(CommonResult.Failed("还书失败"));
            }
        }
        //续借
        public ActionResult Renewborrow(BorrowAndBook borrowBook)
        {
            try
            {
                int day = borrowBook.BbReborrowDays;
                if (day == 0)
                {
                    return Json(CommonResult.Failed("续借天数不能为0"));
                }
                if (day > 30)
                {
                    return Json(CommonResult.Failed("续借天数不能为超过30天"));
                }
                DateTime time = borrowBook.BbReturntime.AddDays(day);
                if (borrowBook.BbReturntime <= DateTime.Now)
                {
                    return Json(CommonResult.Failed("超时了不可续借请先还书"));
                }
                //创建借阅实体类接收数据
                //修改数据
                BorrowModel borrow = new BorrowModel();
                borrow.BbId = borrowBook.BbId;
                var newborrow = db.BorrowModels.Attach(borrow);
                newborrow.BbReturntime = time;
                newborrow.BbIsreborrow = 2;
                newborrow.BbReborrowDays = day;
                db.SaveChanges();
                //返回json数据
                return Json(CommonResult.Success("续借成功"));
            }
            catch
            {
                return Json(CommonResult.Failed("续借失败"));
            }
        }
    }
}