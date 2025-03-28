function updateCartUI(cart) {
    const cartCount = document.querySelector('.cart-count');
    const cartItems = document.getElementById('cartItems');
    const cartTotal = document.getElementById('cartTotal');
    
    // Update count
    cartCount.textContent = cart.length;
    
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
                            <button class="btn btn-outline-secondary btn-sm" type="button" 
                                    onclick="updateQuantity(${item.productId}, ${item.quantity - 1})">-</button>
                            <input type="text" class="form-control text-center" value="${item.quantity}" readonly>
                            <button class="btn btn-outline-secondary btn-sm" type="button" 
                                    onclick="updateQuantity(${item.productId}, ${item.quantity + 1})">+</button>
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
        const response = await fetch('/Cart/AddToCart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({ productId, quantity })
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
            timer: 3000
        });
    } catch (error) {
        console.error('Error:', error);
    }
}

async function getCart() {
    const response = await fetch('/Cart/GetCart');
    return await response.json();
}

async function updateQuantity(productId, quantity) {
    if (quantity < 1) return;
    
    try {
        const response = await fetch('/Cart/UpdateQuantity', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({ productId, quantity })
        });

        if (!response.ok) throw new Error('Network response was not ok');
        
        const cart = await getCart();
        updateCartUI(cart);
    } catch (error) {
        console.error('Error:', error);
    }
}

async function removeFromCart(productId) {
    try {
        const response = await fetch('/Cart/RemoveFromCart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({ productId })
        });

        if (!response.ok) throw new Error('Network response was not ok');
        
        const cart = await getCart();
        updateCartUI(cart);
    } catch (error) {
        console.error('Error:', error);
    }
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

