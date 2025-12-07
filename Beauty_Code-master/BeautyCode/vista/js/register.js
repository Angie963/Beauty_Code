document.getElementById("registerForm").addEventListener("submit", function(event) //Selecciona el formulario y "escucha" el evento del envio 
        {
            const password = document.getElementById("password").value;
            const confirmpassword = document.getElementById("confirmpassword").value;
            const errorSpan = document.getElementById("error-password");

            if (password !== confirmpassword) {
                event.preventDefault(); //Hace que no se envie el formulario si no se cumple
                errorSpan.textContent = "Las contraseñas no coinciden."; //Mensaje de error
            } else {
                errorSpan.textContent = ""; //Si coinciden limpia el mensaje y envia el formulario
            }
        });