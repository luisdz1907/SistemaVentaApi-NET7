using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios;
using SistemaVenta.DAL.Repositorios.Contratos;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.DAL.Repositorios
{
    public class VentaRepository : GenericRepository<Venta>, IVentasRepository
    {
        private readonly DbventaContext _dbcontext;

        public VentaRepository(DbventaContext dbcontext) : base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();
            using (var trasction = _dbcontext.Database.BeginTransaction())
            {
                try
                {
                    foreach (DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Producto productoEncontrado = (Producto)_dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto);
                        productoEncontrado.Stock = productoEncontrado.Stock - dv.Cantidad;
                        _dbcontext.Productos.Update(productoEncontrado);


                    }
                    await _dbcontext.SaveChangesAsync();
                    NumeroDocumento correlativo = _dbcontext.NumeroDocumentos.First();
                    correlativo.UltimoNumero = +1;
                    correlativo.FechaRegistro = DateTime.Now;
                    _dbcontext.NumeroDocumentos.Update(correlativo);
                    await _dbcontext.SaveChangesAsync();

                    //Formato del numero de documento de venta
                    int CantidadDigitos = 4;
                    string ceros = string.Concat(Enumerable.Repeat("0", CantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();

                    numeroVenta.Substring(numeroVenta.Length - CantidadDigitos, CantidadDigitos);
                    modelo.NumeroDocumento = numeroVenta;
                    await _dbcontext.Venta.AddAsync(modelo);
                    await _dbcontext.SaveChangesAsync();

                    ventaGenerada = modelo;
                    trasction.Commit();//Finalizamos la transaccion
                }
                catch
                {

                    trasction.Rollback();
                    throw;
                }

                return ventaGenerada;
            }
        }
    }
}
