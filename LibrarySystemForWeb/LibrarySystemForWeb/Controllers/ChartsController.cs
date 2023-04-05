using LibrarySystemForWeb.Models;
using LibrarySystemForWeb.Models.VO;
using LibrarySystemForWeb.Tools;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LibrarySystemForWeb.Controllers
{
    public class ChartsController : Controller
    {
        private LibrarySystemContext db = new LibrarySystemContext();
        public ActionResult GetBookTypeChart()
        {
            List<BookTypeChartVO> chartData =  db.Database.SqlQuery<BookTypeChartVO>("select bt.bt_name BtName,count(*) as Sum from ls_bookinfo bi left join ls_booktype bt on bi.bt_id = bt.bt_id group by bi.bt_id;").ToList();
            return Json(CommonResult.Success(chartData),JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBookNumChart()
        {
            List<BookModel> bl = db.BookModels.ToList();
            List<BookInfoVO> bo = new List<BookInfoVO>();
            foreach(BookModel b in bl)
            {
                bo.Add(new BookInfoVO(b.BiName,b.BiNum));
            }
            return Json(CommonResult.Success(bo),JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBorrowTopChart()
        {
            string sql = "select  bk.bi_name BiName,count(*) as BoCount,bo.bb_lendtime BbLendtime,bk.bi_cover_picture BiCoverPicture from ls_borrow bo left join ls_bookinfo bk on bo.bi_id = bk.bi_id where DATE_SUB(CURDATE(), INTERVAL 30 DAY) <= date(bo.bb_lendtime) group by bo.bi_id order by BoCount DESC limit 10; ";

            List<BorrowNumInfoVO> topList = db.Database.SqlQuery<BorrowNumInfoVO>(sql).ToList();

            return Json(CommonResult.Success(topList),JsonRequestBehavior.AllowGet);
        }


    }
}