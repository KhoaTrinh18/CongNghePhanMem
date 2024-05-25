using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("BillInfos")]
    public class BillInfos
    {
        [Key, Column(Order = 1)]
        public String BillID { get; set; }

        [Key, Column(Order = 2)]
        public int DrinkID { get; set;}

        [Display(Name = "Số lượng")]
        [Required]
        public int DrinkCount { get; set; }

        [Display(Name = "Thành tiền")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Required]
        public decimal DrinkPrice { get; set; }

        public virtual Bills Bills { get; set; }

        public virtual Drinks Drinks { get; set; }
    }
}