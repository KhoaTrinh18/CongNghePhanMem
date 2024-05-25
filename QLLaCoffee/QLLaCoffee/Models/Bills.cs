using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("Bills")]
    public class Bills
    {
        [Key]
        [Display(Name = "Mã hóa đơn")]
        public String BillID { get; set; }

        [Display(Name = "Ngày lập")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        [Required]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Required]
        public decimal TotalAmount { get; set; }

        public int? TableID { get; set; }

        public int UserID { get; set; }

        public virtual ICollection<BillInfos> BillInfos { get; set; }

        public virtual Tables Tables { get; set; }

        public virtual User User { get; set; }
    }
}