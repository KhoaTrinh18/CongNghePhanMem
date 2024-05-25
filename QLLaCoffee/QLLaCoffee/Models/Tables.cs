using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("Tables")]
    public class Tables
    {
        public Tables()
        {

        }

        public Tables(String TableName, bool Status)
        {
            this.TableName = TableName;
            this.Status = Status;
        }

        [Key]
        public int TableID { get; set; }

        public String TableName { get; set; }

        public bool Status { get; set; }

        public virtual ICollection<Bills> Bills { get; set; }
    }
}