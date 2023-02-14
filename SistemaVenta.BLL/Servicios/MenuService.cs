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
    public class MenuService:IMenuService
    {
        private readonly IGenericReponsitory<Usuario> _usuarioRepository;
        private readonly IGenericReponsitory<MenuRol> _menuRolRepository;
        private readonly IGenericReponsitory<Menu> _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(IGenericReponsitory<Usuario> usuarioRepository, IGenericReponsitory<MenuRol> menuRolRepository, IGenericReponsitory<Menu> menuRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _menuRolRepository = menuRolRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<MenuDTO>> ListaMenu(int idUsuario)
        {
            IQueryable<Usuario> tblUsuario = await _usuarioRepository.Consultar(u=> u.IdUsuario == idUsuario);
            IQueryable<MenuRol> tblMenuRol = await _menuRolRepository.Consultar();
            IQueryable<Menu> tblMenu = await _menuRepository.Consultar();

            try
            {
                IQueryable<Menu> Resultado = (from u in tblUsuario
                                              join mr in tblMenuRol on u.IdRol equals mr.IdRol
                                              join m in tblMenu on mr.IdMenu equals m.IdMenu
                                              select m
                                              ).AsQueryable();
                var listaResultado = Resultado.ToList();
                return _mapper.Map<List<MenuDTO>>(listaResultado);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
