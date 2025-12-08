using LaReservaBackend.Application.Common.Models.DTOs.Complejos;
using LaReservaBackend.Application.Common.Models.DTOs.Deportes;

namespace LaReservaBackend.Application.Common.Interfaces;
public interface IDeportesRepository
{
    Task<List<DeporteDTO>> GetDeportes(CancellationToken cancellationToken);
}
