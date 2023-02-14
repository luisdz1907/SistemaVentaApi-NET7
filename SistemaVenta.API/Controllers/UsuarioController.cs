using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaVenta.API.Utilidad;
using SistemaVenta.BLL.Servicios;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DTO;

namespace SistemaVenta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<UsuarioDTO>>();
            try
            {
                response.Estatus = true;
                response.Value = await _usuarioService.Lista();
            }
            catch (Exception ex)
            {

                response.Estatus = false;
                response.message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {
            var response = new Response<SesionDTO>();
            try
            {
                response.Estatus = true;
                response.Value = await _usuarioService.ValidarCredenciales(login.Correo, login.Clave);
            }
            catch (Exception ex)
            {

                response.Estatus = false;
                response.message = ex.Message;
            }

            return Ok(response);
        }



        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] UsuarioDTO usuario)
        {
            var response = new Response<UsuarioDTO>();
            try
            {
                response.Estatus = true;
                response.Value = await _usuarioService.Crear(usuario);
            }
            catch (Exception ex)
            {

                response.Estatus = false;
                response.message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO usuario)
        {
            var response = new Response<bool>();
            try
            {
                response.Estatus = true;
                response.Value = await _usuarioService.Editar(usuario);
            }
            catch (Exception ex)
            {

                response.Estatus = false;
                response.message = ex.Message;
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = new Response<bool>();
            try
            {
                response.Estatus = true;
                response.Value = await _usuarioService.Eliminar(id);
            }
            catch (Exception ex)
            {

                response.Estatus = false;
                response.message = ex.Message;
            }

            return Ok(response);
        }
    }
}
