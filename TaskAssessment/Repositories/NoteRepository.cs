using Microsoft.EntityFrameworkCore;
using TaskAssessment.Data;
using TaskAssessment.Dto.Note;
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
    public async Task<bool> AddNote(Note Note)
    {
        try
        {
            await _context.AddAsync(Note);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public async Task<bool> DeleteNote(Note Note)
    {
        try
        {
            _context.Remove(Note);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }

    public async Task<ICollection<Note>> GetNotes(int TicketId)
    {
        return await _context.Notes.Where(n => n.TicketId == TicketId).ToListAsync();
    }


    public async Task<bool> UpdateNote(Note Note, NoteDto NoteDto)
    {
        Note.Text = NoteDto.Text;
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
