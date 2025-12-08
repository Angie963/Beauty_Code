const usuarios = [
    { usuario: "admin", password: "admin123", rol: "admin", pagina: "dashboard-admin.html" },
    { usuario: "empleado", password: "empleado123", rol: "empleado", pagina: "dashboard-empleado.html" },
    { usuario: "cliente", password: "cliente123", rol: "cliente", pagina: "index.html" }
];

document.getElementById("loginForm").addEventListener("submit", function(e) {
    e.preventDefault();

    let user = document.getElementById("username").value.trim();
    let pass = document.getElementById("password").value.trim();

    let encontrado = usuarios.find(u => u.usuario === user && u.password === pass);

    if (encontrado) {

        // Guardamos al usuario para que funcione lo del menú
        localStorage.setItem("user", JSON.stringify({
            nombre: encontrado.usuario,
            rol: encontrado.rol
        }));

        alert("Inicio de sesión exitoso como " + encontrado.rol);
        window.location.href = encontrado.pagina;

    } else {
        alert("Usuario o contraseña incorrectos");
    }
});
