using LaReservaBackend.Application.Common.Models.DTOs.Complejos;

namespace LaReservaBackend.Application.Common.Interfaces;

public interface IComplejosRepository
{
    Task<List<ComplejoDTO>> GetComplejos(CancellationToken cancellationToken, long? ciudadId = null, long? deporteId = null, long? complejoId = null);
}
