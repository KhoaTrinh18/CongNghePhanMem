using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QLLaCoffee.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Tên nhân viên không được để trống")]
        public String FullName { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Nhập đúng định dạng số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public String PhoneNumber { get; set; }

        [Display(Name = "Hình ảnh")]
        public String UserImage {  get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Giới tính không được để trống")]
        public String Gender { get; set; }

        [Display(Name = "Tên tài khoản")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Tên tài khoản phải có độ tối thiểu 2 và tối đa 25")]
        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        public String AccountName { get; set; }

        [Display(Name = "Mật khẩu")]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "Mật khẩu phải có độ tối thiểu 4 và tối đa 10")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public String Password { get; set; }

        [Display(Name = "Mã loại người dùng")]
        [Required(ErrorMessage = "Mã loại người dùng không được để trống")]
        public int UserCategoryID { get; set; }

        public virtual UserCategories UserCategories { get; set; }

        public virtual ICollection<Bills> Bills { get; set; }

        public virtual ICollection<ShiftReports> ShiftReports { get; set; }
    }
}