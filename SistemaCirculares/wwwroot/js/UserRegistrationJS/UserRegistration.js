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
        InputPassword: '#InputPassword'

    },

    botones: {

        btnLogin: '#btnLogin'

    },

    variables: {




    },
    metodos: {

        LogIn: function () {

            event.preventDefault();

            let User = $(jslogin.controles.InputEmail).val().trim();
            let Password = $(jslogin.controles.InputPassword).val().trim();

            console.log(User)
            console.log(Password)
            


            // Realizar la solicitud AJAX
            $.ajax({
                url: '../Inicio/LogIn',
                type: 'POST',
                data: { Email: User, Password: Password },
                success: function (result) {

                    if (result.ok) {
                        
                        //Swal.fire({
                        //    title: "Éxito",
                        //    text: `${result.mensaje}`,
                        //    icon: "success"
                        //});

                        // Redirigir a la página principal después de 2 segundos
                        setTimeout(function () {
                            window.location.href = '/UserRegistration/UserRegistration';
                        }, 2100);

                    } else {
                        alert("error")
                        //Swal.fire({
                        //    title: "Advertencia",
                        //    text: `${result.mensaje}`,
                        //    icon: "warning"
                        //});
                    }



                },
                error: function () {
                    // Manejo de errores si es necesario

                    Swal.fire({
                        title: "Error",
                        text: `${result.mensaje}`,
                        icon: "error"
                    });

                }
            });


        }


    },
    eventos:
        function () {

            $(jslogin.botones.btnLogin).on('click', function () {

                jslogin.metodos.LogIn();

            });


        }

}

$(function () {
    jslogin.eventos();
});