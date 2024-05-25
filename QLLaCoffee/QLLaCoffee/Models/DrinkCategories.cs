using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("DrinkCategories")]
    public class DrinkCategories
    {
        [Key]
        [Display(Name = "Mã loại đồ uông")]
        public int DrinkCategoryID { get; set; }

        [Display(Name = "Tên loại đồ uống")]
        [Required(ErrorMessage = "Tên loại đồ uống không được để trống")]
        public String DrinkCategoryName { get; set; }

        [Display(Name = "Mô tả")]
        public String DrinkCategoryDesciption { get; set; }

        public virtual ICollection<Drinks> Drinks { get; set; }
    }
}