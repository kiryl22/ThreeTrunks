using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeTrunks.Data.Models;

namespace ThreeTrunks.Data
{
    public class ThreeTrunksContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<ImageCategory> ImageCategories { get; set; }
        public DbSet<Settings> Settings { get; set; } 
    }
}
