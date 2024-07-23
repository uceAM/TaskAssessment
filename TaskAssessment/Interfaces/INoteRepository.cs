using Microsoft.AspNetCore.Mvc;
using TaskAssessment.Models;

namespace TaskAssessment.Interfaces;

public interface INoteRepository
{
    public Task<ICollection<Note>> GetNotes(int TicketId);
    public Task<Note> AddNote(Note Note);
    public Task<bool> UpdateNote(Note Note);
    public Task<bool> DeleteNote(int NoteId);
}
