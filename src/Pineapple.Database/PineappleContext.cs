using System;
using System.Data.Entity;
using System.Reflection;
using Pineapple.Database.Models;

namespace Pineapple.Database
{
    public sealed class PineappleContext : DbContext
    {
        public PineappleContext(String nameOrConnectionString) : base(nameOrConnectionString)
        {
            System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PineappleContext>());
            //System.Data.Entity.Database.SetInitializer(new DropCreateDatabaseAlways<PineappleContext>());
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
