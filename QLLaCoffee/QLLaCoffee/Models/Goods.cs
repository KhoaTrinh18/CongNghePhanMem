using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("Goods")]
    public class Goods
    {
        [Key]
        [Display(Name = "Mã mặt hàng")]
        public int GoodsID { get; set; }

        [Display(Name = "Mã loại mặt hàng")]
        [Required(ErrorMessage = "Mã loại mặt hàng không được để trống")]
        public int GoodsCategoryID { get; set; }

        [Display(Name = "Tên mặt hàng")]
        [Required(ErrorMessage = "Tên mặt hàng không được để trống")]
        public String GoodsName { get; set; }

        [Display(Name = "Số lượng")]
        [Range(minimum: 0, maximum: 1000000, ErrorMessage = "Số lượng không được âm")]
        [Required(ErrorMessage = "Số lượng không được để trống")]
        public int GoodsCount { get; set; }

        [Display(Name = "Đơn vị")]
        [Required(ErrorMessage = "Đơn vị không được để trống")]
        public String GoodsUnit { get; set; }

        [Display(Name = "Giá nhập")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Range(minimum: 0, maximum: 1000000, ErrorMessage = "Giá nhập không được âm")]
        [Required(ErrorMessage = "Giá nhập không được để trống")]
        public decimal GoodsPrice { get; set; }

        public virtual GoodsCategories GoodsCategories { get; set; }
    }
}