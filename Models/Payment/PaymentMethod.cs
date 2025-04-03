using System;
using System.ComponentModel.DataAnnotations;

namespace FlowerShop.Models.Payment
{
    public enum PaymentMethod
    {
        [Display(Name = "Thanh toán khi nhận hàng (COD)")]
        COD = 1,
        
        [Display(Name = "Thanh toán qua MoMo")]
        MoMo = 2,
        
        [Display(Name = "Chuyển khoản ngân hàng")]
        BankTransfer = 3
    }
}
