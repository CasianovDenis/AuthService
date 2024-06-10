using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Myproject.Models
{
    public class LoginModel
    {
        public string Token { get; set; }
        public DateTime? ExpireTimeToken { get; set; }
    }
}