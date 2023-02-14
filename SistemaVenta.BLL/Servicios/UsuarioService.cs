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
    public class UsuarioService: IUsuarioService
    {
        private readonly IGenericReponsitory<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioService(IGenericReponsitory<Usuario> usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }
        public async Task<List<UsuarioDTO>> Lista()
        {
            try
            {
                var queryUsuario = await _usuarioRepository.Consultar();
                var listaUsuarios = queryUsuario.Include(rol => rol.IdRolNavigation).ToList();
                return _mapper.Map<List<UsuarioDTO>>(listaUsuarios);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<SesionDTO> ValidarCredenciales(string email, string clave)
        {
            try
            {
                var queryUsuario = await _usuarioRepository.Consultar(u => u.Correo == email && u.Clave == clave);
                if (queryUsuario.FirstOrDefault() == null)
                {
                    throw new TaskCanceledException("El Usuario no existe");
                }

                Usuario user = queryUsuario.Include(rol =>rol.IdRolNavigation).First();
                return _mapper.Map<SesionDTO>(user);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try
            {
                var usuarioNew = await _usuarioRepository.Crear(_mapper.Map<Usuario>(modelo));
                if (usuarioNew.IdUsuario == 0)
                {
                    throw new TaskCanceledException("El Usuario no se pudo crear");

                }

                var query = await _usuarioRepository.Consultar(u => u.IdUsuario == usuarioNew.IdUsuario);
                usuarioNew = query.Include(rol =>rol.IdRolNavigation).First();

                return _mapper.Map<UsuarioDTO>(usuarioNew);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try
            {
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El Usuario no existe");
                }
                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol   ;
                usuarioEncontrado.Clave = usuarioModelo.Clave;
                usuarioEncontrado.Activo = usuarioModelo.Activo;

                bool response = await _usuarioRepository.Editar(usuarioEncontrado);
                if (!response)
                {
                    throw new TaskCanceledException("No se pudo editar");
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
            try
            {
                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdRol == id);
                if (usuarioEncontrado == null)
                {
                    throw new TaskCanceledException("El usuario no existe");
                }
                bool response = await _usuarioRepository.Eliminar(usuarioEncontrado);
                if (!response)
                {
                    throw new TaskCanceledException("No se pudo editar");
                }
                return response;
            }
            catch (Exception)
            {

                throw;
            }

        }




    }
}
