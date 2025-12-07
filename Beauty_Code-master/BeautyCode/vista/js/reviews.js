// Cargar reseñas
function cargarResenas() {
    let resenas = JSON.parse(localStorage.getItem("resenas"));

    if (!resenas || resenas.length == 0) {
        resenas = [
            {
                id: Date.now(),
                nombre: "María Gómez",
                servicio: "Manicure Tradicional",
                rating: 5,
                comentario: "Excelente trabajo, muy detallado y profesional. 100% recomendado."
            },
            {
                id: Date.now() + 1,
                nombre: "Valentina Ruiz",
                servicio: "Uñas Acrílicas",
                rating: 4,
                comentario: "Me encantó el diseño, aunque tardó un poco más de lo esperado."
            },
            {
                id: Date.now() + 2,
                nombre: "Camila Ortega",
                servicio: "Pedicure Spa",
                rating: 5,
                comentario: "Un servicio súper relajante, quedé encantada con el resultado."
            }
        ];

        localStorage.setItem("resenas", JSON.stringify(resenas));
    }

    return resenas;
}

// Mostrar reseñas
function mostrarResenas() {
    const cont = document.getElementById("reviewsList");
    cont.innerHTML = "";

    const resenas = cargarResenas();

    resenas.forEach(r => {
        const card = document.createElement("div");
        card.style.background = "#f1d4d0";
        card.style.padding = "20px";
        card.style.borderRadius = "15px";
        card.style.marginBottom = "20px";
        card.style.boxShadow = "0px 8px 25px rgba(150,0,30,0.10)";
        card.style.position = "relative";

        let estrellas = "★".repeat(r.rating) + "☆".repeat(5 - r.rating);

        card.innerHTML = `
            <div style="display:flex;justify-content:space-between;">
                <strong style="font-size:18px;">${r.nombre}</strong>
                <span style="color:gold;font-size:22px;">${estrellas}</span>
            </div>
            <p style="color:#777;margin:5px 0;">Servicio: ${r.servicio}</p>
            <p>${r.comentario}</p>

            <button 
                onclick="eliminarResena(${r.id})"
                style="
                    position:absolute; 
                    top:10px; 
                    right:10px; 
                    background:none; 
                    border:none; 
                    font-size:18px;
                    cursor:pointer;
                    color:#7a1a1a;
                ">
                ✖
            </button>`;

        cont.appendChild(card);
    });
}

// Limpiar formulario
function limpiarFormulario() {
    document.getElementById("inputNombre").value = "";
    document.getElementById("inputServicio").value = "";
    document.getElementById("inputRating").value = "5";
    document.getElementById("inputComentario").value = "";
}

// Guardar reseña nueva
document.getElementById("btnGuardarResena").onclick = () => {
    const nombre = document.getElementById("inputNombre").value;
    const servicio = document.getElementById("inputServicio").value;
    const rating = parseInt(document.getElementById("inputRating").value);
    const comentario = document.getElementById("inputComentario").value;

    if (!nombre || !servicio || !comentario) {
        alert("Por favor completa todos los campos");
        return;
    }

    const resenas = cargarResenas();

    resenas.push({
        id: Date.now(),
        nombre,
        servicio,
        rating,
        comentario
    });

    localStorage.setItem("resenas", JSON.stringify(resenas));

    limpiarFormulario();
    modal.style.display = "none";
    mostrarResenas();
};

// Eliminar reseña
function eliminarResena(id) {
    let resenas = cargarResenas();
    resenas = resenas.filter(r => r.id !== id);
    localStorage.setItem("resenas", JSON.stringify(resenas));
    mostrarResenas();
}

// Manejo del modal
const modal = document.getElementById("modalResena");

document.getElementById("btnAbrirResena").onclick = () => {
    limpiarFormulario();
    modal.style.display = "flex";
};

document.getElementById("btnCancelar").onclick = () => {
    modal.style.display = "none";
};

// Inicializar
document.addEventListener("DOMContentLoaded", mostrarResenas);
