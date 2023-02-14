using AutoMapper;
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
    public class CategoriaService: ICategoriaService
    {
        private readonly IGenericReponsitory<Categoria> _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaService(IGenericReponsitory<Categoria> categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> listaCategoria()
        {
            try
            {
                var listacategoria = await _categoriaRepository.Consultar();
                return _mapper.Map<List<CategoriaDTO>>(listacategoria.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
