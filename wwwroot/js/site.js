// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function showOrderSuccess(orderId) {
    Swal.fire({
        title: 'Đặt hàng thành công!',
        text: `Mã đơn hàng của bạn là #${orderId}`,
        icon: 'success',
        showConfirmButton: true,
        confirmButtonText: 'Xem chi tiết đơn hàng',
        showCancelButton: true,
        cancelButtonText: 'Tiếp tục mua sắm'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = `/Order/Details/${orderId}`;
        } else {
            window.location.href = '/Product';
        }
    });
}
