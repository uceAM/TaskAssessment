using Microsoft.EntityFrameworkCore;
using TaskAssessment.Data;
using TaskAssessment.Interfaces;
using TaskAssessment.Models;

namespace TaskAssessment.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly ApplicationDbContext _context;
    public NoteRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Note> AddNote(Note Note)
    {
        await _context.AddAsync(Note);
        await _context.SaveChangesAsync();
        return Note;
    }

    public Task<bool> DeleteNote(int NoteId)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Note>> GetNotes(int TicketId)
    {
        return await _context.Notes.Where(n => n.TicketId == TicketId).ToListAsync();
    }


    public Task<bool> UpdateNote(Note Note)
    {
        throw new NotImplementedException();
    }
}
