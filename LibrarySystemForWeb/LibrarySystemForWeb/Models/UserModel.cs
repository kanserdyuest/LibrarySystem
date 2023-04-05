using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrarySystemForWeb.Models
{
    [Table("ls_user")]
    public class UserModel
    {
        [Key]
        [Column("u_id")]
        public int UId { get; set; }
        [Column("u_name")]
        public string UName { get; set; }
        [Column("u_pwd")]
        public string UPwd { get; set; }
        [Column("u_gender")]
        public int UGender { get; set; }
        [Column("u_birthday")]
        public string UBirthday { get; set; }
        [Column("u_phone")]
        public string UPhone { get; set; }
        [Column("u_identity")]
        public int UIdentity { get; set; }
    }
}