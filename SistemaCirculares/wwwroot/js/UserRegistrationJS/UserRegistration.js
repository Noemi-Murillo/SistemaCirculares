// Write your JavaScript code.

/// <summary>
/// JavaScript para la pantalla de login
/// </summary>
/// <createdate>24/08-2025</createdate>
/// <author>Noemí Murillo</author>
/// <lastmodificationdate></lastmodificationdate>
/// <lastmodificationdescription></lastmodificationdescription>
/// <lastmodifierauthor></lastmodifierauthor>

jslogin = {

    objetos: {


    },
    controles: {

        InputEmail: '#InputEmail',
        InputPassword: '#InputPassword',

        //Modal Crear Usuario
        InputNombreR: '#txtNombreRegistro',
        InputApellido1R: '#txtApellido1Registro',
        InputApellido2R: '#txtApellido2Registro',
        InputCorreoR: '#txtCorreoRegistro',
        InputContrasenaR: '#txtContrasenaRegistro',
        InputCodigoR: '#txtCodigoRegistro',

        //Modal Recuperación de contraseña
        txtCorreoRecuperar: '#txtCorreoRecuperar'

    },

    botones: {

        btnLogin: '#btnLogin',
        BtnCrearUsuario: '#btnCrearUsuario',
        btnEnviarRecuperacion: '#btnEnviarRecuperacion'

    },

    variables: {




    },
    metodos: {

        MensajeGeneralSweetAlert: function (Icono, Mensaje, RecargaPagina, Color, TamanoLetra) {

            swal.fire({
                icon: `${Icono}`,
                html:
                    `<br/><span style='color: ${Color}; font-size: ${TamanoLetra}px; text-align: center; font-style:bold;' >${Mensaje}</span><br/><br/>`,
                width: 500,
                showCloseButton: true,
                showCancelButton: false,
                showConfirmButton: false,
                focusConfirm: false,
                allowOutsideClick: false,
                timer: 1400,
                willClose: function () {
                }
            })


            // Cerrar el mensaje y recargar la página después de 1.6 segundos
            if (RecargaPagina) {

                setTimeout(() => {
                    swal.close(); // Cierra el mensaje
                    location.reload(); // Recarga la página
                }, 1800);

            }
        },

        LogIn: function () {

            event.preventDefault();

            try {

                let User = $(jslogin.controles.InputEmail).val().trim();
                let Password = $(jslogin.controles.InputPassword).val().trim();




                // Realizar la solicitud AJAX
                $.ajax({
                    url: '../Inicio/LogIn',
                    type: 'POST',
                    data: { Email: User, Password: Password },
                    success: function (result) {


                        if (result.ok) {

                            Swal.fire({
                                title: "Éxito",
                                text: `Bienveni@: ${result.result.nombre}`,
                                icon: "success"
                            });

                            //Redirigir a la página principal después de 2 segundos
                            setTimeout(function () {
                                window.location.href = '/Home/Index';
                            }, 2100);

                        } else {
                            Swal.fire({
                                title: "Advertencia",
                                text: `${result.message}`,
                                icon: "warning"
                            });
                        }



                    },
                    error: function () {
                        // Manejo de errores si es necesario

                        Swal.fire({
                            title: "Error",
                            text: `${result.message}`,
                            icon: "error"
                        });

                    }
                });


            } catch (e) {

                console.error("Ha ocurrido un error en el método LogIn", e)

            }


        },

        RestablecerContrasena: function () {

            event.preventDefault();

            try {

                let Correo = $(jslogin.controles.txtCorreoRecuperar).val().trim();

                // Mostrar loader antes del AJAX
                Swal.fire({
                    title: 'Procesando...',
                    text: 'Por favor espera un momento',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }
                });

                $.ajax({
                    url: '../Inicio/RestablecerContrasena',
                    type: 'POST',
                    data: { Email: Correo },

                    success: function (result) {

                        Swal.close(); // 🔹 cerrar loader

                        if (result.ok) {

                            Swal.fire({
                                title: "Solicitud procesada",
                                text: "Si el usuario existe en el sistema, se enviará una contraseña temporal al correo electrónico registrado.",
                                icon: "success"
                            });

                            setTimeout(function () {
                                window.location.reload();
                            }, 2800);

                        } else {
                            Swal.fire({
                                title: "Solicitud procesada",
                                text: "Si el usuario existe en el sistema, se enviará una contraseña temporal al correo electrónico registrado.",
                                icon: "success"
                            });

                            setTimeout(function () {
                                window.location.reload();
                            }, 2800);
                        }
                    },

                    error: function () {

                        Swal.close(); // 🔹 cerrar loader

                        Swal.fire({
                            title: "Error",
                            text: "Ocurrió un error al procesar la solicitud.",
                            icon: "error"
                        });
                    }
                });

            } catch (e) {
                console.error("Ha ocurrido un error en el método RestablecerContrasena", e)
            }
        },

        CrearUsuario: function () {

            event.preventDefault();

            try {

                let Nombre = $(jslogin.controles.InputNombreR).val();
                let Apellido1 = $(jslogin.controles.InputApellido1R).val();
                let Apellido2 = $(jslogin.controles.InputApellido2R).val();
                let Correo = $(jslogin.controles.InputCorreoR).val();
                let Contrasena = $(jslogin.controles.InputContrasenaR).val();
                let Codigo = $(jslogin.controles.InputCodigoR).val();

                ObjUsuario = {
                    Nombre: Nombre,
                    Apellido1: Apellido1,
                    Apellido2: Apellido2,
                    Correo: Correo,
                    Contrasena: Contrasena,
                    CodigoRegistro: Codigo
                }

                console.log(ObjUsuario);


                if (Nombre !== "" && Apellido1 !== "" && Apellido2 !== "" && Correo !== "" && Contrasena !== "" && Codigo !== "") {


                    fetch("/GestionUsuarios/CrearUsuario", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(ObjUsuario)
                    })
                        .then(res => {
                            if (!res.ok) {
                                // Si el servidor mandó error, intento leer el json del error
                                return res.json().then(err => { throw err; });
                            }
                            return res.json();
                        })
                        .then(data => {
                            console.log("Respuesta OK:", data);

                            if (data.ok) {

                                console.log(data)
                                this.MensajeGeneralSweetAlert(
                                    'success',
                                    `${data.message}`,
                                    false,
                                    '#68AB54',
                                    30
                                );

                            } else {
                                this.MensajeGeneralSweetAlert(
                                    'warning',
                                    `${data.message}`,
                                    false,
                                    '#FF0000',
                                    26
                                );


                            }




                        })
                        .catch(err => {
                            console.error("Error del servidor o red:", err);
                        });

                } else {


                    this.MensajeGeneralSweetAlert(
                        'warning',
                        `Debe rellenar todos los espacios`,
                        false,
                        '#FF0000',
                        26
                    );


                }

            } catch (e) {

                console.error("Ha ocurrido un error en el método CrearUsuario", e)

            }


        },

    },
    eventos:
        function () {

            $(jslogin.botones.btnLogin).on('click', function () {

                jslogin.metodos.LogIn();

            });


            $(jslogin.botones.BtnCrearUsuario).on('click', function () {

                jslogin.metodos.CrearUsuario();

            });


            $(jslogin.botones.btnEnviarRecuperacion).on('click', function () {

                jslogin.metodos.RestablecerContrasena();

            });


        }

}

$(function () {
    jslogin.eventos();
});