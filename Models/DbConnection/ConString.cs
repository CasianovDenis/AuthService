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
        //public DbSet<Teachers> Teachers { get; set; }
        //public DbSet<Subjects> Subjects { get; set; }
        //public DbSet<Students_Grade> Students_Grade { get; set; }

        public static ConString test(ConString t)
        {
            return t;
        }
        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
         {

             modelBuilder.Entity<Roles>().HasData(
              new Roles[]
              {
                 new Roles { ID=1, Name="Elev"},
                 new Roles { ID=2, Name="Profesor"},
                new Roles {  ID=3, Name="Director"},
              });

             modelBuilder.Entity<Users>().HasData(new Users
             {
                 ID = 1,
                 Username = "admin",
                 Password = "597f5441e7d174b607873874ed54b974674986ad543e7458e28a038671c9f64c",//testadmin
                 Email = "admin@gmail.com",
                 Role_id = 3,

             });

         }*/
    }
}