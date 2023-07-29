using Microsoft.EntityFrameworkCore;

namespace FileService.DAL
{
    public class DataContext : DbContext
    {
        public DbSet<File> Files { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
    : base(options)
        { }


    }
}
