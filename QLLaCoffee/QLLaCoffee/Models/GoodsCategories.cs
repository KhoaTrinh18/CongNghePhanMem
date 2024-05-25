using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("GoodsCategories")]
    public class GoodsCategories
    {
        [Key]
        [Display(Name = "Mã loại mặt hàng")]
        public int GoodsCategoryID { get; set; }

        [Display(Name = "Tên loại mặt hàng")]
        [Required(ErrorMessage = "Tên loại mặt hàng không được để trống")]
        public String GoodsCategoryName { get; set; }

        [Display(Name = "Mô tả")]
        public String GoodsCategoryDescription { get; set; }

        public virtual ICollection<Goods> Goods { get; set; }

    }
}