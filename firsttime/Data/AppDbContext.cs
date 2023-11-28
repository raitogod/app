using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using firsttime.Models;

namespace firsttime.Data
{
    class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseMySql($"Server=localhost;Database=firsttimedatabase;Port=3306;User=root;Password=root");
        }

    }
}
