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
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<ProductoDTO>>();
            try
            {
                response.Estatus = true;
                response.Value = await _productoService.Lista();
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
        public async Task<IActionResult> Guardar([FromBody] ProductoDTO producto)
        {
            var response = new Response<ProductoDTO>();
            try
            {
                response.Estatus = true;
                response.Value = await _productoService.Crear(producto);
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
        public async Task<IActionResult> Editar([FromBody] ProductoDTO producto)
        {
            var response = new Response<bool>();
            try
            {
                response.Estatus = true;
                response.Value = await _productoService.Editar(producto);
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
                response.Value = await _productoService.Eliminar(id);
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
