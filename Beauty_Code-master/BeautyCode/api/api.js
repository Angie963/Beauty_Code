const API_URL = "http://localhost:5272/api";

/* ============================
        USERS
============================ */

// Obtener todos los usuarios
export async function getUsers() {
    const res = await fetch(`${API_URL}/Users`);
    return await res.json();
}

// Crear usuario
export async function createUser(userData) {
    const res = await fetch(`${API_URL}/Users`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userData)
    });
    return await res.json();
}


/* ============================
        APPOINTMENTS (CITAS)
============================ */

// Obtener todas las citas
export async function getAppointments() {
    const res = await fetch(`${API_URL}/Agenda`);
    return await res.json();
}

// Crear cita
export async function createAppointment(data) {
    const res = await fetch(`${API_URL}/Agenda`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return await res.json();
}


/* ============================
        SERVICES (SERVICIOS)
============================ */

export async function getServices() {
    const res = await fetch(`${API_URL}/Servicios`);
    return await res.json();
}


/* ============================
        CATEGORIES (CATEGORÍAS)
============================ */

export async function getCategories() {
    const res = await fetch(`${API_URL}/Categorias`);
    return await res.json();
}


/* ============================
        PAYMENTS (PAGOS)
============================ */

export async function getPayments() {
    const res = await fetch(`${API_URL}/Pagos`);
    return await res.json();
}

export async function createPayment(data) {
    const res = await fetch(`${API_URL}/Pagos`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return await res.json();
}


/* ============================
        REVIEWS (RESEÑAS)
============================ */

export async function getReviews() {
    const res = await fetch(`${API_URL}/Resenas`);
    return await res.json();
}

export async function createReview(data) {
    const res = await fetch(`${API_URL}/Resenas`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(data)
    });
    return await res.json();
}