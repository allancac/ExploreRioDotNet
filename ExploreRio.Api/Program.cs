
using ExploreRio.Api.Dtos;

namespace ExploreRio.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            List<TrailResponseDto> trailsList = new List<TrailResponseDto>();
            {
                trailsList.Add(new TrailResponseDto(1, "Trilha Pedra da Gávea", "Trilha desafiadora com trechos de escalada leve, levando ao topo de uma das maiores formações rochosas à beira-mar do mundo, com vista panorâmica do Rio.", "montanha, mirante, floresta, aventura, zona sul", 6, 6.5, "dificil", "zona sul"));
                trailsList.Add(new TrailResponseDto(2, "Trilha Pedra Bonita", "Trilha de nível moderado que leva ao topo da Pedra Bonita, excelente para fotos e com opção de voo de asa delta.", "mirante, floresta, aventura, asa delta", 3, 2.5, "media", "zona sul"));
                trailsList.Add(new TrailResponseDto(3, "Trilha Morro Dois Irmãos", "Trilha curta com vista incrível das praias do Leblon e Ipanema, acessada pela comunidade do Vidigal.", "mirante, favela tour, praia, floresta urbana", 2, 1.6, "facil", "zona sul"));
                trailsList.Add(new TrailResponseDto(4, "Trilha da Pedra do Telégrafo", "Famosa por suas fotos em perspectiva, essa trilha oferece vista para praias selvagens e costões rochosos.", "mirante, foto, floresta, aventura, zona oeste", 4, 3.5, "media", "zona oeste"));
                trailsList.Add(new TrailResponseDto(5, "Trilha Circuito das Praias Selvagens", "Percurso costeiro que conecta as praias do Meio, Funda, Inferno e Perigoso. Um dos mais belos circuitos do Rio.", "praia, floresta, costão, zona oeste, trilha longa", 8, 4.0, "media", "zona oeste"));
            }

            const string GetTrailEndpointName = "getTrail";

            //GET /
            app.MapGet("/", () => "Explore Rio API");
            //GET /api/v1/trilhas
            app.MapGet("/api/v1/trilhas", () => trailsList);
            //GET /api/v1/trilhas/{id}
            app.MapGet("/api/v1/trilhas/{id}", (int id) => trailsList.Find((trail) => trail.Id == id))
            .WithName(GetTrailEndpointName);
            //POST /api/v1/trilhas
            app.MapPost("/api/v1/trilhas", (CreateTrailDto newTrailDto) =>
            {
                TrailResponseDto trail = new TrailResponseDto
                (
                    trailsList.Count + 1,
                    newTrailDto.Name,
                    newTrailDto.Description,
                    newTrailDto.Keywords,
                    newTrailDto.Distance,
                    newTrailDto.Time,
                    newTrailDto.Difficult,
                    newTrailDto.Region

                );
                trailsList.Add(trail);

                return Results.CreatedAtRoute(GetTrailEndpointName, new { id = trail.Id }, trail);
            });
            //PUT /api/v1/trilhas
            app.MapPut("/api/v1/trilhas/{id}", (int id, UpdateTrailDto updatedTrail) =>
            {
                var index = trailsList.FindIndex(trail => trail.Id == id);

                if (index == -1) return Results.NotFound();
             
                trailsList[index] = new TrailResponseDto(
                    id,
                    updatedTrail.Name,
                    updatedTrail.Description,
                    updatedTrail.Keywords,
                    updatedTrail.Distance,
                    updatedTrail.Time,
                    updatedTrail.Difficult,
                    updatedTrail.Region
                    );

                return Results.NoContent();
                
            });

            //DELETE /api/v1/trilhas
            app.MapDelete("/api/v1/trilhas/{id}", (int id) => {

                int index = trailsList.FindIndex(trail =>  trail.Id == id);

                if (index == -1) return Results.NotFound();

                trailsList.RemoveAt(index);

                return Results.NoContent();

            });


            app.Run();
        }
    }
}
