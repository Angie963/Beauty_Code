// Oculta los pasos
document.getElementById("paso2").style.display = "none";
document.getElementById("paso3").style.display = "none";
document.getElementById("paso4").style.display = "none";
document.getElementById("paso5").style.display = "none";
document.getElementById("paso6").style.display = "none";

// Datos ficticios
let servicios = [
    { id: 1, nombre: "Manicura en Gel" },
    { id: 2, nombre: "Uñas Acrílicas" },
    { id: 3, nombre: "Semipermanentes" }
];

let empleados = [
    { id: 1, nombre: "Camila" },
    { id: 2, nombre: "Daniela" },
    { id: 3, nombre: "Sofía" }
];

// Disponibilidad definida por admin
let disponibilidad = {
    diasHabiles: [1,2,3,4,5],   // Lunes a Viernes (0=Dom)
    horaInicio: "08:00",
    horaFin: "17:00"
};

// Cargar citas existentes o crear lista vacía
let citas = JSON.parse(localStorage.getItem("citas")) || [];

// Cargar selects dinámicamente
function cargarServicios() {
    const select = document.getElementById("servicioSelect");
    servicios.forEach(s => {
        let option = document.createElement("option");
        option.value = s.id;
        option.textContent = s.nombre;
        select.appendChild(option);
    });
}

function cargarEmpleados() {
    const select = document.getElementById("empleadoSelect");
    empleados.forEach(e => {
        let option = document.createElement("option");
        option.value = e.id;
        option.textContent = e.nombre;
        select.appendChild(option);
    });
}

// Controlador de pasos
function nextStep(step) {
    if (step === 1 && !document.getElementById("servicioSelect").value) return alert("Selecciona un servicio");
    if (step === 2 && !document.getElementById("empleadoSelect").value) return alert("Selecciona un manicurista");
    if (step === 3 && !validarFecha()) return;
    if (step === 4 && !validarHora()) return;

    document.getElementById("step" + step).classList.add("d-none");
    document.getElementById("step" + (step + 1)).classList.remove("d-none");
}

// Validar fecha según disponibilidad admin
function validarFecha() {
    let fecha = document.getElementById("fechaInput").value;
    if (!fecha) {
        alert("Selecciona una fecha");
        return false;
    }

    let date = new Date(fecha);
    let hoy = new Date();

    // No permitir días pasados
    if (date < hoy) {
        alert("La fecha no puede ser anterior a hoy");
        return false;
    }

    // No permitir agendar más de 1 año
    let max = new Date();
    max.setFullYear(max.getFullYear() + 1);

    if (date > max) {
        alert("No puedes agendar una cita con más de 1 año de anticipación");
        return false;
    }

    // Validar día hábil
    let dia = date.getDay();
    if (!disponibilidad.diasHabiles.includes(dia)) {
        alert("Ese día no está disponible. El negocio trabaja de lunes a viernes.");
        return false;
    }

    return true;
}

// Validar hora según horario admin
function validarHora() {
    let hora = document.getElementById("horaInput").value;
    if (!hora) {
        alert("Selecciona una hora");
        return false;
    }

    if (hora < disponibilidad.horaInicio || hora > disponibilidad.horaFin) {
        alert("El horario permitido es de " + disponibilidad.horaInicio + " a " + disponibilidad.horaFin);
        return false;
    }

    return true;
}

// Guardar cita
function guardarCita() {
    let servicioID = document.getElementById("servicioSelect").value;
    let empleadoID = document.getElementById("empleadoSelect").value;
    let fecha = document.getElementById("fechaInput").value;
    let hora = document.getElementById("horaInput").value;

    let cita = {
        id: Date.now(),
        servicio: servicios.find(s => s.id == servicioID).nombre,
        empleado: empleados.find(e => e.id == empleadoID).nombre,
        fecha,
        hora
    };

    citas.push(cita);
    localStorage.setItem("citas", JSON.stringify(citas));

    alert("Cita registrada con éxito");
    mostrarCitas();
}

// Mostrar citas en tabla
function mostrarCitas() {
    const tabla = document.getElementById("tablaCitas");
    tabla.innerHTML = "";

    citas.forEach(c => {
        let tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${c.servicio}</td>
            <td>${c.empleado}</td>
            <td>${c.fecha}</td>
            <td>${c.hora}</td>
            <td>
                <button class="btn btn-warning btn-sm" onclick="reagendar(${c.id})">Reagendar</button>
                <button class="btn btn-danger btn-sm" onclick="cancelar(${c.id})">Cancelar</button>
            </td>
        `;
        tabla.appendChild(tr);
    });
}

// Funciones de reagendar y cancelar
function cancelar(id) {
    citas = citas.filter(c => c.id !== id);
    localStorage.setItem("citas", JSON.stringify(citas));
    mostrarCitas();
}

function reagendar(id) {
    alert("Esta función se conectará al backend después.");
}

// Inicialización
cargarServicios();
cargarEmpleados();
mostrarCitas();
