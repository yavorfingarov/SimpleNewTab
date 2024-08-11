using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SimpleNewTab.Api.Data
{
    public sealed class DataContext : DbContext
    {
        public DbSet<ImageMetadata> ImageMetadata { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImageMetadata>()
                .HasKey(x => x.Expiration);

            modelBuilder.Entity<ImageMetadata>()
                .Property(x => x.Expiration)
                .HasConversion<DateTimeOffsetToBinaryConverter>();
        }
    }
}
