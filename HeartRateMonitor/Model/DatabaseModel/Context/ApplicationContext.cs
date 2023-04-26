using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Model.DatabaseModel.Context
{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {

        }
        public DbSet<Device> Devices { get; set; }
        public DbSet<User> Users { get; set; }  
        public DbSet<Scene> Scenes { get; set; }    
        public DbSet<Physiological_parameter> Parameters { get; set; }
        public DbSet<Company> Companies { get;set; }
        public DbSet<SceneType> SceneTypes { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Company>()
        //        .HasMany(c => c.Devices)
        //        .WithRequired(c => c.Company);


        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
