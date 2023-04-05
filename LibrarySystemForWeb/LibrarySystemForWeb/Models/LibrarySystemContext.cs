using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LibrarySystemForWeb.Models
{
    public class LibrarySystemContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public LibrarySystemContext() : base("name=MyContext")
        {

        }
        public System.Data.Entity.DbSet<LibrarySystemForWeb.Models.BookModel> BookModels { get; set; }

        public System.Data.Entity.DbSet<LibrarySystemForWeb.Models.UserModel> UserModels { get; set; }
        public System.Data.Entity.DbSet<LibrarySystemForWeb.Models.TypeModel> TypeModels { get; set; }

        public System.Data.Entity.DbSet<LibrarySystemForWeb.Models.ReaderModel> ReaderModels { get; set; }

        public System.Data.Entity.DbSet<LibrarySystemForWeb.Models.BorrowModel> BorrowModels { get; set; }
        public System.Data.Entity.DbSet<LibrarySystemForWeb.Models.BorrowAndBook> BorrowAndBooks { get; set; }

    }
}
