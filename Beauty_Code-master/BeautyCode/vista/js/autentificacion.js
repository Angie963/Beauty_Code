document.addEventListener("DOMContentLoaded", () => {
    const user = JSON.parse(localStorage.getItem("user"));

    const linkPerfil = document.querySelector("linkPerfil");
    const linkLogin = document.querySelector("btnLogin");
    const btnLogout = document.getElementById("btnLogout");

    if (user) {
        // Usuario con sesión
        linkPerfil.style.display = "inline-block";
        linkLogin.style.display = "none";
        btnLogout.style.display = "inline-block";

        // Mostrar nombre usuario
        mostrarSaludo(user);

    } else {
        // Sin sesión
        linkPerfil.style.display = "none";
        linkLogin.style.display = "inline-block";
        btnLogout.style.display = "none";
    }

    // Cerrar sesión
    btnLogout.addEventListener("click", () => {
        localStorage.removeItem("user");
        window.location.reload();
    });
});

// Mostrar saludo en el menú
function mostrarSaludo(user) {
    const menu = document.getElementById("menu-links");

    const saludo = document.createElement("span");
    saludo.textContent = `Hola, ${user.nombre}`;
    saludo.style.marginLeft = "15px";
    saludo.style.color = "#6B0C17";
    saludo.style.fontWeight = "600";

    menu.appendChild(saludo);
}
