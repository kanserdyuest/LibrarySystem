using LibrarySystemForWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace LibrarySystemForWeb.Tools
{
    public class BorrowBookResult
    {
        //调用数据库封装类
        private LibrarySystemContext db = new LibrarySystemContext();
        //查询书籍ibsn是否存在 是否与书籍匹配
        public List<BookModel> selectIbsn(string Isbn)
        {
            List<BookModel> books = db.BookModels.Where(x => x.BiIsbn == Isbn).ToList();
            return books;
        }
        //查询读者手机号是否存在
        public List<ReaderModel> selectPhone(string Phone)
        {
            List<ReaderModel> readerPhone = db.ReaderModels.Where(x => x.RPhone == Phone).ToList();
            return readerPhone;
        }
        //借阅时间要比归还时间早
        public bool CompareTime(DateTime a, DateTime b)
        {
            if (DateTime.Compare(a, b) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public BorrowModel NewBorrow(DateTime BbLendtime, DateTime BbReturntime, DateTime? BbRealReturntime, int BbIsreborrow, int BbReborrowDays, int BbIsrenewBook, string BiIsbn, string RPhone)
        {
            BorrowModel borrow = new BorrowModel();
            List<BookModel> book = selectIbsn(BiIsbn);
            List<ReaderModel> readers = selectPhone(RPhone);
            //书籍id
            borrow.BiId = book[0].BiId;
            //读者id
            borrow.RId = readers[0].RId;
            //借出时间
            borrow.BbLendtime = BbLendtime;
            //归还时间
            borrow.BbReturntime = BbReturntime;
            //实际归还时间
            borrow.BbRealReturntime = BbRealReturntime;
            //是否续借: 1:否  2:是
            borrow.BbIsreborrow = BbIsreborrow;
            //续借天数
            borrow.BbReborrowDays = BbReborrowDays;
            //是否归还书籍: 1：是 2：否
            borrow.BbIsrenewBook = BbIsrenewBook;
            return borrow;
        }

        public BorrowNewList timeShift(BorrowAndBook borrow)
        {
            BorrowNewList newBorrow = new BorrowNewList();
            //借阅id
            newBorrow.BbId = borrow.BbId;
            //书籍名
            newBorrow.BiName = borrow.BiName;
            //读者名
            newBorrow.RName = borrow.RName;
            //借出时间
            newBorrow.BbLendtime = borrow.BbLendtime.ToString("G");
            //归还时间
            newBorrow.BbReturntime = borrow.BbReturntime.ToString("G");
            //实际归还时间
            newBorrow.BbRealReturntime = borrow.BbRealReturntime.ToString();
            //是否续借: 1:否  2:是
            newBorrow.BbIsreborrow = borrow.BbIsreborrow;
            //续借天数
            newBorrow.BbReborrowDays = borrow.BbReborrowDays;
            //书籍isbn
            newBorrow.BiIsbn = borrow.BiIsbn;
            //书记数量
            newBorrow.BiNum = borrow.BiNum;
            //读者电话
            newBorrow.RPhone = borrow.RPhone;
            //是否归还书籍: 1：是 2：否
            newBorrow.BbIsrenewBook = borrow.BbIsrenewBook;
            return newBorrow;
        }
    }
}