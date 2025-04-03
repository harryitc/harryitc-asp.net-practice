using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FlowerShop.Models;

namespace FlowerShop.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "Vai trò")]
        public List<string> Roles { get; set; }
        
        [Display(Name = "Email đã xác nhận")]
        public bool EmailConfirmed { get; set; }
        
        [Display(Name = "Thời gian khóa")]
        public DateTimeOffset? LockoutEnd { get; set; }
        
        [Display(Name = "Trạng thái")]
        public string Status => LockoutEnd != null && LockoutEnd > DateTimeOffset.Now ? "Bị khóa" : "Hoạt động";
    }

    public class UserDetailsViewModel : UserViewModel
    {
        [Display(Name = "Đơn hàng")]
        public List<Order> Orders { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "Email đã xác nhận")]
        public bool EmailConfirmed { get; set; }
        
        [Display(Name = "Thời gian khóa")]
        public DateTimeOffset? LockoutEnd { get; set; }
        
        [Display(Name = "Vai trò hiện tại")]
        public List<string> UserRoles { get; set; }
        
        [Display(Name = "Tất cả vai trò")]
        public List<string> AllRoles { get; set; }
        
        [Display(Name = "Vai trò được chọn")]
        public List<string> SelectedRoles { get; set; }
    }
}
