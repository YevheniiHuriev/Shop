document.getElementById("loginForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    await auth(email, password);
});

const auth = async (email, password) => {
    try {
        const response = await fetch(`https://localhost:7044/api/apiuser/auth`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ email, password }),
        });
        const data = await response.json();

        if (response.ok) {
            localStorage.setItem("api_token", data.token);
            console.log("Login successful!", data);

            window.location.href = "../index.html";
        } else {
            console.log(data.message);
        }
    } catch (error) {
        console.error("Login failed:", error);
    }
};
