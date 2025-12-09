// Datos ficticios
let servicios = [
    { id: 1, nombre: "Manicura en Gel", precio:50000},
    { id: 2, nombre: "Uñas Acrílicas", precio:90000},
    { id: 3, nombre: "Semipermanentes", precio:40000}
];

let empleados = [
    { id: 1, nombre: "Camila" },
    { id: 2, nombre: "Daniela" },
    { id: 3, nombre: "Sofía" }
];

// Disponibilidad definida por admin
let disponibilidad = {
    diasHabiles: [1,2,3,4,5],   // Lunes a Viernes (0=Dom)
    horaInicio: "09:00",
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

    if (step === 1 && !document.getElementById("servicioSelect").value)
        return alert("Selecciona un servicio");
    if (step === 2 && !document.getElementById("empleadoSelect").value)
        return alert("Selecciona un manicurista");
    if (step === 3 && !validarFecha()) return;
    if (step === 4 && !validarHora()) return;
    // Paso 4 genera el resumen (paso5)
    if (step === 4) {
        generarResumen();
    }
    // Paso 6 → Validar comprobante ANTES de pasar al paso7
    if (step === 6) {
        let comprobante = document.getElementById("comprobante").files[0];
        if (!comprobante) {
            alert("Debes subir el comprobante antes de continuar");
            return;
        }
        generarResumenFinal();
    }

    // Mostrar siguiente paso
    document.getElementById("paso" + (step + 1)).classList.remove("d-none");
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
    let fecha = document.getElementById("fechaInput").value;
    let empleadoID = document.getElementById("empleadoSelect").value;

    if (!hora) {
        alert("Selecciona una hora");
        return false;
    }

    if (hora < disponibilidad.horaInicio || hora > disponibilidad.horaFin) {
        alert("El horario permitido es de " + disponibilidad.horaInicio + " a " + disponibilidad.horaFin);
        return false;
    }

    if (!horaDisponible (fecha, hora, empleadoID))
    {
        alert("Ya existe una cita agendada a esta hora con esta manicurista.");
        return false;
    }

    return true;
}

function generarResumen() {
    const idServicio = document.getElementById("servicioSelect").value;
    const idEmpleado = document.getElementById("empleadoSelect").value;
    const fecha = document.getElementById("fechaInput").value;
    const hora = document.getElementById("horaInput").value;

    const servicio = servicios.find(s => s.id == idServicio);
    const empleado = empleados.find(e => e.id == idEmpleado);

    let adelanto = servicio.precio * 0.10;
    let html = `
        <p><strong>Servicio:</strong> ${servicio.nombre}</p>
        <p><strong>Especialista:</strong> ${empleado.nombre}</p>
        <p><strong>Fecha:</strong> ${fecha}</p>
        <p><strong>Hora:</strong> ${hora}</p>
        <hr>
        <p><strong>Valor del servicio:</strong> $${servicio.precio.toLocaleString()}</p>
        <p><strong>Adelanto (10%):</strong> $${adelanto.toLocaleString()}</p>
        <p><strong>Total a pagar ahora:</strong> $${adelanto.toLocaleString()}</p>
    `;

    document.getElementById("resumenBox").innerHTML = html;
}

// Guardar cita
function guardarCita() {
    let servicioID = document.getElementById("servicioSelect").value;
    let empleadoID = document.getElementById("empleadoSelect").value;
    let fecha = document.getElementById("fechaInput").value;
    let hora = document.getElementById("horaInput").value;

    const servicio = servicios.find(s => s.id == servicioID);

    let cita = {
        id: Date.now(),
        servicio: servicio.nombre,
        precio: servicio.precio,
        adelanto: servicio.precio * 0.10,
        empleado: empleados.find(e => e.id == empleadoID).nombre,
        fecha,
        hora
    };

    citas.push(cita);
    localStorage.setItem("citas", JSON.stringify(citas));

    alert("Cita registrada con éxito");
    mostrarCitas();
    limpiarFormulario();
}

function generarResumenFinal()
{
    const idServicio = document.getElementById("servicioSelect").value;
    const idEmpleado = document.getElementById("empleadoSelect").value;

    const servicio = servicios.find(s => s.id == idServicio);
    const empleado = empleados.find(e => e.id == idEmpleado);

    const fecha = document.getElementById("fechaInput").value;
    const hora = document.getElementById("horaInput").value;
    const comprobante = document.getElementById("comprobante").files[0];

    let adelanto = servicio.precio * 0.10;

    let html = `
    <p><strong>Servicio:</strong> ${servicio.nombre}</p>
    <p><strong>Especialista:</strong> ${empleado.nombre}</p>
    <p><strong>Fecha:</strong> ${fecha}</p>
    <p><strong>Hora:</strong> ${hora}</p>
    <p><strong>Comprobante:</strong> ${comprobante ? comprobante.name : "No cargado"}</p>
    <hr>
    <p><strong>Valor del servicio:</strong> $${servicio.precio.toLocaleString()}</p>
    `;

    document.getElementById("resumenFinal").innerHTML = html;
}

function generarHorasDisponibles()
{
    const contenedor = document.getElementById("horaContainer");
    contenedor.innerHTML = "";
    const fecha = document.getElementById("fechaInput").value;
    const empleadoID = document.getElementById("empleadoSelect").value;

    let inicio = parseInt(disponibilidad.horaInicio.split(":")[0]);
    let fin = parseInt(disponibilidad.horaFin.split(":")[0]);

    for (let h = inicio; h <= fin; h++)
    {
        let hora = (h < 10 ? "0" : "") + h + ":00";

        let ocupada = cita.some(c =>
            c.fecha === fecha &&
            c.hora === hora &&
            c.empleado === empleados.find(e => e.id == empleadoID).nombre
        );

        let btn = document.createElement("button");
        btn.textContent = hora;

        btn.className = "btn m-1 " + (ocupada ? "btn-secondary" : "btn-primary");
        btn.disabled = ocupada;

        if (!ocupada)
        {
            btn.onclick = function ()
            {
                document.getElementById("horaInput").value = hora;
            };
        }
        contenedor.appendChild(btn);
    }
}

function limpiarFormulario()
{
    document.getElementById("servicioSelect").value = "";
    document.getElementById("empleadoSelect").value = "";
    document.getElementById("fechaInput").value = "";
    document.getElementById("horaInput").value = "";
    document.getElementById("comprobante").value = "";

    // Ocultar pasos de nuevo
    for (let i=2; i<=7; i++)
    {
        document.getElementById("paso" + i).classList.add("d-none");
    }
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
                <button class="btn-primary" style="background:#F07A4B; margin-top:0" onclick="reagendar(${c.id})">Reagendar</button>
                <button class="btn-primary" onclick="cancelar(${c.id})">Cancelar</button>
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

function horaDisponible(fecha, hora, empleadoID)
{
    return !citas.some(c =>
        c.fecha === fecha &&
        c.hora === hora &&
        c.empleado === empleados.find(e => e.id == empleadoID).nombre
    );
}

// Inicialización
cargarServicios();
cargarEmpleados();
mostrarCitas();
