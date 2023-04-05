using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemForWeb.Models
{
    [Table("ls_booktype")]
    public class TypeModel
    {
        [Key]
        [Column("bt_id")]
        public int BtId { get; set; }
        [Column("bt_name")]
        public string BtName { get; set; }
    }
}