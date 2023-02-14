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
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int id)
        {
            var response = new Response<List<MenuDTO>>();
            try
            {
                response.Estatus = true;
                response.Value = await _menuService.ListaMenu(id);
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
