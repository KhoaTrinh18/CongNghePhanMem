using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    [Table("UserCatgories")]
    public class UserCategories
    {
        [Key]
        public int UserCategoryID { get; set; }

        [Display(Name = "Tên loại người dùng")]
        [Required(ErrorMessage = "Tên loại người dùng không được để trống")]
        public String UserCategoryName { get; set; }

        [Display(Name = "Mô tả")]
        public String UserCategoryDescription { get; set; }

        public virtual ICollection<Authorizations> Authorizations { get; set; }
        
        public virtual ICollection<User> Users { get; set; }
    }
}