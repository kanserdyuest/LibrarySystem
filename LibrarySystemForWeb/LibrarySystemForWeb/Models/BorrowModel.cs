using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LibrarySystemForWeb.Models
{
    [Table("ls_borrow")]
    public class BorrowModel
    {
        [Key]
        [Column("bb_id")]
        public int BbId  { get; set; }
        [Column("r_id")]
        public int  RId{ get; set; }
        [Column("bi_id")]
        public int  BiId{ get; set; }
        [Column("bb_lendtime")]
        public DateTime BbLendtime { get; set; }
        [Column("bb_returntime")]
        public DateTime BbReturntime { get; set; }
        [Column("bb_real_returntime")]
        public DateTime? BbRealReturntime { get; set; }
        [Column("bb_isreborrow")]
        public int BbIsreborrow { get; set; }
        [Column("bb_reborrow_days")]
        public int? BbReborrowDays { get; set; }
        [Column("bb_isrenewbook")]
        public int BbIsrenewBook { get; set; }
    }
}