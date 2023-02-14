using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.Repositorios.Contratos;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.BLL.Servicios
{
    public class ProductoService:IProductoService
    {
        private readonly IGenericReponsitory<Producto> _productoRepository;
        private readonly IMapper _mapper;

        public ProductoService(IGenericReponsitory<Producto> productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }
        public async Task<List<ProductoDTO>> Lista()
        {
            try
            {
                var queryProducto = await _productoRepository.Consultar();
                var listaProducto = queryProducto.Include( cat => cat.IdCategoriaNavigation).ToList();
                return _mapper.Map<List<ProductoDTO>>(listaProducto.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try
            {
                var productoNew = await  _productoRepository.Crear(_mapper.Map<Producto>(modelo));
                if (productoNew.IdProducto == 0)
                {
                    throw new TaskCanceledException("No se pudo crear el producto");
                }

                return _mapper.Map<ProductoDTO>(productoNew);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try
            {
                var productoModel = _mapper.Map<Producto>(modelo);
                var productoEncontrado = await _productoRepository.Obtener(u => u.IdProducto == productoModel.IdProducto);

                if (productoEncontrado == null)
                {
                    throw new TaskCanceledException("El producto no existe");
                }

                productoEncontrado.Nombre = productoModel.Nombre;
                productoEncontrado.IdCategoria= productoModel.IdCategoria;
                productoEncontrado.Stock = productoModel.Stock;
                productoEncontrado.Precio = productoModel.Precio;
                productoEncontrado.Activo = productoModel.Activo;

                bool response = await _productoRepository.Editar(productoEncontrado);
                if (!response)
                {
                    throw new TaskCanceledException("El producto no se pudo editar");
                }

                return response;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == id);
            if (productoEncontrado == null)
            {
                throw new TaskCanceledException("El no se encontro");
            }

            bool response = await _productoRepository.Eliminar(productoEncontrado);
            if (!response)
            {
                throw new TaskCanceledException("El producto no se pudo eliminar");
            }

            return response;
        }


    }
}
