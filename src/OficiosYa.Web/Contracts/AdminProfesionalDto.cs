namespace OficiosYa.Web.Contracts;

public sealed class AdminProfesionalDto
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? Documento { get; set; }

    public string? Ciudad { get; set; }

    public string? Provincia { get; set; }

    public string? Descripcion { get; set; }

    /// <summary>Tipos derivados de categorías: Oficiante, Profesional, o ambos.</summary>
    public List<string> Tipos { get; set; } = new();

    public List<string> Categorias { get; set; } = new();

    public string PlanActual { get; set; } = "Free";

    public string? SuscripcionEstado { get; set; }

    public bool Disponible { get; set; }

    public bool Activo { get; set; }

    public decimal CalificacionPromedio { get; set; }

    public int TotalResenas { get; set; }

    public DateTime FechaRegistro { get; set; }
}

public sealed class AdminActualizarProfesionalRequest
{
    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string? Telefono { get; set; }

    public string? Documento { get; set; }

    public string? Ciudad { get; set; }

    public string? Provincia { get; set; }

    public string? Descripcion { get; set; }

    public bool Disponible { get; set; } = true;

    public bool Activo { get; set; } = true;

    /// <summary>Nombres de categorías de servicio a asociar.</summary>
    public List<string>? Categorias { get; set; }
}
