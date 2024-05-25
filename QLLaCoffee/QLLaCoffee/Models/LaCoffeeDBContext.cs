using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;

namespace QLLaCoffee.Models
{
    public partial class LaCoffeeDBContext : DbContext
    {
        public LaCoffeeDBContext()
            : base("name=LaCoffeeDBContext")
        {
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.GoodsCategories> GoodsCategories { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.Goods> Goods { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.DrinkCategories> DrinkCategories { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.Drinks> Drinks { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.Bills> Bills { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.BillInfos> BillInfos { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.Tables> Tables { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.Functions> Functions { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.UserCategories> UserCategories { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.Authorizations> Authorizations { get; set; }

        public System.Data.Entity.DbSet<QLLaCoffee.Models.ShiftReports> ShiftReports { get; set; }
    }
}
