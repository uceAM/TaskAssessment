using TaskAssessment.Models;

namespace TaskAssessment.Interfaces;

public interface IUploadRepository
{
    public Task<ICollection<Upload>> GetUploads(int ticketId);
    public Task<bool> AddUpload(Upload upload);
    public Task<bool> DeleteUpload(Upload upload);
}
