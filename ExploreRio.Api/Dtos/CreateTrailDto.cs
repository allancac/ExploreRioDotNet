namespace ExploreRio.Api.Dtos
{
    public record class CreateTrailDto
    (
        string Name,
        string Description,
        string Keywords,
        int Distance,
        double Time,
        string Difficult,
        string Region
    );
}

