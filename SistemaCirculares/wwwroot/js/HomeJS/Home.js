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

                    var b64 = (data && (data.archivoBytes || (data.result && data.result.archivoBytes) || data.ArchivoBytes)) || '';
                    if (!b64) { throw new Error('No vino archivoBytes en la respuesta'); }

                    b64 = String(b64).trim();
                    if (b64.indexOf(',') !== -1) b64 = b64.split(',')[1]; 
                    b64 = b64.replace(/\s/g, ''); 

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






    },
    eventos:
        function () {

            $(jsHome.botones.BtnVerCircular).on('click', function (event) {

                jsHome.metodos.ObtenerCircularPorId(event);

            });




        }




}

$(function () {
    jsHome.eventos();
});