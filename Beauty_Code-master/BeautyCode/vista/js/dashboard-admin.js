// Datos simulados (el backend luego los reemplaza)
let servicios = 8;
let empleados = 3;
let imagenes = 14;

let citas = [
    { id: 1, cliente: "Laura Pérez", servicio: "Uñas Acrílicas", empleado: "Camila", fecha: "2025-12-10", hora: "10:00", estado: "Pendiente" },
    { id: 2, cliente: "Sofía Ríos", servicio: "Gel", empleado: "Daniela", fecha: "2025-12-11", hora: "02:00", estado: "Completada" },
    { id: 3, cliente: "María Gómez", servicio: "Spa", empleado: "Camila", fecha: "2025-12-11", hora: "04:00", estado: "Cancelada" }
];

// Cargar estadísticas
document.getElementById("totalServicios").textContent = servicios;
document.getElementById("totalEmpleados").textContent = empleados;
document.getElementById("totalImagenes").textContent = imagenes;

// Tabla de citas
function cargarCitas() {
    const tbody = document.getElementById("tablaCitasAdmin");
    tbody.innerHTML = "";

    citas.forEach(c => {
        let badge = `<span class="badge bg-warning">Pendiente</span>`;
        if (c.estado === "Completada") badge = `<span class="badge bg-success">Completada</span>`;
        if (c.estado === "Cancelada") badge = `<span class="badge bg-danger">Cancelada</span>`;

        const fila = document.createElement("tr");

        fila.innerHTML = `
            <td>${c.cliente}</td>
            <td>${c.servicio}</td>
            <td>${c.empleado}</td>
            <td>${c.fecha}</td>
            <td>${c.hora}</td>
            <td>${badge}</td>
            <td>
                <button class="btn btn-success btn-sm" onclick="marcarCompletada(${c.id})">✔</button>
                <button class="btn btn-danger btn-sm" onclick="cancelar(${c.id})">✘</button>
                <button class="btn btn-warning btn-sm" onclick="reagendar(${c.id})">⟳</button>
            </td>
        `;

        tbody.appendChild(fila);
    });

    document.getElementById("totalCitas").textContent = citas.length;
}

function marcarCompletada(id) {
    let c = citas.find(x => x.id === id);
    c.estado = "Completada";
    cargarCitas();
}

function cancelar(id) {
    let c = citas.find(x => x.id === id);
    c.estado = "Cancelada";
    cargarCitas();
}

function reagendar(id) {
    alert("Esta función se conectará al backend después.");
}

cargarCitas();
