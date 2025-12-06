using System;
using System.Collections.Generic;

namespace LaReservaBackend.Application.Complejos.Queries.GetComplejos;

public class GetComplejosResponse
{
    public List<DeporteDto> Deportes { get; init; } = new();
    public List<ComplejoDto> Complejos { get; init; } = new();
    public DateTime Timestamp { get; init; }
}

public class DeporteDto
{
    public int Id { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
    public string BgClass { get; init; } = string.Empty;
    public string TextClass { get; init; } = string.Empty;
}

public class ComplejoDto
{
    public int Id { get; init; }
    public int CiudadId { get; init; }
    public string Nombre { get; init; } = string.Empty;
    public string Precio { get; init; } = string.Empty;
    public string Imagen { get; init; } = string.Empty;
    public string Categoria { get; init; } = string.Empty;
    public string DeportePillBg { get; init; } = string.Empty;
    public string DeportePillText { get; init; } = string.Empty;
}
