using TaskAssessment.Dto.Upload;
using TaskAssessment.Models;

namespace TaskAssessment.Interfaces;

public interface IUploadRepository
{
    public Task<ICollection<UploadDto>> GetUploads(int ticketId);
    public Task<bool> AddUpload(Upload upload);
    public Task<bool> DeleteUpload(Upload upload);
    public Task<Upload?> GetById(int id);
}
