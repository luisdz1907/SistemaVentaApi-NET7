using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class VentaService: IVentaService
    {
        private readonly IVentasRepository _ventasRepository;
        private readonly IGenericReponsitory<DetalleVenta> _detalleVentaRepository;
        private readonly IMapper _mapper;

        public VentaService(IVentasRepository ventasRepository, IGenericReponsitory<DetalleVenta> detalleVentaRepository, IMapper mapper)
        {
            _ventasRepository = ventasRepository;
            _detalleVentaRepository = detalleVentaRepository;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try
            {
                var ventaGenerada = await _ventasRepository.Registrar(_mapper.Map<Venta>(modelo));

                if (ventaGenerada.IdVenta == 0)
                {
                    throw new TaskCanceledException("No se pudo crear");
                }

                return _mapper.Map<VentaDTO>(modelo);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFin)
        {
                IQueryable<Venta> query = await _ventasRepository.Consultar();//Hacemos la consulta ala tabla
                var ListaResultado = new List<Venta>();
            try
            {
                if (buscarPor == "fecha")
                {
                    DateTime fechInicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
                    DateTime fechFin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-CO"));

                    ListaResultado = await query.Where(v =>
                    v.FechaRegistro.Value >= fechInicio.Date &&
                    v.FechaRegistro.Value <= fechFin.Date
                    ).Include(dv => dv.DetalleVenta
                    ).ThenInclude(dp => dp.IdProductoNavigation).ToListAsync();
                }
                else
                {
                    ListaResultado = await query.Where(v =>
                  v.NumeroDocumento == numeroVenta
                  ).Include(dv => dv.DetalleVenta
                  ).ThenInclude(dp => dp.IdProductoNavigation).ToListAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
           return _mapper.Map<List<VentaDTO>>(ListaResultado);
        }

        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepository.Consultar();
            var ListaResultado = new List<DetalleVenta>();
            try
            {
                DateTime fechInicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-CO"));
                DateTime fechFin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-CO"));
                ListaResultado = await query
                    .Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv =>
                     dv.IdVentaNavigation.FechaRegistro.Value.Date >= fechInicio.Date &&
                     dv.IdVentaNavigation.FechaRegistro.Value.Date <= fechFin.Date
                    ).ToListAsync();

            }
            catch (Exception)
            {

                throw;
            }
            return _mapper.Map<List<ReporteDTO>>(ListaResultado);
        }
    }
}
