using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaReservaBackend.Application.Common.Interfaces;
using LaReservaBackend.Application.Complejos.Queries.GetComplejos;

namespace LaReservaBackend.Application.Reservas.Queries;
public class GetHorariosDisponibles : IRequest<GetHorariosDisponiblesResponse>
{
    public int ComplejoId { get; init; }

    public GetHorariosDisponibles(int complejoId)
    {
        ComplejoId = complejoId;
    }

    public class GetHorariosDisponiblesHandler : IRequestHandler<GetHorariosDisponibles, GetHorariosDisponiblesResponse>
    {
private readonly IReservaRepository _reservaRepository;


        public GetHorariosDisponiblesHandler(IReservaRepository reservaRepository)
      {
       _reservaRepository = reservaRepository;
     }

        public async Task<GetHorariosDisponiblesResponse> Handle(GetHorariosDisponibles request, CancellationToken cancellationToken)
        {
            return await _reservaRepository.GetHorariosDisponibles(request.ComplejoId, cancellationToken);
        }
    }
}
