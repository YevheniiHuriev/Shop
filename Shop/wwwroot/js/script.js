const apiUrl = "http://localhost:7044/api/apiorders";

document.getElementById("orderForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const order = {
        totalAmount: document.getElementById("totalAmount").value,
        userId: document.getElementById("userId").value,
        products: JSON.parse(document.getElementById("products").value)
    };

    try {
        const response = await fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem("api_token")}`
            },
            body: JSON.stringify(order)
        });

        if (response.ok) {
            alert("Замовлення успішно додано!");
        } else {
            alert("Помилка при додаванні замовлення");
        }
    } catch (error) {
        console.error("Помилка: ", error);
        alert("Щось пішло не так!");
    }
});

document.getElementById("editOrderForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const orderId = document.getElementById("orderId").value;
    const order = {
        totalAmount: document.getElementById("totalAmount").value,
        userId: document.getElementById("userId").value,
        products: JSON.parse(document.getElementById("products").value)
    };

    try {
        const response = await fetch(`${apiUrl}/${orderId}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${localStorage.getItem("api_token")}`
            },
            body: JSON.stringify(order)
        });

        if (response.ok) {
            alert("Замовлення оновлено!");
        } else {
            alert("Помилка при оновленні замовлення");
        }
    } catch (error) {
        console.error("Помилка: ", error);
        alert("Щось пішло не так!");
    }
});

async function fetchOrders() {
    try {
        const response = await fetch(apiUrl, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${localStorage.getItem("api_token")}`
            }
        });

        const orders = await response.json();
        const ordersTable = document.getElementById("ordersTable").getElementsByTagName("tbody")[0];
        ordersTable.innerHTML = "";

        orders.forEach(order => {
            const row = ordersTable.insertRow();
            row.innerHTML = `
                <td>${order.id}</td>
                <td>${order.totalAmount}</td>
                <td>${order.userId}</td>
                <td>${JSON.stringify(order.products)}</td>
                <td>
                    <button onclick="editOrder(${order.id})">Редагувати</button>
                    <button onclick="deleteOrder(${order.id})">Видалити</button>
                </td>
            `;
        });
    } catch (error) {
        console.error("Помилка: ", error);
        alert("Щось пішло не так!");
    }
}

function editOrder(orderId) {
    window.location.href = `edit-order.html?id=${orderId}`;
}

async function deleteOrder(orderId) {
    try {
        const response = await fetch(`${apiUrl}/${orderId}`, {
            method: "DELETE",
            headers: {
                "Authorization": `Bearer ${localStorage.getItem("api_token")}`
            }
        });

        if (response.ok) {
            alert("Замовлення видалено!");
            fetchOrders();
        } else {
            alert("Помилка при видаленні замовлення");
        }
    } catch (error) {
        console.error("Помилка: ", error);
        alert("Щось пішло не так!");
    }
}

if (document.getElementById("ordersTable")) {
    fetchOrders();
}
