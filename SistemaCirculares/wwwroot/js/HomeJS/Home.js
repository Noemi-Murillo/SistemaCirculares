// Write your JavaScript code.

/// <summary>
/// JavaScript para la pantalla de login
/// </summary>
/// <createdate>24/08-2025</createdate>
/// <author>Noemí Murillo</author>
/// <lastmodificationdate></lastmodificationdate>
/// <lastmodificationdescription></lastmodificationdescription>
/// <lastmodifierauthor></lastmodifierauthor>

jsHome = {

    objetos: {

        ListaUsuarios: []



    },
    controles: {

        //Modal Generar código
        FechaLimiteCodigo: '#fechaLimite',
   






    },

    botones: {
        //Modal Generar código
        BtnVerCircular: '.ver-circular',
   

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


        ObtenerCircularPorId: function (event) {

            let button = $(event.currentTarget);
            let IdCircular = button.data('idcircular')
            console.log(IdCircular)


            fetch("/Home/ObtenerCircularesPorId", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(IdCircular)
            })
                .then(res => {
                    if (!res.ok) {
                        // Si el servidor mandó error, intento leer el json del error
                        return res.json().then(err => { throw err; });
                    }
                    return res.json();
                })
                .then(data => {

                   
                    console.log(data)

                    // 1) Tomar el base64 de la respuesta
                    var b64 = (data && (data.archivoBytes || (data.result && data.result.archivoBytes) || data.ArchivoBytes)) || '';
                    if (!b64) { throw new Error('No vino archivoBytes en la respuesta'); }

                    // 2) Limpiar prefijo y espacios
                    b64 = String(b64).trim();
                    if (b64.indexOf(',') !== -1) b64 = b64.split(',')[1]; // si venía "data:application/pdf;base64,XXXX"
                    b64 = b64.replace(/\s/g, ''); // quitar saltos/espacios

                    // 3) Base64 -> bytes (sincrónico)
                    function b64ToBlob(base64, contentType) {
                        contentType = contentType || 'application/pdf';
                        var byteChars = atob(base64);
                        var len = byteChars.length;
                        var bytes = new Uint8Array(len);
                        for (var i = 0; i < len; i++) {
                            bytes[i] = byteChars.charCodeAt(i);
                        }
                        return new Blob([bytes], { type: contentType });
                    }

                    var blob = b64ToBlob(b64, 'application/pdf');
                    var url = URL.createObjectURL(blob);

                    // 4) Pintar en el modal
                    var iframe = document.getElementById('pdfViewer');
                    var aDesc = document.getElementById('btnDescargarPdf');

                    iframe.src = url;
                    aDesc.href = url;
                    aDesc.download = (data.nombreArchivo || data.fileName || 'documento.pdf');

                    // 5) Abrir modal (Bootstrap 5) y limpiar al cerrar
                    var modalEl = document.getElementById('pdfModal');
                    var modal = new bootstrap.Modal(modalEl);
                    modal.show();

                    modalEl.addEventListener('hidden.bs.modal', function () {
                        try {
                            iframe.src = 'about:blank';
                            aDesc.removeAttribute('href');
                            aDesc.removeAttribute('download');

                            if (typeof url !== 'undefined' && url) {
                                URL.revokeObjectURL(url);
                            }
                        } catch (err) {
                            console.warn("Error limpiando URL del PDF:", err);
                        }

                        // 🔧 Elimina manualmente el backdrop si quedó pegado
                        const backdrops = document.querySelectorAll('.modal-backdrop');
                        backdrops.forEach(b => b.remove());

                        document.body.classList.remove('modal-open');
                        document.body.style.removeProperty('overflow');
                        document.body.style.removeProperty('padding-right');
                    });           



                })
                .catch(err => {
                    console.error("Error del servidor o red:", err);
                });




        },



        GenerarCodigoRegistro: function () {

            event.preventDefault();

            try {

                let FechaLimite = $(jsHome.controles.FechaLimiteCodigo).val();

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

                            $(jsHome.controles.InputCodigoGenerado).val(data.result);
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

                //        $(jsHome.controles.InputCodigoGenerado).val(data.result);
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
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(1)
                })
                    .then(res => {
                        if (!res.ok) return res.json().then(err => { throw err; });
                        return res.json();
                    })
                    .then(data => {
                        const selects = document.querySelectorAll(".ComitesSelect");

                        for (const select of selects) {
                            // 0) Memoriza el valor actual (o un data-atributo que traigas del server)
                            const currentValue =
                                (select.value ?? "") ||
                                select.getAttribute("data-current") || ""; // opcional

                            // 1) Asegura placeholder 0 sin duplicar
                            let opt0 = select.querySelector('option[value="0"]');
                            if (!opt0) {
                                opt0 = new Option("— Seleccione una opción —", "0");
                                select.insertBefore(opt0, select.firstChild);
                            }

                            // 2) Índice de opciones existentes para no duplicar
                            const existentes = new Set([...select.options].map(o => String(o.value)));

                            // 3) Agrega comités
                            for (const { idComite, nombreComite } of (data?.result ?? [])) {
                                const val = String(idComite);
                                if (val && !existentes.has(val)) {
                                    select.add(new Option(nombreComite, val));
                                    existentes.add(val);
                                }
                            }

                            // 4) Restaurar selección si existe; si no, caer a 0
                            if (currentValue && existentes.has(String(currentValue))) {
                                select.value = String(currentValue);
                            } else if (!select.value || !existentes.has(String(select.value))) {
                                select.value = "0";
                            }
                        }
                    })
                    .catch(err => {
                        console.error("Error del servidor o red:", err);
                    });
            } catch (e) {
                console.error("Ha ocurrido un error en el método CargarComites", e);
            }
        },



        CargarDatatableGestionUsuarios: function () {
            $(jsHome.tablas.TablaGestionUsuarios).DataTable({
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
            jsHome.objetos.ListaUsuarios = [];

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

                jsHome.objetos.ListaUsuarios.push({
                    Id,
                    Nombre,
                    IdComite,
                    NombreComite,
                    EsCordinador,
                    Activo
                });

            });

            Swal.fire({
                title: '¿Estás seguro?',
                text: 'Se actualizarán los datos de los usuarios.',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'Sí, actualizar',
                cancelButtonText: 'No, cancelar',
                confirmButtonColor: '#2ad765',
                reverseButtons: true,
                allowOutsideClick: false
            }).then(result => {
                if (!result.isConfirmed) return;




                fetch("/GestionUsuarios/GuardarUsuario", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(jsHome.objetos.ListaUsuarios)
                })
                    .then(res => {
                        if (!res.ok) {
                            // Si el servidor mandó error, intento leer el json del error
                            return res.json().then(err => { throw err; });
                        }
                        return res.json();
                    })
                    .then(data => {


                        if (data.ok) {

                            this.MensajeGeneralSweetAlert(
                                'success',
                                `${data.message}`,
                                false,
                                '#68AB54',
                                30
                            );

                            setTimeout(function () {
                                window.location.reload();
                            }, 2000);


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





            })
                .catch(err => {
                    console.error("Error del servidor o red:", err);
                });






        },



    },
    eventos:
        function () {

            $(jsHome.botones.BtnVerCircular).on('click', function (event) {

                jsHome.metodos.ObtenerCircularPorId(event);

            });


            $(jsHome.botones.BtnGuardarUsuarios).on('click', function () {

                jsHome.metodos.LeerTablaUsuarios();

            });


        }




}

$(function () {
    jsHome.eventos();
    jsHome.metodos.CargarComites();
    jsHome.metodos.CargarDatatableGestionUsuarios();
});