var jsCircular = {
    c: {
        nombreCircular: '#nombreCircular',
        soloMiembros: '#soloMiembros',
        dropArea: '#dropArea',
        archivo: '#archivo',
        fileInfo: '#fileInfo',
        fechaEvento: '#fechaEvento',
        nombreEvento: '#nombreEvento',
        form: '#formCircular',
        btnPublicar: '#btnPublicarCircular'
    },
    v: { file: null, enviando: false },

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

    init: function () {
        var $drop = $(jsCircular.c.dropArea);

        // Click abre selector
        $drop.on('click', function () { $(jsCircular.c.archivo).trigger('click'); });

        // Drag & drop
        $drop.on('dragover', function (e) { e.preventDefault(); e.stopPropagation(); $drop.addClass('dragover'); });
        $drop.on('dragleave', function (e) { e.preventDefault(); e.stopPropagation(); $drop.removeClass('dragover'); });
        $drop.on('drop', function (e) {
            e.preventDefault(); e.stopPropagation(); $drop.removeClass('dragover');
            var dt = e.originalEvent.dataTransfer;
            if (dt && dt.files && dt.files.length) jsCircular.setFile(dt.files[0]);
        });

        // Selector de archivo
        $(jsCircular.c.archivo).on('change', function (e) {
            var f = e.target.files && e.target.files[0];
            jsCircular.setFile(f);
            this.value = '';
        });

        // Submit
        $(jsCircular.c.form).on('submit', jsCircular.enviar);
    },

    setFile: function (file) {
        if (!file) return;

        // Forzar solo PDF (puedes quitarlo si aceptas otros)
        var esPdf = file.type === 'application/pdf' || /\.pdf$/i.test(file.name);
        if (!esPdf) { alert('El archivo debe ser PDF.'); return; }

        var maxMB = 50;
        if (file.size > maxMB * 1024 * 1024) { alert('Máximo ' + maxMB + ' MB'); return; }

        jsCircular.v.file = file;
        $(jsCircular.c.fileInfo).text('Archivo: ' + file.name + ' (' + (file.size / 1024 / 1024).toFixed(2) + ' MB)');
    },

    bloquearUI: function (bloquear) {
        jsCircular.v.enviando = bloquear;
        $(jsCircular.c.btnPublicar).prop('disabled', bloquear).toggleClass('disabled', bloquear);
        if (bloquear) {
            $(jsCircular.c.btnPublicar).data('txt', $(jsCircular.c.btnPublicar).text());
            $(jsCircular.c.btnPublicar).text('Publicando…');
        } else {
            var txt = $(jsCircular.c.btnPublicar).data('txt') || 'Publicar';
            $(jsCircular.c.btnPublicar).text(txt);
        }
    },

    enviar: function (e) {
        e.preventDefault();
        if (jsCircular.v.enviando) return;

        var nombreCircular = ($(jsCircular.c.nombreCircular).val() || '').trim();
        var soloMiembros = $(jsCircular.c.soloMiembros).is(':checked');
        var fechaEvento = $(jsCircular.c.fechaEvento).val() || '';
        var nombreEvento = ($(jsCircular.c.nombreEvento).val() || '').trim();
        var file = jsCircular.v.file;

        if (!nombreCircular) { alert('Ingrese el nombre de la circular'); return; }
        if (!file) { alert('Adjunte un PDF'); return; }

        var fd = new FormData();
        fd.append('NombreCircular', nombreCircular);
        fd.append('SoloMiembros', soloMiembros ? 'true' : 'false'); // .NET lo mapea a bool
        fd.append('FechaEvento', fechaEvento);
        fd.append('NombreEvento', nombreEvento);
        fd.append('Archivo', file, file.name); // <-- IFormFile Archivo

        // Opcional: timeout con AbortController
        var ctrl = new AbortController();
        var timer = setTimeout(function () { ctrl.abort(); }, 120000); // 120s

        jsCircular.bloquearUI(true);

        fetch('/SubirCirculares/EnviarCircular', { // <-- CAMBIA a tu endpoint real
            method: 'POST',
            body: fd,
            signal: ctrl.signal,
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
            // No poner Content-Type; FormData lo establece.
        })
            .then(function (resp) {
                clearTimeout(timer);
                if (!resp.ok) return resp.text().then(function (t) { throw new Error(t || ('Error ' + resp.status)); });
                // Intentar parsear JSON; si no es JSON, seguir
                return resp.text().then(function (t) {
                    try { return JSON.parse(t); } catch { return { raw: t }; }
                });
            })
            .then(function (data) {
                jsCircular.MensajeGeneralSweetAlert(
                    'success',
                    `La circular ha sido publicada de manera exitosa`,
                    false,
                    '#68AB54',
                    30
                );
                // Reset
                jsCircular.v.file = null;
                $(jsCircular.c.fileInfo).text('');
                $(jsCircular.c.form)[0].reset();
            })
            .catch(function (err) {

                this.MensajeGeneralSweetAlert(
                    'warning',
                    `Ha ocurrido un error al publicar la circular`,
                    false,
                    '#FF0000',
                    26
                );
                console.error('PublicarCircular error:', err);
            })
            .finally(function () {
                jsCircular.bloquearUI(false);
            });
    }
};

$(function () { jsCircular.init(); });
