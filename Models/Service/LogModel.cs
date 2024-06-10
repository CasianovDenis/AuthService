using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Myproject.Models.Repository
{
    public class LogModel
    {
        public int? Id { get; set; }
        public string ApiName { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
    }
}