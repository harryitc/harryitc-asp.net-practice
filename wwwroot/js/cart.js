function updateCartUI(cart) {
    const cartCount = document.querySelector('.cart-count');
    const cartItems = document.getElementById('cartItems');
    const cartTotal = document.getElementById('cartTotal');

    // Update count
    if(cart.length == 0) {
        cartCount.classList.remove('d-inline-block');
        cartCount.classList.add('d-none');
    }else {
        cartCount.classList.remove('d-none');
        cartCount.classList.add('d-inline-block');
        cartCount.textContent = cart.length;
    }

    // Check if cart is empty
    if (cart.length === 0) {
        cartItems.innerHTML = `
            <div class="empty-cart">
                <i class="fas fa-shopping-cart mb-2" style="font-size: 2rem;"></i>
                <p class="mb-0">Giỏ hàng trống</p>
            </div>`;
        cartTotal.textContent = '0đ';
        return;
    }

    // Update items
    cartItems.innerHTML = cart.map(item => `
        <div class="dropdown-item">
            <div class="d-flex align-items-center gap-2">
                <img src="${item.imageUrl}" alt="${item.productName}"
                     style="width: 40px; height: 40px; object-fit: cover;">
                <div class="flex-grow-1">
                    <h6 class="mb-0">${item.productName}</h6>
                    <div class="d-flex align-items-center gap-2">
                        <div class="input-group input-group-sm" style="width: 100px;">
                            <button class="btn btn-outline-secondary btn-sm" type="button" data-product-id="${item.productId}"
                                    onclick="updateQuantity(${item.productId}, ${item.quantity - 1}, event)">-</button>
                            <input type="text" class="form-control text-center" value="${item.quantity}" readonly>
                            <button class="btn btn-outline-secondary btn-sm" type="button" data-product-id="${item.productId}"
                                    onclick="updateQuantity(${item.productId}, ${item.quantity + 1}, event)">+</button>
                        </div>
                        <button class="btn btn-link text-danger btn-sm p-0"
                                onclick="removeFromCart(${item.productId})">
                            <i class="fas fa-trash"></i>
                        </button>
                    </div>
                </div>
                <div class="text-end">
                    <div>${item.price.toLocaleString()}đ</div>
                </div>
            </div>
        </div>
    `).join('');

    // Update total
    const total = cart.reduce((sum, item) => sum + (item.price * item.quantity), 0);
    cartTotal.textContent = total.toLocaleString() + 'đ';
}

async function addToCart(productId, quantity = 1) {
    try {
        // Tạo FormData để gửi dữ liệu dạng form
        const formData = new FormData();
        formData.append('productId', productId);
        formData.append('quantity', quantity);
        formData.append('__RequestVerificationToken', document.querySelector('input[name="__RequestVerificationToken"]').value);

        const response = await fetch('/Cart/AddToCart', {
            method: 'POST',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: formData
        });

        if (!response.ok) throw new Error('Network response was not ok');

        const cart = await getCart();
        updateCartUI(cart);

        // Show success toast
        Swal.fire({
            icon: 'success',
            title: 'Đã thêm vào giỏ hàng',
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 1500
        });

        return true; // Trả về true nếu thành công
    } catch (error) {
        console.error('Error:', error);
        return false; // Trả về false nếu có lỗi
    }
}

async function getCart() {
    const response = await fetch('/Cart/GetCart');
    return await response.json();
}

async function updateQuantity(productId, newQuantity, e) {
    // Nếu được gọi từ sự kiện click, sử dụng target của sự kiện
    // Nếu không, sử dụng button có data-product-id tương ứng
    let button;

    if (e && e.target) {
        button = e.target;
    } else {
        // Tìm button dựa vào productId
        button = document.querySelector(`button[data-product-id="${productId}"]`) || document.activeElement;
    }

    const originalText = button.innerHTML;
    button.disabled = true;
    button.innerHTML = '<span class="spinner-border spinner-border-sm"></span>';

    try {
        await updateCartItem(productId, newQuantity);
        const cart = await getCart();
        updateCartUI(cart);
    } catch (error) {
        Swal.fire({
            icon: 'error',
            title: 'Lỗi',
            text: 'Không thể cập nhật số lượng',
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000
        });
    } finally {
        button.disabled = false;
        button.innerHTML = originalText;
    }
}

async function removeFromCart(productId) {
    try {
        // Tạo FormData để gửi dữ liệu dạng form
        const formData = new FormData();
        formData.append('productId', productId);
        formData.append('__RequestVerificationToken', document.querySelector('input[name="__RequestVerificationToken"]').value);

        const response = await fetch('/Cart/RemoveFromCart', {
            method: 'POST',
            headers: {
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: formData
        });

        if (!response.ok) throw new Error('Network response was not ok');

        const cart = await getCart();
        updateCartUI(cart);

        // Hiển thị thông báo đã xóa
        Swal.fire({
            icon: 'success',
            title: 'Đã xóa sản phẩm',
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 1500
        });
    } catch (error) {
        console.error('Error:', error);

        // Hiển thị thông báo lỗi
        Swal.fire({
            icon: 'error',
            title: 'Lỗi',
            text: 'Không thể xóa sản phẩm',
            toast: true,
            position: 'top-end',
            showConfirmButton: false,
            timer: 3000
        });
    }
}

async function updateCartItem(productId, newQuantity) {
    // Tạo FormData để gửi dữ liệu dạng form
    const formData = new FormData();
    formData.append('productId', productId);
    formData.append('quantity', newQuantity);
    formData.append('__RequestVerificationToken', document.querySelector('input[name="__RequestVerificationToken"]').value);

    const response = await fetch('/Cart/UpdateQuantity', {
        method: 'POST',
        headers: {
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: formData
    });

    if (!response.ok) {
        throw new Error('Failed to update cart item');
    }

    return response;
}

// Load cart when page loads
document.addEventListener('DOMContentLoaded', async () => {
    const cart = await getCart();
    updateCartUI(cart);
});

// Add event listener to prevent dropdown from closing when clicking inside
document.addEventListener('DOMContentLoaded', function() {
    const dropdownMenu = document.querySelector('.dropdown-menu');
    dropdownMenu.addEventListener('click', function(e) {
        e.stopPropagation();
    });
});

