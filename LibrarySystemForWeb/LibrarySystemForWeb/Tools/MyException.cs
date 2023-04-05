using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibrarySystemForWeb.Tools
{
    public class MyException : ApplicationException
    {
        public MyException(string message) : base(message)
        {
        }
    }
}