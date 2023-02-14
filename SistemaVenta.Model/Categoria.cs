using System;
using System.Collections.Generic;

namespace SistemaVenta.Model;

public partial class Categoria
{
    public int IdCategoria { get; set; }

    public string? Nombre { get; set; }

    public bool? Activo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Producto> Productos { get; } = new List<Producto>();
}
