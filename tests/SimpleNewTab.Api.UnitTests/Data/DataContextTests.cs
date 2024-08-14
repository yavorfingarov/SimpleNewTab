using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleNewTab.Api.UnitTests.Data
{
    public sealed class DataContextTests
    {
        [Fact]
        public Task Schema()
        {
            using var dataContext = DbHelpers.CreateDataContext();

            var schema = dataContext.Database.GenerateCreateScript();

            return Verify(schema);
        }

        [Theory]
        [MemberData(nameof(GetMigrations), true)]
        public Task UpMigration(string name, string script)
        {
            return Verify(script)
                .UseParameters(name);
        }

        [Theory]
        [MemberData(nameof(GetMigrations), false)]
        public Task DownMigration(string name, string script)
        {
            return Verify(script)
                .UseParameters(name);
        }

        [Fact]
        public void NoPendingChanges()
        {
            using var dataContext = DbHelpers.CreateDataContext();
            var pendingChanges = dataContext.Database.HasPendingModelChanges();

            Assert.False(pendingChanges);
        }

        public static TheoryData<string, string> GetMigrations(bool up)
        {
            var theoryData = new TheoryData<string, string>();
            using var dataContext = DbHelpers.CreateDataContext();
            var migrator = dataContext.GetService<IMigrator>();
            var migrationNames = dataContext.Database.GetMigrations();
            if (up)
            {
                migrationNames = migrationNames.Prepend(null!);
            }
            else
            {
                migrationNames = migrationNames.Reverse()
                    .Append(null!);
            }

            var fromMigration = migrationNames.First();
            foreach (var targetMigration in migrationNames.Skip(1))
            {
                var migrationScript = migrator.GenerateScript(fromMigration, targetMigration);
                var migrationName = up ? targetMigration : fromMigration;

                theoryData.Add(migrationName, migrationScript);

                fromMigration = targetMigration;
            }

            return theoryData;
        }
    }
}
