namespace SimpleNewTab.Api.Features
{
    [Endpoint]
    public sealed class HealthEndpoint
    {
        [Map("HEAD", "/api/health")]
        public static IResult Handle(DataContext dataContext)
        {
            var isHealthy = dataContext.ImageMetadata.Any();
            if (isHealthy)
            {
                return Results.Ok();
            }

            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
