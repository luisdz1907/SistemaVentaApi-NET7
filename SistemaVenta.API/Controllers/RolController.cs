using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {
       private readonly  IRolService _rolService;

        public RolController(IRolService rolService)
        {
            _rolService = rolService;
        }
        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<RolDTO>>();
            try
            {
                response.Estatus = true;
                response.Value = await _rolService.Lista();
            }
            catch (Exception ex)
            {

                response.Estatus = false;
                response.message= ex.Message;
            }

            return Ok(response);
        }
    }
}
