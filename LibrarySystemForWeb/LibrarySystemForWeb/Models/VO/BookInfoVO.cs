using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibrarySystemForWeb.Models.VO
{
    public class BookInfoVO
    {
        public string BiName { get; set; }
        public int BiNum { get; set; }

        public BookInfoVO(string BiName, int BiNum)
        {
            this.BiName = BiName;
            this.BiNum = BiNum;
        }

    }
}