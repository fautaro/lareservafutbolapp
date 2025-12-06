namespace LaReservaBackend.Application.Complejos.Queries.GetComplejos;

public class GetComplejosRequest : IRequest<GetComplejosResponse>
{
    public int ComplejoId { get; init; }
    public int DeporteId { get; init; }

    public GetComplejosRequest(int complejoId, int deporteId)
    {
        ComplejoId = complejoId;
        DeporteId = deporteId;
    }
}

public class GetComplejosHandler : IRequestHandler<GetComplejosRequest, GetComplejosResponse>
{
    public async Task<GetComplejosResponse> Handle(GetComplejosRequest request, CancellationToken cancellationToken)
    {

        await Task.Delay(100, cancellationToken);

        var deportes = new List<DeporteDto>
        {
            new DeporteDto
            {
                Id =1,
                Nombre = "Fútbol",
                Icon = "fas fa-futbol",
                BgClass = "bg-blue-600",
                TextClass = "text-white",
            },
            new DeporteDto
            {
                Id =2,
                Nombre = "Pádel",
                Icon = "fas fa-table-tennis",
                BgClass = "bg-gray-200",
                TextClass = "text-black",
            }
        };

        var complejos = new List<ComplejoDto>
        {
            new ComplejoDto
            {
                Id =1,
                CiudadId =1,
                Nombre = "La tranquera Complejo Deportivo",
                Precio = "$45.000",
                Imagen = "https://www.infocanuelas.com/media/luz-verde-para-el-funcionamiento-de-gimnasios-y-canchas-de-futbol-5-21942.jpg",
                Categoria = "Fútbol5",
                DeportePillBg = "bg-blue-100",
                DeportePillText = "text-blue-800",
            },
            new ComplejoDto
            {
                Id =4,
                CiudadId =1,
                Nombre = "Tercer Tiempo",
                Precio = "$40.000",
                Imagen = "https://i.ibb.co/FbyqzzWF/497928378-2885642464969002-7636997158516311138-n.jpg",
                Categoria = "Fútbol5",
                DeportePillBg = "bg-blue-100",
                DeportePillText = "text-blue-800",
            }
        };

        return new GetComplejosResponse
        {
            Deportes = deportes,
            Complejos = complejos,
            Timestamp = DateTime.UtcNow,
        };
    }
}
