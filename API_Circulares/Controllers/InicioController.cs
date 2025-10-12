using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities_Circulares.Reply;
using Entities_Circulares.UserRegistration;

namespace API_Circulares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InicioController : ControllerBase
    {

        public Reply<UserRegistration> LogIn(UserRegistration ObjUsuario)
        {

            Reply<UserRegistration> reply = new Reply<UserRegistration>();

            return reply;


        }

    }
}
