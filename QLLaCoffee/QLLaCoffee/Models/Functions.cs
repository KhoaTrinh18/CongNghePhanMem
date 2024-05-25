using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("Functions")]
    public class Functions
    {
        [Key]
        public String FunctionID { get; set; }

        [Required]
        public String FunctionName { get; set; }

        public String FunctionGroup {  get; set; }

        public virtual ICollection<Authorizations> Authorizations { get; set; }
    }
}