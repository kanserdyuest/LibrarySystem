using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemForWeb.Models
{
    [Table("ls_readers")]
    public class ReaderModel
    {
        [Key]
        [Column("r_id")]
        public int RId { get; set; }
        
        [Column("r_name")]
        public string RName { get; set; }
        
        [Column("r_gender")]
        public int RGender { get; set; }
        
        [Column("r_birthday")]
        public string RBirthday { get; set; }
        
        [Column("r_phone")]
        public string  RPhone { get; set; }
        
        [Column("r_email")]
        public string REmail { get; set; }
        [Column("deleted")]
        public int Deleted { get; set; }
    }
}