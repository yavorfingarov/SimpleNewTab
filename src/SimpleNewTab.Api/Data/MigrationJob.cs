namespace SimpleNewTab.Api.Data
{
    [RunBeforeStart]
    public sealed class MigrationJob : IJob
    {
        private readonly DataContext _DataContext;

        public MigrationJob(DataContext dataContext)
        {
            _DataContext = dataContext;
        }

        public Task Run(CancellationToken cancellationToken)
        {
            _DataContext.Database.Migrate();

            return Task.CompletedTask;
        }
    }
}
