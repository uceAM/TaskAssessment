using TaskAssessment.Dto.Note;
using TaskAssessment.Models;

namespace TaskAssessment.Interfaces;

public interface INoteRepository
{
    public Task<ICollection<Note>> GetNotes(int TicketId);
    public Task<bool> AddNote(Note Note);
    public Task<bool> UpdateNote(Note Note,NoteDto NoteDto);
    public Task<bool> DeleteNote(Note Note);
}
