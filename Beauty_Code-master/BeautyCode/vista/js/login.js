document.getElementById("loginForm").addEventListener("submit", async function(e) {
    e.preventDefault();

    let email = document.getElementById("email").value.trim();
    let password = document.getElementById("password").value.trim();

    try {
        const response = await fetch("http://localhost:5272/api/Users/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                email: email,
                contrasena: password,
                password: password
            })
        });

        if (!response.ok) {
            const error = await response.json();
            alert(error.message || "Correo o contraseña incorrectos");
            return;
        }

        const data = await response.json();

        // Redirecciones según rol
        if (data.user.rol.includes("administrador")) {
            window.location.href = "dashboard-admin.html";
        } else if (data.user.rol.includes("empleado")) {
            window.location.href = "dashboard-empleado.html";
        } else {
            window.location.href = "index.html";
        }

    } catch (error) {
        alert("No se pudo conectar al servidor: " + error);
    }
});
