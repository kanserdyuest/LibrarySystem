using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibrarySystemForWeb.Models.VO
{
    public class BorrowNumInfoVO
    {
        public string BiName { get; set; }
        public int BoCount { get; set; }
        public DateTime BbLendtime { get; set; }
        public string BiCoverPicture { get; set; }
    }
}