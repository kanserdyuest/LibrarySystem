using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibrarySystemForWeb.Models
{
    public class BorrowAndBook
    {
        [Key]
        
        public int BbId { get; set; }
        
        public string RName { get; set; }
        public string BiName { get; set; }
        public DateTime BbLendtime { get; set; }
        public DateTime BbReturntime { get; set; }
        public DateTime? BbRealReturntime { get; set; }
        public int BbIsreborrow { get; set; }
        public int BbReborrowDays { get; set; }
        public string BiIsbn { get; set; }
        public string RPhone { get; set; }
        public int BiNum { get; set; }
        public int BbIsrenewBook { get; set; }
    }
}