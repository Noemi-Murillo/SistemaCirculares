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

        ListaUsuarios: []



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
        BtnEnviarCorreo: '#btnEnviar',
        BtnGuardarUsuarios: '#btnguardarUsuarios'
    },

    tablas: {

        TablaGestionUsuarios: '#TbGestionUsuarios'


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


        },


        CargarComites: function () {


            try {

                fetch("/GestionUsuarios/ObtenerComites", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(1)
                })
                    .then(res => {
                        if (!res.ok) {
                            // Si el servidor mandó error, intento leer el json del error
                            return res.json().then(err => { throw err; });
                        }
                        return res.json();
                    })
                    .then(data => {
                        const selects = document.querySelectorAll(".ComitesSelect");

                        for (const select of selects) {
                            const existentes = new Set([...select.options].map(o => o.value));
                            for (const { idComite, nombreComite } of data.result) {
                                const val = String(idComite);
                                if (val && !existentes.has(val)) {
                                    select.add(new Option(nombreComite, val));
                                    existentes.add(val);
                                }
                            }
                        }




                    })
                    .catch(err => {
                        console.error("Error del servidor o red:", err);
                    });





            } catch (e) {
                console.error("Ha ocurrido un error en el método CargarComites", e)

            }




        },

        CargarDatatableGestionUsuarios: function () {
            $(jsGestionUsuarios.tablas.TablaGestionUsuarios).DataTable({
                ordering: true,
                columnDefs: [
                    {
                        targets: [2, 3], // Aplica a todas las columnas
                        orderable: false // Las desactiva...
                    },
                    {
                        //targets: [0, 1], // ...excepto esta
                        //orderable: true
                    }
                ],// 🔒 Desactiva el ordenamiento en todas las columnas
                paging: true,
                pageLength: 5,
                autoWidth: true,
                dom: 'Brtip',
                lengthMenu: [
                    [5, 10, 20, -1],
                    ['5 elem', '10 elem', '20 elem', 'Mostrar todos']
                ],
                buttons: [],
                language: {
                    paginate: {
                        previous: "<i style='font-size: 18px; color:#B2B6BF;' class='fa fa-chevron-left' aria-hidden='true'></i>",
                        next: "<i style='font-size: 18px; color:#B2B6BF;' class='fa fa-chevron-right' aria-hidden='true'></i>"
                    },
                    zeroRecords: 'No se encontraron datos'
                }
            });
        },
        RealizarBusquedaPersonalizada: function () {

            var valor = $(jsCanastasBasicas.controles.InputBuscar).val();
            var tabla = $(jsCanastasBasicas.tablas.TbGastosDependencia).DataTable();
            tabla.search(valor).draw();


        },

        LeerTablaUsuarios: function () {
            jsGestionUsuarios.objetos.ListaUsuarios = [];

            $('#TbGestionUsuarios tbody tr').each(function () {
                const $td = $(this).children('td');

                // Saltar la fila "No hay registros" (colspan) u otras filas inválidas
                if ($td.length < 5) return;

                const Id = $.trim($td.eq(0).text());
                const Nombre = $.trim($td.eq(1).text());

                const $select = $td.eq(2).find('select.ComitesSelect');
                const IdComite = $select.val(); // string; usa parseInt si requieres número
                const NombreComite = ($select.find('option:selected').text() || '').trim();

                const EsCordinador = $td.eq(3).find('input[type=checkbox]').prop('checked') === true;
                const Activo = $td.eq(4).find('input[type=checkbox]').prop('checked') === true;

                jsGestionUsuarios.objetos.ListaUsuarios.push({
                    Id,
                    Nombre,
                    IdComite,
                    NombreComite,
                    EsCordinador,
                    Activo
                });
           
            });




            fetch("/GestionUsuarios/GuardarUsuario", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(jsGestionUsuarios.objetos.ListaUsuarios)
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
                    return;
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






        },





    },
    eventos:
        function () {

            $(jsGestionUsuarios.botones.BtnGenerarCodigo).on('click', function () {

                jsGestionUsuarios.metodos.GenerarCodigoRegistro();

            });


            $(jsGestionUsuarios.botones.BtnGuardarUsuarios).on('click', function () {

                jsGestionUsuarios.metodos.LeerTablaUsuarios();

            });


        }

}

$(function () {
    jsGestionUsuarios.eventos();
    jsGestionUsuarios.metodos.CargarComites();
    jsGestionUsuarios.metodos.CargarDatatableGestionUsuarios();
});