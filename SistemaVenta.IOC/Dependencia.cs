using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaVenta.BLL.Servicios;
using SistemaVenta.BLL.Servicios.Contrato;
using SistemaVenta.DAL.DBContext;
using SistemaVenta.DAL.Repositorios;
using SistemaVenta.DAL.Repositorios.Contratos;
using SistemaVenta.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaVenta.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            //Configuramos la depedencia del contexto de la base de datos
            services.AddDbContext<DbventaContext>(opt => {
                opt.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });

            //Registramos nuestras interfaces y repositorios
            services.AddTransient(typeof(IGenericReponsitory<>), typeof(GenericRepository<>));
            services.AddScoped<IVentasRepository, VentaRepository>();

            //Agregamos la refrencia a todos los mapeos
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //Agregamos las refrencias de los servicios
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IVentaService, VentaService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IMenuService, MenuService>();
        }


    }
}
