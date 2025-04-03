using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FlowerShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [DisplayName("Họ tên")]
        public string FullName { get; set; } = string.Empty;

        public string? FullName_khongdau { get; set; }

        [DisplayName("Địa chỉ")]
        public string? Address { get; set; }

        [DisplayName("Tuổi")]
        [Range(1, 120, ErrorMessage = "Tuổi phải từ 1 đến 120")]
        public string? Age { get; set; }

        // Navigation property
        public ICollection<Order>? Orders { get; set; }
    }
}