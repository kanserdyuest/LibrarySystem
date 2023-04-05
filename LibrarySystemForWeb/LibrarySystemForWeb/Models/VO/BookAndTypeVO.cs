using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemForWeb.Models.VO
{
    public class BookAndTypeVO
    {
        [Key]
        public int BiId { get; set; }

        public string BiName { get; set; }

        public int BtId { get; set; }

        public string BtName { get; set; }

        public string BiPress { get; set; }

        public string BiIsbn { get; set; }

        public string BiAuthor { get; set; }

        public string BiLocation { get; set; }

        public double BiPrice { get; set; }

        public int BiPages { get; set; }

        public string BiAddtime { get; set; }

        public string BiNum { get; set; }

        public string BiCoverPicture { get; set; }

        public int BiBorrowNum { get; set; }

        public int Deleted { get; set; }
    }
}