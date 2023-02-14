using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using SistemaVenta.DTO;
using SistemaVenta.Model;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaVenta.Utility
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Rol, RolDTO>().ReverseMap();
            CreateMap<Menu, MenuDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>()
                .ForMember(destino =>
                destino.RolDescripcion,
                opt =>
                opt.MapFrom(origen => origen.IdRolNavigation.Nombre))
                .ForMember(destino =>
                destino.Activo,
                opt =>
                opt.MapFrom(origen => origen.Activo == true ? 1 : 0)
                );

            CreateMap<Usuario, SesionDTO>()
                .ForMember(destino =>
                destino.RolDescripcion,
                opt =>
                opt.MapFrom(origen => origen.IdRolNavigation.Nombre)
                );

            CreateMap<UsuarioDTO, Usuario>()
                .ForMember(destino =>
                destino.IdRolNavigation,
                opt => opt.Ignore()
                )
                .ForMember(destino =>
                destino.Activo,
                opt =>
                opt.MapFrom(origen => origen.Activo == 1 ? true : false));


            CreateMap<Categoria, CategoriaDTO>().ReverseMap();

            CreateMap<Producto, ProductoDTO>()
                .ForMember(destino =>
                destino.DescripcionCategoria,
                opt => opt.MapFrom(origen => origen.IdCategoriaNavigation.Nombre))

                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-CO"))))
                .ForMember(destino =>
                destino.Activo,
                opt =>
                opt.MapFrom(origen => origen.Activo == true ? 1 : 0));


            CreateMap<ProductoDTO, Producto>()
                .ForMember(destino =>
                destino.IdProducto,
                opt => opt.Ignore())

                .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen => Convert.ToDecimal(origen.Precio, new CultureInfo("es-CO"))))
                .ForMember(destino =>
                destino.Activo,
                opt =>
                opt.MapFrom(origen => origen.Activo == 1 ? true : false));

            CreateMap<Venta, VentaDTO>()
                 .ForMember(destino =>
                 destino.PrecioTexto,
                 opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-CO")))
                 )

                 .ForMember(destino =>
                 destino.FechaRegistro,
                 opt => opt.MapFrom(origen => origen.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                 );

            CreateMap<VentaDTO, Venta>()
                 .ForMember(destino =>
                 destino.Precio,
                 opt => opt.MapFrom(origen => Convert.ToDecimal(origen.PrecioTexto, new CultureInfo("es-CO")))
                 );

            CreateMap<DetalleVenta, DetalleVentaDTO>()
                .ForMember(destino =>
                destino.DescripcionProducto,
                opt =>
                opt.MapFrom(origen => origen.IdProductoNavigation.Nombre))
              .ForMember(destino =>
                destino.PrecioTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-CO"))))
               .ForMember(destino =>
                destino.TotalTexto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Total.Value, new CultureInfo("es-CO")))
                );            
            
            CreateMap<DetalleVentaDTO, DetalleVenta>()
              .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen => Convert.ToString(origen.PrecioTexto, new CultureInfo("es-CO"))))
               .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen => Convert.ToString(origen.TotalTexto, new CultureInfo("es-CO")))
                );


            CreateMap<DetalleVenta, ReporteDTO>()
                .ForMember(destino =>
                 destino.FechaRegistro,
                 opt => opt.MapFrom(origen => origen.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy"))
                 )
             .ForMember(destino =>
                 destino.NumeroDocuento,
                 opt => opt.MapFrom(origen => origen.IdVentaNavigation.NumeroDocumento)
                 )
            .ForMember(destino =>
                 destino.TipoPago,
                 opt => opt.MapFrom(origen => origen.IdVentaNavigation.TipoPago)
                 )
            .ForMember(destino =>
                destino.TotalVenta,
                opt => opt.MapFrom(origen => Convert.ToString(origen.IdVentaNavigation.Precio.Value, new CultureInfo("es-CO")))
                )
            .ForMember(destino =>
                destino.Producto,
                opt => opt.MapFrom(origen => Convert.ToString(origen.IdProductoNavigation.Nombre))
                )
            .ForMember(destino =>
                destino.Precio,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Precio.Value, new CultureInfo("es-CO")))
                )
            .ForMember(destino =>
                destino.Total,
                opt => opt.MapFrom(origen => Convert.ToString(origen.Total, new CultureInfo("es-CO")))
                );
        }
    }
}
