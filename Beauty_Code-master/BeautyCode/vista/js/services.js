async function cargarServicios() {
    const servicios = JSON.parse(localStorage.getItem("servicios")) || [];

    // Si no hay servicios guardados, crea unos de ejemplo
    if (servicios.length === 0) {
        const serviciosDemo = [
            {
                id: "1",
                nombre: "Manicura en Gel",
                descripcion: "Duración hasta por 3 semanas.",
                precio: 50000,
                imagen: "img/servicios/uñas en gel.jpg"
            },
            {
                id: "2",
                nombre: "Uñas Acrílicas",
                descripcion: "Elegantes, fuertes y personalizables.",
                precio: 90000,
                imagen: "img/servicios/acrilicas.jpg"
            },
            {
                id: "3",
                nombre: "Semipermanentes",
                descripcion: "Hidratación profunda y exfoliación.",
                precio: 40000,
                imagen: "img/servicios/semipermanente.jpg"
            }
        ];

        localStorage.setItem("servicios", JSON.stringify(serviciosDemo));
        return serviciosDemo;
    }

    return servicios;
}

// Pintar tarjetas en el HTML
function mostrarServicios(servicios) {
    const contenedor = document.getElementById("servicesList");
    contenedor.innerHTML = ""; // limpiar antes de pintar

    servicios.forEach(serv => {
        const card = document.createElement("div");
        card.classList.add("tarjeta", "tarjeta-serv");

        card.innerHTML = `
            <div class="service-img-container">
                <img src="${serv.imagen}" alt="${serv.nombre}">
            </div>
            <h3>${serv.nombre}</h3>
            <p class="service-desc">${serv.descripcion}</p>
            <p class="service-price">$${serv.precio.toLocaleString()} COP</p>

            <a href="appointments.html?servicio=${serv.id}" class="btn-primary">
                Agendar
            </a>
        `;

        contenedor.appendChild(card);
    });
}

// Inicializar
document.addEventListener("DOMContentLoaded", async () => {
    const servicios = await cargarServicios();
    mostrarServicios(servicios);
});