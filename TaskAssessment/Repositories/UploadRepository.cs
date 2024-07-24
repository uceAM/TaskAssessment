using Microsoft.EntityFrameworkCore;
using TaskAssessment.Data;
using TaskAssessment.Dto.Upload;
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

    public async Task<Upload?> GetById(int id)
    {
        return await _context.Uploads.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<ICollection<UploadDto>> GetUploads(int ticketId)
    {
        return await _context.Uploads.Where(u => u.TicketId == ticketId).Select(t => new UploadDto { Name = t.Name,path = t.path }).ToListAsync();
    }
}
