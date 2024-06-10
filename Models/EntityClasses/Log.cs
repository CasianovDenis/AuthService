using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Myproject.Models.EntityClasses
{
    [Table("api_request", Schema = "log")]
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string ApiName { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
    }
}