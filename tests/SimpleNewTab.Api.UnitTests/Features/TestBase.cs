using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Time.Testing;

namespace SimpleNewTab.Api.UnitTests.Features
{
    public abstract class TestBase : IDisposable
    {
        public DateTimeOffset UtcNow { get; }
        public FakeTimeProvider TimeProvider { get; }
        public DataContext DataContext { get; }

        private readonly SqliteConnection _DbConnection;

        public TestBase()
        {
            UtcNow = new DateTimeOffset(2024, 8, 7, 17, 30, 0, TimeSpan.Zero);
            TimeProvider = new FakeTimeProvider();
            TimeProvider.SetLocalTimeZone(TimeZoneInfo.Utc);
            TimeProvider.SetUtcNow(UtcNow);
            _DbConnection = DbHelpers.CreateDbConnection();
            DataContext = DbHelpers.CreateDataContext(_DbConnection);
            DataContext.Database.EnsureCreated();
        }

        public void Hydrate(params ImageMetadata[] imageMetadata)
        {
            DataContext.ImageMetadata.AddRange(imageMetadata);
            DataContext.SaveChanges();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _DbConnection.Dispose();
            DataContext.Dispose();
        }

        public static ImageMetadata ImageMetadata(string id, DateTimeOffset expiry = default)
        {
            var imageMetadata = new ImageMetadata()
            {
                Expiration = expiry,
                Url = $"https://www.bing.com/{id}-image.jpg",
                Title = $"{id} title",
                QuizUrl = $"https://www.bing.com/quiz/{id}",
                Copyright = $"{id} author",
                CopyrightUrl = $"https://{id}-author.com"
            };

            return imageMetadata;
        }
    }
}
