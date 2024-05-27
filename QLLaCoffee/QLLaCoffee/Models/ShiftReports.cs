using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("ShiftReports")]
    public class ShiftReports
    {
        public ShiftReports() { }

        [Key]
        [Display(Name = "Mã báo cáo kết ca")]
        public String ShiftReportID { get; set; }

        [Display(Name = "Doanh thu")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Required]
        public decimal Revenue { get; set; }

        [Display(Name = "Số tiền đầu ca")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Required]
        public decimal FirstAmount { get; set; }

        [Display(Name = "Số tiền kết ca")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Required]
        public decimal LastAmount { get; set; }

        [Display(Name = "Số hóa đơn")]
        [Required]
        public int BillCount { get; set; }

        [Display(Name = "Số tiền đang phục vụ")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Required]
        public decimal UncollectedAmount { get; set; }

        [Display(Name = "Thời gian đầu ca")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        [Required]
        public DateTime FirstTime { get; set; }

        [Display(Name = "Thời gian kết ca")]
        [DisplayFormat(DataFormatString = "{0:g}")]
        [Required]
        public DateTime LastTime { get; set; }

        public int UserID { get; set; }

        public virtual User User { get; set; }
    }
}