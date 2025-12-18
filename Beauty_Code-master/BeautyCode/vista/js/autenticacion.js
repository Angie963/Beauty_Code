// Datos desde login.js
    const usuario = localStorage.getItem("usuario");
    const nombreUsuario = localStorage.getItem("nombreUsuario");

    const perfil = document.getElementById("perfilBtn");
    const login = document.getElementById("loginBtn");
    const logout = document.getElementById("logoutBtn");
    const bienvenidaTexto = document.getElementById("bienvenidaTexto");
    const saludoPrincipal = document.getElementById("saludoPrincipal");

    // Si hay sesión iniciada
    if (usuario && nombreUsuario) {
        perfil.style.display = "inline-block";
        bienvenidaTexto.textContent = "Perfil"; // SOLO mostrar "Perfil" en el menú
        login.style.display = "none";
        logout.style.display = "inline-block";

        // Cambiar saludo inferior
        saludoPrincipal.textContent = "Bienvenido, " + nombreUsuario;
    } 
    else {
        perfil.style.display = "none";
        login.style.display = "inline-block";
        logout.style.display = "none";

        saludoPrincipal.textContent = "Bienvenida a Beauty Code";
    }

    // Cerrar sesión
    logout.addEventListener("click", function () {
        localStorage.removeItem("usuario");
        localStorage.removeItem("nombreUsuario");
        window.location.reload();
    });