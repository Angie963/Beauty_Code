document.getElementById("registerForm").addEventListener("submit", async function(event) {
    event.preventDefault();

    const fullName = document.getElementById("fullName").value.trim();
    const doctype = document.getElementById("doctype").value.trim();
    const documentNum = document.getElementById("document").value.trim();
    const email = document.getElementById("email").value.trim();
    const phone = document.getElementById("phone").value.trim();
    const password = document.getElementById("password").value.trim();
    const confirmPassword = document.getElementById("confirmpassword").value.trim();

    const errorSpan = document.getElementById("error-password");

    if (password !== confirmPassword) {
        errorSpan.textContent = "Las contraseñas no coinciden.";
        return;
    } else {
        errorSpan.textContent = "";
    }

    // ---- AQUÍ LA CLAVE ----
    const userData = {
        nombre: fullName,
        contrasena: password,
        email: email,
        telefono: phone,
        tipo_documento: doctype,
        numero_documento: documentNum,
        roles: ["cliente"]
    };

    try {
        const response = await fetch("http://localhost:5272/api/Users", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(userData)
        });

        
        let data;
try {
    data = await response.json();
} catch {
    data = null;
}

console.log("STATUS:", response.status);
console.log("RAW RESPONSE:", data);


        if (!response.ok) {
            alert(data.message || "Error al registrar el usuario");
            return;
        }

        alert("Usuario registrado correctamente");
        window.location.href = "login.html";

    } catch (error) {
        console.error(error);
        alert("No se pudo conectar con el servidor.");
    }
});
