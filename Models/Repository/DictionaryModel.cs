using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Myproject.Models.EntityClasses;

namespace Myproject.Model.Repository
{
    public class DictionaryModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Guid Type { get; set; }

        public DictionaryModel(Dictionary @in)
        {
            Id = @in.Id;
            Code = @in.Code;
            Description = @in.Description;
            //Type = @in.Type;
        }
        public DictionaryModel() { }
    }
}