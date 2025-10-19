// Write your JavaScript code.

/// <summary>
/// JavaScript para la pantalla de login
/// </summary>
/// <createdate>24/08-2025</createdate>
/// <author>Noemí Murillo</author>
/// <lastmodificationdate></lastmodificationdate>
/// <lastmodificationdescription></lastmodificationdescription>
/// <lastmodifierauthor></lastmodifierauthor>

jsGestionUsuarios = {

    objetos: {


    },
    controles: {

        //Modal Generar código
        FechaLimiteCodigo: '#fechaLimite',
        InputEmailEnviar: '#emailDestino',
        InputCodigoGenerado: '#txtCodigo'

    },

    botones: {
        //Modal Generar código
        BtnGenerarCodigo: '#btnGenerar',
        BtnEnviarCorreo: '#btnEnviar'
    },

    variables: {




    },
    metodos: {



        //Función de mensaje general, reutilizar para evitar la gran duplicidad existente de código basura
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


        GenerarCodigoRegistro: function () {

            event.preventDefault();

            try {

                let FechaLimite = $(jsGestionUsuarios.controles.FechaLimiteCodigo).val();

                ObjUsuario = {

                    FechaExpiracion: FechaLimite

                }


                if (FechaLimite !== "" && FechaLimite !== null && FechaLimite !== undefined) {


                    console.log(ObjUsuario)
                    fetch("/GestionUsuarios/GenerarCodigoRegistro", {
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

                            $(jsGestionUsuarios.controles.InputCodigoGenerado).val(data.result);
                            navigator.clipboard.writeText(data.result);
                            msgCopiado.classList.remove('d-none');
                            setTimeout(() => msgCopiado.classList.add('d-none'), 1800);
                            console.log(data)
                            this.MensajeGeneralSweetAlert(
                                'success',
                                `${data.message}`,
                                false,
                                '#68AB54',
                                30
                            );



                        })
                        .catch(err => {
                            console.error("Error del servidor o red:", err);
                        });

                } else {


                    this.MensajeGeneralSweetAlert(
                        'warning',
                        `Debe seleccionar una fecha límite para generar el código`,
                        false,
                        '#FF0000',
                        26
                    );


                }

                //fetch("/GestionUsuarios/GenerarCodigoRegistro", {
                //    method: "GET",
                //    cache: "no-store"
                //})
                //    .then(respuesta => {
                //        if (!respuesta.ok) throw new Error("HTTP " + r.status);
                //        return respuesta.json();
                //    })
                //    .then(data => {

                //        $(jsGestionUsuarios.controles.InputCodigoGenerado).val(data.result);
                //        navigator.clipboard.writeText(data.result);
                //        msgCopiado.classList.remove('d-none');
                //        setTimeout(() => msgCopiado.classList.add('d-none'), 1800);
                //        console.log(data)
                //        this.MensajeGeneralSweetAlert(
                //            'success',
                //            `${data.message}`,
                //            false,
                //            '#68AB54',
                //            30
                //        );



                //    })
                //    .catch((err) => {


                //        this.MensajeGeneralSweetAlert(
                //            'error',
                //            `Ha ocurrido un error> ${err}`,
                //            false,
                //            '#FF0000',
                //            26
                //        );



                //    })







            } catch (e) {

                console.error("Ha ocurrido un error en el método GenerarCodigoRegistro", e)

            }


        }


    },
    eventos:
        function () {

            $(jsGestionUsuarios.botones.BtnGenerarCodigo).on('click', function () {

                jsGestionUsuarios.metodos.GenerarCodigoRegistro();

            });


        }

}

$(function () {
    jsGestionUsuarios.eventos();
});