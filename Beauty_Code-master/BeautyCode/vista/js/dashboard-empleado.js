// Datos ficticios
let citas = [
    { id: 1, cliente: "Laura Pérez", servicio: "Manicura en Gel", fecha: "2025-12-10", hora: "11:00", estado: "Pendiente" },
    { id: 2, cliente: "Sofía Ríos", servicio: "Uñas Acrílicas", fecha: "2025-12-10", hora: "13:00", estado: "Pendiente" },
    { id: 3, cliente: "María Gómez", servicio: "Semipermanente", fecha: "2025-12-10", hora: "15:30", estado: "Completada" }
];

let citaSeleccionada = null;

// Cargar citas en tabla
function cargarCitas() {
    const tbody = document.getElementById("tablaCitas");
    tbody.innerHTML = "";

    citas.forEach(c => {
        const fila = document.createElement("tr");

        let badge = `<span class="badge bg-warning">Pendiente</span>`;
        if (c.estado === "Completada") badge = `<span class="badge bg-success">Completada</span>`;
        if (c.estado === "Cancelada") badge = `<span class="badge bg-danger">Cancelada</span>`;

        fila.innerHTML = `
            <td>${c.cliente}</td>
            <td>${c.servicio}</td>
            <td>${c.fecha}</td>
            <td>${c.hora}</td>
            <td>${badge}</td>
            <td>
                ${c.estado === "Pendiente" ? `
                    <button class="btn btn-success btn-sm" onclick="marcarCompletada(${c.id})">Completada</button>
                    <button class="btn btn-danger btn-sm" onclick="cancelarCita(${c.id})">Cancelar</button>
                    <button class="btn btn-warning btn-sm" onclick="abrirModal(${c.id})">Reagendar</button>
                ` : `<button class="btn btn-secondary btn-sm" disabled>✔</button>`}
            </td>
        `;

        tbody.appendChild(fila);
    });

    actualizarContadores();
}

// Marcar como completada
function marcarCompletada(id) {
    const c = citas.find(c => c.id === id);
    c.estado = "Completada";
    cargarCitas();
}

// Cancelar cita
function cancelarCita(id) {
    const c = citas.find(c => c.id === id);
    c.estado = "Cancelada";
    cargarCitas();
}

// Abrir modal reagendar
function abrirModal(id) {
    citaSeleccionada = citas.find(c => c.id === id);

    document.getElementById("nuevaFecha").value = citaSeleccionada.fecha;
    document.getElementById("nuevaHora").value = citaSeleccionada.hora;

    document.getElementById("modalReagendar").style.display = "flex";
}

function cerrarModal() {
    document.getElementById("modalReagendar").style.display = "none";
}

// Guardar nueva fecha/hora
function guardarCambio() {
    citaSeleccionada.fecha = document.getElementById("nuevaFecha").value;
    citaSeleccionada.hora = document.getElementById("nuevaHora").value;

    cerrarModal();
    cargarCitas();
}

// Actualizar tarjetas
function actualizarContadores() {
    document.getElementById("pendientes").textContent = citas.filter(c => c.estado === "Pendiente").length;
    document.getElementById("completadas").textContent = citas.filter(c => c.estado === "Completada").length;
    document.getElementById("canceladas").textContent = citas.filter(c => c.estado === "Cancelada").length;
}

// Inicializar
cargarCitas();
