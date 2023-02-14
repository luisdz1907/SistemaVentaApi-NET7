using AutoMapper;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class DashboardService: IDashboardService
    {
        private readonly IVentasRepository _ventasRepository;
        private readonly IGenericReponsitory<Producto> _productoRepository;
        private readonly IMapper _mapper;

        public DashboardService(IVentasRepository ventasRepository, IGenericReponsitory<Producto> productoRepository, IMapper mapper)
        {
            _ventasRepository = ventasRepository;
            _productoRepository = productoRepository;
            _mapper = mapper;
        }
        private IQueryable<Venta> ObtenerVentas(IQueryable<Venta> tablaVenta, int catidadDias) { 
            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();
            ultimaFecha = ultimaFecha.Value.AddDays(catidadDias);
            return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
        }

        private async Task<int> TotalVentaUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> ventaQuery = await _ventasRepository.Consultar();
            if (ventaQuery.Count() > 0)
            {
                var tablaVenta = ObtenerVentas(ventaQuery, -7);
                total = tablaVenta.Count();
            }

            return total;
        }        
        private async Task<string> totalIngresosUltimaSemana()
        {
            decimal total = 0;
            IQueryable<Venta> ventaQuery = await _ventasRepository.Consultar();
            if (ventaQuery.Count() > 0)
            {
                var tablaVenta = ObtenerVentas(ventaQuery, -7);
                total = tablaVenta.Select(v => v.Precio).Sum(v => v.Value);
            }

            return Convert.ToString(total, new CultureInfo("es-CO"));
        }

        private async Task<int> totalProductos(){
            IQueryable<Producto> productoQuery = await _productoRepository.Consultar();
            int total = productoQuery.Count();
            return total;

        }

        private async Task<Dictionary<string, int>> ventasUltimaSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            IQueryable<Venta> ventaQuery = await _ventasRepository.Consultar();
            if (ventaQuery.Count() > 0)
            {
                var tablaVenta = ObtenerVentas(ventaQuery, -7);
                resultado = tablaVenta
                    .GroupBy(v=> v.FechaRegistro.Value.Date).OrderBy( g => g.Key)
                    .Select(dv=> new {fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count()})
                    .ToDictionary(keySelector: r=> r.fecha, elementSelector: r => r.total);

            }
        return resultado;
        }
        public async Task<DashboardDTO> Resumen()
        {
            DashboardDTO vmDashboard = new DashboardDTO();
            try
            {
                vmDashboard.TotalVentas = await TotalVentaUltimaSemana();
                vmDashboard.TotalIngresos = await totalIngresosUltimaSemana();
                vmDashboard.TotalProductos = await totalProductos();
                List<VentasSemanaDTO> listaVentasSemanas = new List<VentasSemanaDTO>();

                foreach (KeyValuePair<string, int> item in await ventasUltimaSemana())
                {
                    listaVentasSemanas.Add(new VentasSemanaDTO()
                    {
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }

                vmDashboard.VentasUltimasSemana = (listaVentasSemanas);

                return vmDashboard;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
