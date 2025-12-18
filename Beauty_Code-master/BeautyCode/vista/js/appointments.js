const API_URL = "http://localhost:5272/api";

let citaEnEdicion = null;
let servicios = [];
let citas = [];

let empleados = [
    { id: 1, nombre: "Camila" },
    { id: 2, nombre: "Daniela" },
    { id: 3, nombre: "Sofía" }
];

function esReagendar()
{
    return citaEnEdicion !== null;
}

async function cargarCitas() {
    try {
        const res = await fetch(`http://localhost:5272/api/Agenda`);
        if (!res.ok) throw new Error("No se pudo cargar citas");

        citas = await res.json();
        mostrarCitas();

    } catch (error) {
        console.error(error);
    }
}

// Disponibilidad definida por admin
let disponibilidad = {
    diasHabiles: [1,2,3,4,5],   // Lunes a Viernes (0=Dom)
    horaInicio: "09:00",
    horaFin: "17:00"
};

// Cargar selects dinámicamente
async function cargarServicios() {
    const select = document.getElementById("servicioSelect");
    select.innerHTML = `<option value="">Seleccione...</option>`;

    try {
        const res = await fetch(`http://localhost:5272/api/servicios`);
        if (!res.ok) throw new Error("Error obteniendo servicios");

        const data = await res.json();

        data.forEach(s => {
            let option = document.createElement("option");
            option.value = s.id;
            option.textContent = `${s.nombre} ($${s.precio.toLocaleString()})`;
            select.appendChild(option);
        });

        // Guardarlo en una variable global para usar en el resumen
        servicios = data;

    } catch (err) {
        console.error(err);
        alert("Error cargando servicios desde el servidor");
    }
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

    // VALIDACIONES SOLO PARA CITA NUEVA
    if (!esReagendar()) {

        if (step === 1 && !document.getElementById("servicioSelect").value)
            return alert("Selecciona un servicio");

        if (step === 2 && !document.getElementById("empleadoSelect").value)
            return alert("Selecciona un manicurista");

        if (step === 3)
        {
            if (!validarFecha()) return;
            generarHorasDisponibles();
        }

        if (step === 4 && !validarHora()) return;

        if (step === 4) generarResumen();

        if (step === 6) {
            let comprobante = document.getElementById("comprobante").files[0];
            if (!comprobante) {
                alert("Debes subir el comprobante");
                return;
            }
            generarResumenFinal();
        }

    } 
    // VALIDACIONES PARA REAGENDAR
    else {

        if (step === 2 && !document.getElementById("empleadoSelect").value)
            return alert("Selecciona un manicurista");

        if (step === 3 && !validarFecha()) return;

        if (step === 4 && !validarHora()) return;

        if (step === 4) generarResumenFinal();
    }

    document.getElementById("paso" + (step + 1)).classList.remove("d-none");
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

function validarFecha() {
    const fecha = document.getElementById("fechaInput").value;

    if (!fechaInput) {
        alert("Selecciona una fecha");
        return false;
    }

    return true;
}

function generarResumen() {
    const idServicio = document.getElementById("servicioSelect").value;
    const idEmpleado = document.getElementById("empleadoSelect").value;
    const fecha = document.getElementById("fechaInput").value;
    const hora = document.getElementById("horaInput").value;

    const servicio = servicios.find(s => s.id === idServicio);
    const empleado = empleados.find(e => String(e.id) === String(idEmpleado));

    if (!servicio || !empleado) {
        alert("Error cargando datos del servicio o especialista");
        console.error("Servicio:", servicio, "Empleado:", empleado);
        return;
    }

    let adelanto = servicio.precio * 0.10;

    let html = `
        <p><strong>Servicio:</strong> ${servicio.nombre}</p>
        <p><strong>Especialista:</strong> ${empleado.nombre}</p>
        <p><strong>Fecha:</strong> ${fecha}</p>
        <p><strong>Hora:</strong> ${hora}</p>
        <hr>
        <p><strong>Valor del servicio:</strong> $${servicio.precio.toLocaleString()}</p>
        <p><strong>Adelanto (10%):</strong> $${adelanto.toLocaleString()}</p>
    `;

    document.getElementById("resumenBox").innerHTML = html;
}

// Guardar cita
async function guardarCita() {

    const empleadoID = document.getElementById("empleadoSelect").value;
    const fechaInput = document.getElementById("fechaInput").value;
    const hora = document.getElementById("horaInput").value;

    const fechaISO = new Date(`${fechaInput}T${hora}:00`).toISOString();

    let url = "http://localhost:5272/api/Agenda";
    let method = "POST";

    let cita = {
        fecha: fechaISO,
        hora: hora,
        empleadoId: empleadoID
    };

    // SI ES REAGENDAR
    if (citaEnEdicion) {
        url += `/${citaEnEdicion.id}`;
        method = "PUT";

        cita = {
            ...citaEnEdicion,
            fecha: fechaISO,
            hora: hora,
            empleadoId: empleadoID
        };
    } 
    // CITA NUEVA
    else {
        const servicioID = document.getElementById("servicioSelect").value;
        const servicio = servicios.find(s => s.id === servicioID);

        cita = {
            fecha: fechaISO,
            hora,
            servicioId: servicioID,
            empleadoId: empleadoID,
            precioServicio: servicio.precio,
            estado: "Pendiente",
            pagoAgenda: servicio.precio * 0.10,
            usuarioId: "675aeb8ac8dc2fcebd76f122",
            creadoEn: new Date().toISOString()
        };
    }

    const res = await fetch(url, {
        method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(cita)
    });

    if (!res.ok) {
        alert("Error guardando la cita");
        return;
    }

    alert(citaEnEdicion ? "Cita reagendada con éxito" : "Cita registrada con éxito");

    citaEnEdicion = null;
    limpiarFormulario();
    mostrarPasoInicial();
    cargarCitas();
}

function generarResumenFinal() {

    let servicio;
    let empleado;

    const idEmpleado = document.getElementById("empleadoSelect").value;
    empleado = empleados.find(e => String(e.id) === String(idEmpleado));

    // SI ES CITA NUEVA
    if (!citaEnEdicion) {
        const idServicio = document.getElementById("servicioSelect").value;
        servicio = servicios.find(s => String(s.id) === String(idServicio));
    } 
    // SI ES REAGENDAR
    else {
        servicio = servicios.find(s => String(s.id) === String(citaEnEdicion.servicioId));
    }

    if (!servicio || !empleado) {
        console.error("Servicio:", servicio);
        console.error("Empleado:", empleado);
        return;
    }

    const fecha = document.getElementById("fechaInput").value;
    const hora = document.getElementById("horaInput").value;

    let html = `
        <p><strong>Servicio:</strong> ${servicio.nombre}</p>
        <p><strong>Especialista:</strong> ${empleado.nombre}</p>
        <p><strong>Fecha:</strong> ${fecha}</p>
        <p><strong>Hora:</strong> ${hora}</p>
    `;

    // SOLO PARA CITA NUEVA
    if (!citaEnEdicion) {
        const comprobante = document.getElementById("comprobante").files[0];
        let adelanto = servicio.precio * 0.10;

        html += `
            <p><strong>Comprobante:</strong> ${comprobante.name}</p>
            <hr>
            <p><strong>Valor del servicio:</strong> $${servicio.precio.toLocaleString()}</p>
            <p><strong>Adelanto:</strong> $${adelanto.toLocaleString()}</p>
        `;
    }

    document.getElementById("resumenFinal").innerHTML = html;
}

function generarHorasDisponibles()
{
    const contenedor = document.getElementById("horaContainer");
    if (!contenedor) return;
    contenedor.innerHTML = "";
    const fecha = document.getElementById("fechaInput").value;
    const empleadoID = document.getElementById("empleadoSelect").value;

    if (!fecha || !empleadoID) return;

    let inicio = parseInt(disponibilidad.horaInicio.split(":")[0]);
    let fin = parseInt(disponibilidad.horaFin.split(":")[0]);

    for (let h = inicio; h <= fin; h++)
    {
        let hora = (h < 10 ? "0" : "") + h + ":00";

        let ocupada = citas.some(c =>
            c.fecha.split("T")[0] === fecha &&
            c.hora === hora &&
            String(c.empleadoId) === String(empleadoID)
        );

        let btn = document.createElement("button");
        btn.textContent = hora;
        btn.className = "hora-btn" + (ocupada ? " ocupada" : "");
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
        const servicio = servicios.find(s => s.id === c.servicioId);
        const empleado = empleados.find(e => e.id.toString() === c.empleadoId);

        let tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${servicio ? servicio.nombre : "N/A"}</td>
            <td>${empleado ? empleado.nombre : "N/A"}</td>
            <td>${c.fecha.split("T")[0]}</td>
            <td>${c.hora}</td>
            <td>
                <button class="btn-primary" style="background:#F07A4B; margin-top:0" onclick="reagendar('${c.id}')">Reagendar</button>
                <button class="btn-primary" onclick="cancelar('${c.id}')">Cancelar</button>
            </td>
        `;
        tabla.appendChild(tr);
    });
}

// Funciones de reagendar y cancelar
async function cancelar(id) {
const res = await fetch(`http://localhost:5272/api/Agenda/${id}`, {method: "DELETE"});

    if (!res.ok) {
        alert("Error al cancelar cita");
        return;
    }

    cargarCitas();
}

function reagendar(id) {

    citaEnEdicion = citas.find(c => c.id === id);
    if (!citaEnEdicion) {
        alert("Cita no encontrada");
        return;
    }

    // Cargar datos actuales
    document.getElementById("empleadoSelect").value = citaEnEdicion.empleadoId;
    document.getElementById("fechaInput").value = citaEnEdicion.fecha.split("T")[0];
    document.getElementById("horaInput").value = citaEnEdicion.hora;

    generarHorasDisponibles();

    // OCULTAR pasos que NO deben salir
    document.getElementById("paso1").classList.add("d-none"); // servicio
    document.getElementById("paso5").classList.add("d-none"); // resumen normal
    document.getElementById("paso6").classList.add("d-none"); // comprobante

    // MOSTRAR solo lo necesario
    document.getElementById("paso2").classList.remove("d-none"); // manicurista
    document.getElementById("paso3").classList.remove("d-none"); // fecha
    document.getElementById("paso4").classList.remove("d-none"); // hora
    document.getElementById("paso7").classList.remove("d-none"); // confirmar

    generarResumenFinal();
}

function horaDisponible(fecha, hora, empleadoID) {
    return !citas.some(c =>
        c.fecha.split("T")[0] === fecha &&
        c.hora === hora &&
        String(c.empleadoId) === String(empleadoID)
    );
}

function mostrarPasoInicial() {
    // Mostrar paso 1
    document.getElementById("paso1").classList.remove("d-none");

    // Ocultar todos los demás pasos
    for (let i = 2; i <= 7; i++) {
        document.getElementById("paso" + i).classList.add("d-none");
    }
}

// Inicialización
cargarServicios();
cargarEmpleados();
cargarCitas();