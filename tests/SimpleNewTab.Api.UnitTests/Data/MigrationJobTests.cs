namespace SimpleNewTab.Api.UnitTests.Data
{
    public class MigrationJobTests
    {
        [Fact]
        public async Task Run()
        {
            using var dbConnection = DbHelpers.CreateDbConnection();
            using var dataContext = DbHelpers.CreateDataContext(dbConnection);
            var migrationJob = new MigrationJob(dataContext);
            var pendingMigrations = dataContext.Database.GetPendingMigrations();
            Assert.NotEmpty(pendingMigrations);

            await migrationJob.Run(CancellationToken.None);

            pendingMigrations = dataContext.Database.GetPendingMigrations();
            Assert.Empty(pendingMigrations);
        }
    }
}
