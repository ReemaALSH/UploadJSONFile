using UploadJSONFile.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace UploadJSONFile.Infrastructure
{
    public class DataContext : DbContext
    {
        //public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<JSONFile> JSONFiles { get; set; }

    }
}
