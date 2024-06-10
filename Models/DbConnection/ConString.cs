using Microsoft.EntityFrameworkCore;
using Myproject.Models.EntityClasses;

namespace Myproject.Models.DBConnection
{
    public class ConString : DbContext
    {
        public ConString(DbContextOptions<ConString> options) : base(options)
        {

        }

        public DbSet<Users> Users { get; set; }

        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Dictionary>().HasData(
             new Dictionary[]
             {
                 new Dictionary { Id=1, Code="SUCCES",Description = "Succes"},
                 new Dictionary { Id=2, Code="TECHNICAL_ERROR",Description = "Something wrong"},
                new Dictionary {  Id=3, Code="USER_ALREADY_EXIST",Description = "Username already in use, choser another"},
                new Dictionary {  Id=3, Code="NO_RECORD",Description = "No Data"},
                new Dictionary {  Id=3, Code="USER_NOT_EXIST",Description = "User not found"},

             });

            /*  
            modelBuilder.Entity<Users>().HasData(new Users
              {
                  ID = 1,
                  Username = "admin",
                  Password = "597f5441e7d174b607873874ed54b974674986ad543e7458e28a038671c9f64c",//testadmin
                  Email = "admin@gmail.com",
                  Role_id = 3,

              });*/

        }
    }
}