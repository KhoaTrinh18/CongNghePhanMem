using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("Drinks")]
    public class Drinks
    { 
        [Key]
        [Display(Name = "Mã đồ uống")]
        public int DrinkID { get; set; }

        [Display(Name = "Mã loại đồ uống")]
        [Required(ErrorMessage = "Mã loại đồ uống không được để trống")]
        public int DrinkCategoryID { get; set; }

        [Display(Name = "Tên đồ uống")]
        [Required(ErrorMessage = "Tên đồ uống không được để trống")]
        public String DrinkName { get; set; }

        [Display(Name = "Hình ảnh")]
        public String DrinkImage {  get; set; }

        [Display(Name = "Giá bán")]
        [DisplayFormat(DataFormatString = "{0:C0}")]
        [Range(minimum: 0, maximum: 1000000, ErrorMessage = "Giá bán không được âm")]
        [Required(ErrorMessage = "Giá bán không được để trống")]
        public decimal DrinkPrice { get; set; }

        public virtual DrinkCategories DrinkCategories { get; set; }

        public virtual ICollection<BillInfos> BillInfos { get; set; }
    }
}