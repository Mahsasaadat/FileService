using FileService.DAL;
using Microsoft.EntityFrameworkCore;

namespace FileService.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly DataContext _context;
        public FileRepository(DataContext context)
        {
            _context = context;
        }
        public async Task AddFile(DAL.File file)
        {
            await _context.Files.AddAsync(file);
            await _context.SaveChangesAsync();
        }
        public async Task<List<DAL.File>> GetAllFiles()
        {
            return await _context.Files.ToListAsync();
        }
        public async Task<bool> Exist(string originalUrl)
        {
            return await _context.Files.AnyAsync(f => f.OriginalURL == originalUrl);
        }
    }
}
