using Microsoft.Data.SqlClient;
using Myproject.Models.DBConnection;

namespace Myproject.Base.Models
{
    public class BaseAttribute : Attribute
    {
        private int userId { get; set; }
        private string _conString { get; set; }

        private bool mmmm { get { return false; } }
        public bool test { get; set; }
    }
}