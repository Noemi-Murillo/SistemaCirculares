// Write your JavaScript code.

/// <summary>
/// JavaScript para la pantalla de login
/// </summary>
/// <createdate>24/08-2025</createdate>
/// <author>Noemí Murillo</author>
/// <lastmodificationdate></lastmodificationdate>
/// <lastmodificationdescription></lastmodificationdescription>
/// <lastmodifierauthor></lastmodifierauthor>

jsCalendario = {

    objetos: {
        ListaUsuarios: [],
        CalendarioInstance: null // Guardamos la instancia del calendario
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

    variables: {},

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

        /**
         * Obtiene los eventos del calendario desde el servidor
         * @returns {Promise} Promesa con los eventos del calendario
         */
        ObtenerEventosCalendario: function () {
            return fetch("/Calendario/ObtenerEventos", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({}) // Puedes enviar filtros si es necesario
            })
                .then(res => {
                    if (!res.ok) {
                        return res.json().then(err => { throw err; });
                    }
                    return res.json();
                })
                .then(data => {
                    console.log("Eventos obtenidos:", data);
                    
                    if (data.ok && data.result) {
                        return this.FormatearEventosParaCalendario(data.result);
                    } else {
                        throw new Error(data.message || "Error al obtener eventos");
                    }
                })
                .catch(err => {
                    console.error("Error al obtener eventos del calendario:", err);
                    this.MensajeGeneralSweetAlert(
                        'error',
                        `Error al cargar eventos: ${err.message || err}`,
                        false,
                        '#FF0000',
                        26
                    );
                    return []; // Retorna array vacío en caso de error
                });
        },

        /**
         * Formatea los eventos del servidor al formato requerido por FullCalendar
         * @param {Array} eventos - Array de eventos desde el servidor
         * @returns {Array} Array formateado para FullCalendar
         */
        FormatearEventosParaCalendario: function (eventos) {
            try {
                if (!Array.isArray(eventos)) return [];

                return eventos
                    .filter(e => e && e.fecha && e.titulo) // evita eventos incompletos
                    .map(e => {
                        // FullCalendar acepta Date, ISO string, o "YYYY-MM-DD"
                        // Aquí tu fecha viene como "2026-02-28T00:00:00"
                        const start = e.fecha;

                        return {
                            id: e.idCircular,                 // ✅ camelCase
                            title: e.titulo,                  // ✅ camelCase
                            start: start,                     // ✅ camelCase
                            // Si son eventos de 1 día, puedes omitir end.
                            // Si quisieras end, lo calculas o lo agregas desde tu API.
                            backgroundColor: e.color || '#3788d8',
                            borderColor: e.color || '#3788d8',
                            allDay: true, // como es 00:00:00, normalmente es todo el día
                            extendedProps: {
                                nombreComite: e.nombreComite || '',
                                idCircular: e.idCircular
                            }
                        };
                    });
            } catch (e) {
                console.error("Error al formatear eventos:", e, eventos);
                return [];
            }
        },

        /**
         * Inicializa el calendario FullCalendar con los eventos
         */
        InicializarCalendario: function () {
            try {
                var calendarEl = document.getElementById('calendar');
                
                if (!calendarEl) {
                    console.error("No se encontró el elemento #calendar");
                    return;
                }

                var calendar = new FullCalendar.Calendar(calendarEl, {
                    initialView: 'dayGridMonth',
                    locale: 'es',
                    headerToolbar: {
                        left: 'prev,next today',
                        center: 'title',
                        right: 'dayGridMonth,timeGridWeek,timeGridDay'
                    },
                    buttonText: {
                        today: 'Hoy',
                        month: 'Mes',
                        week: 'Semana',
                        day: 'Día'
                    },
                    events: (info, successCallback, failureCallback) => {
                        // Carga los eventos de forma dinámica
                        this.ObtenerEventosCalendario()
                            .then(eventos => {
                                successCallback(eventos);
                            })
                            .catch(err => {
                                failureCallback(err);
                            });
                    },
                    eventClick: (info) => {
                        // Maneja el clic en un evento
                        this.MostrarDetalleEvento(info.event);
                    },
                    eventDidMount: (info) => {
                        // Agrega tooltip al evento
                        info.el.title = info.event.extendedProps.descripcion || info.event.title;
                    }
                });

                calendar.render();
                jsCalendario.objetos.CalendarioInstance = calendar;
                
            } catch (e) {
                console.error("Error al inicializar el calendario:", e);
            }
        },

        /**
         * Muestra los detalles de un evento en un modal o alerta
         * @param {Object} event - Evento de FullCalendar
         */
        MostrarDetalleEvento: function (event) {
            const props = event.extendedProps;
            const detalles = `
                <div style="text-align: left;">
                    <p><strong>Título:</strong> ${event.title}</p>
                    <p><strong>Inicio:</strong> ${event.start.toLocaleString('es-ES')}</p>
                    ${event.end ? `<p><strong>Fin:</strong> ${event.end.toLocaleString('es-ES')}</p>` : ''}
                    ${props.descripcion ? `<p><strong>Descripción:</strong> ${props.descripcion}</p>` : ''}
                    ${props.ubicacion ? `<p><strong>Ubicación:</strong> ${props.ubicacion}</p>` : ''}
                    ${props.nombreComite ? `<p><strong>Comité:</strong> ${props.nombreComite}</p>` : ''}
                </div>
            `;

            Swal.fire({
                title: 'Detalles del Evento',
                html: detalles,
                icon: 'info',
                confirmButtonText: 'Cerrar',
                confirmButtonColor: '#3788d8',
                width: 600
            });
        },

        /**
         * Recarga los eventos del calendario
         */
        RecargarEventosCalendario: function () {
            if (jsCalendario.objetos.CalendarioInstance) {
                jsCalendario.objetos.CalendarioInstance.refetchEvents();
            }
        },

        GenerarCodigoRegistro: function () {
            event.preventDefault();
            try {
                let FechaLimite = $(jsCalendario.controles.FechaLimiteCodigo).val();
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
                                return res.json().then(err => { throw err; });
                            }
                            return res.json();
                        })
                        .then(data => {
                            console.log("Respuesta OK:", data);
                            $(jsCalendario.controles.InputCodigoGenerado).val(data.result);
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
            $(jsCalendario.tablas.TablaGestionUsuarios).DataTable({
                ordering: true,
                columnDefs: [
                    {
                        targets: [2, 3],
                        orderable: false
                    },
                    {
                    }
                ],
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
            jsCalendario.objetos.ListaUsuarios = [];

            $('#TbGestionUsuarios tbody tr').each(function () {
                const $td = $(this).children('td');

                if ($td.length < 5) return;

                const Id = $.trim($td.eq(0).text());
                const Nombre = $.trim($td.eq(1).text());

                const $select = $td.eq(2).find('select.ComitesSelect');
                const IdComite = $select.val();
                const NombreComite = ($select.find('option:selected').text() || '').trim();

                const EsCordinador = $td.eq(3).find('input[type=checkbox]').prop('checked') === true;
                const Activo = $td.eq(4).find('input[type=checkbox]').prop('checked') === true;

                jsCalendario.objetos.ListaUsuarios.push({
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
                    body: JSON.stringify(jsCalendario.objetos.ListaUsuarios)
                })
                    .then(res => {
                        if (!res.ok) {
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

    eventos: function () {
        $(jsCalendario.botones.BtnGenerarCodigo).on('click', function () {
            jsCalendario.metodos.GenerarCodigoRegistro();
        });

        $(jsCalendario.botones.BtnGuardarUsuarios).on('click', function () {
            jsCalendario.metodos.LeerTablaUsuarios();
        });
    }
}

$(function () {
    jsCalendario.eventos();
    // Inicializa el calendario si estamos en la vista de calendario
    if ($('#calendar').length > 0) {
        jsCalendario.metodos.InicializarCalendario();
    }
    // Solo ejecuta estos métodos si los elementos existen
    if ($(jsCalendario.tablas.TablaGestionUsuarios).length > 0) {
        jsCalendario.metodos.CargarComites();
        jsCalendario.metodos.CargarDatatableGestionUsuarios();
    }
});