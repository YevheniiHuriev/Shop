function toggleDropdown() {
    const dropdownMenu = document.getElementById("dropdownMenu");
    dropdownMenu.classList.toggle("show");
}

const apiToken = localStorage.getItem("api_token");
const loginLink = document.getElementById("loginLink");
const userDropdown = document.getElementById("userDropdown");

async function getUserProfile() {
    try {
        const response = await fetch('https://localhost:7044/api/apiuser/profile', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${apiToken}`,
                'Content-Type': 'application/json'
            }
        });
        console.log

        if (response.ok) {
            const data = await response.json();
            const username = data.data.UserName;

            loginLink.style.display = "none";
            userDropdown.style.display = "block";
            document.querySelector(".dropdown-toggle").textContent = username;
        } else {
            console.error("Помилка отримання профілю користувача");
        }
    } catch (error) {
        console.error("Сталася помилка:", error);
    }
}

if (apiToken) {
    getUserProfile();
} else {
    loginLink.style.display = "block";
    userDropdown.style.display = "none";
}

document.addEventListener("click", function(event) {
    const dropdownMenu = document.getElementById("dropdownMenu");
    const userDropdown = document.getElementById("userDropdown");

    if (!userDropdown.contains(event.target) && dropdownMenu.classList.contains("show")) {
        dropdownMenu.classList.remove("show");
    }
});