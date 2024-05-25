using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("Authorizations")]
    public class Authorizations
    {
        [Key, Column(Order = 1)]
        public int UserCategoryID { get; set; }

        [Key, Column(Order = 2)]
        public String FunctionID { get; set; }

        public String AuthorizationDescription { get; set; }

        public virtual UserCategories UserCategories { get; set; }

        public virtual Functions Functions { get; set; }
    }
}