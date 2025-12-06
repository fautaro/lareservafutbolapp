using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaReservaBackend.Application.Common.Models.DTOs.Complejos;
public class ComplejoDTO
{
    public long Id { get; set; }
    public long Ciudad_Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Precio { get; set; }
    public string? Imagen { get; set; }
    public string? Categoria { get; set; }
    public string? DeportePillBg { get; set; }
    public string? DeportePillText { get; set; }
}
