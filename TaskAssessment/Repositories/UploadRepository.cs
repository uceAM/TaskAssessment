using Microsoft.EntityFrameworkCore;
using TaskAssessment.Data;
using TaskAssessment.Interfaces;
using TaskAssessment.Models;

namespace TaskAssessment.Repositories;

public class UploadRepository : IUploadRepository
{
    private readonly ApplicationDbContext _context;
    public UploadRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> AddUpload(Upload upload)
    {
        try
        {
            await _context.AddAsync(upload);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteUpload(Upload upload)
    {
        try
        {
            _context.Remove(upload);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<ICollection<Upload>> GetUploads(int ticketId)
    {
        throw new NotImplementedException();
    }
}
