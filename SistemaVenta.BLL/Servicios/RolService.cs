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
    public class RolService: IRolService
    {
        private readonly IGenericReponsitory<Rol> _rolRepository;
        private readonly IMapper _mapper;

        public RolService(IGenericReponsitory<Rol> rolRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> Lista()
        {
            try
            {
                var ListaRoles = await _rolRepository.Consultar();
                return _mapper.Map<List<RolDTO>>(ListaRoles.ToList());
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
