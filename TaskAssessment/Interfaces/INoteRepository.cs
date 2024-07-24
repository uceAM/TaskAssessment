using TaskAssessment.Dto.Note;
using TaskAssessment.Models;

namespace TaskAssessment.Interfaces;

public interface INoteRepository
{
    public Task<ICollection<NoteDto>> GetNotes(int TicketId);
    public Task<Note?> GetById(int id); //internal API
    public Task<bool> AddNote(Note Note);
    public Task<bool> UpdateNote(Note Note,NoteDto NoteDto);
    public Task<bool> DeleteNote(Note Note);
}
