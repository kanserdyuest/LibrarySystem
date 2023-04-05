using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LibrarySystemForWeb.Models
{
    [Table("ls_bookinfo")]
    public class BookModel
    {
        [Key]
        [Column("bi_id")]
        public int BiId { get; set; }
        
        [Column("bi_name")]
        public string BiName { get; set; }

        [Column("bt_id")]
        public int BtId { get; set; }

        [Column("bi_press")]
        public string BiPress { get; set; }

        [Column("bi_isbn")]
        public string BiIsbn { get; set; }

        [Column("bi_author")]
        public string BiAuthor { get; set; }

        [Column("bi_location")]
        public string BiLocation { get; set; }

        [Column("bi_price")]
        public double BiPrice { get; set; }

        [Column("bi_pages")]
        public int BiPages { get; set; }

        [Column("bi_addtime")]
        public string BiAddtime { get; set; }

        [Column("bi_num")]
        public int BiNum { get; set; }

        [Column("bi_cover_picture")]
        public string BiCoverPicture { get; set; }

        [Column("bi_borrow_num")]
        public int BiBorrowNum { get; set; }
        [Column("deleted")]
        public int Deleted { get; set; }

    }
}