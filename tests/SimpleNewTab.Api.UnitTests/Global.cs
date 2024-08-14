global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Logging;
global using NSubstitute;
global using SimpleNewTab.Api.Common;
global using SimpleNewTab.Api.Data;
global using SimpleNewTab.Api.Features;
global using VerifyTests.MicrosoftLogging;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Data.Sqlite;

[assembly: SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "Test names can contain underscores.")]

namespace SimpleNewTab.Api.UnitTests
{
    public static class ModuleInit
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            VerifyHttp.Initialize();
            VerifyMicrosoftLogging.Initialize();
            VerifyEntityFramework.Initialize();
        }
    }

    public static class DbHelpers
    {
        public static SqliteConnection CreateDbConnection()
        {
            var dbConnection = new SqliteConnection("Data Source=:memory:");
            dbConnection.Open();

            return dbConnection;
        }

        public static DataContext CreateDataContext(SqliteConnection? dbConnection = null)
        {
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DataContext>();
            dbContextOptionsBuilder.EnableRecording();
            if (dbConnection != null)
            {
                dbContextOptionsBuilder.UseSqlite(dbConnection);
            }
            else
            {
                dbContextOptionsBuilder.UseSqlite();
            }

            var dataContext = new DataContext(dbContextOptionsBuilder.Options);

            return dataContext;
        }
    }
}
