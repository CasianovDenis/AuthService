using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Myproject.Models.EntityClasses
{
    [Table("Dictionary", Schema = "config")]
    public class Dictionary
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }
        public string Description { get; set; }
        public Guid Type { get; set; }
    }
}