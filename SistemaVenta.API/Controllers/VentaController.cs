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
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaService;

        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            var response = new Response<VentaDTO>();
            try
            {
                response.Estatus = true;
                response.Value = await _ventaService.Registrar(venta);
            }
            catch (Exception ex)
            {

                response.Estatus = false;
                response.message = ex.Message;
            }

            return Ok(response);
        }


        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Historial(string? buscarPor, string? numeroVenta, string? fechaInicio, string? fechaFin)
        {
            var response = new Response<List<VentaDTO>>();
            numeroVenta = numeroVenta is null ? "" : numeroVenta;
            fechaInicio = fechaInicio is null ? "" : fechaInicio;
            fechaFin = fechaFin is null ? "" : fechaFin;
            try
            {
                response.Estatus = true;
                response.Value = await _ventaService.Historial(buscarPor, numeroVenta, fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {

                response.Estatus = false;
                response.message = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string? fechaInicio, string? fechaFin)
        {
            var response = new Response<List<ReporteDTO>>();
            try
            {
                response.Estatus = true;
                response.Value = await _ventaService.Reporte(fechaInicio, fechaFin);
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
