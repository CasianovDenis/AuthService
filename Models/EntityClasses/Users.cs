using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Myproject.Models
{
    public enum UserStatus
    {
        Uknown = 0,
        Active = 1,
        Blocked = 2,
        Closed = 3
    }

    [Table("Users", Schema = "auth")]

    public class Users
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        public string Username { get; set; }
        //[Required]
        public string Password { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpire { get; set; }
        //[ForeignKey("Role_id")]
        //public Roles roles { get; set; }
    }
}