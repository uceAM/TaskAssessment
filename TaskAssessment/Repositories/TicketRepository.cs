using TaskAssessment.Data;
using TaskAssessment.Dto.Ticket;
using TaskAssessment.Interfaces;
using TaskAssessment.Models;

namespace TaskAssessment.Repositories;

public class TicketRepository : ITicketRepostiory
{
    private readonly ApplicationDbContext _context;
    public TicketRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> AddTicket(Ticket Ticket)
    {
        try
        {
            await _context.AddAsync(Ticket);
            await _context.SaveChangesAsync();
            return true;
        } catch (Exception ex)
        {
            return false;
        }
    }

    public Task<bool> DeleteTicket(int TicketId)
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Ticket>> ListMyTickets()
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Ticket>> ListTeamTickets()
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateTicket(Ticket Ticket)
    {
        throw new NotImplementedException();
    }
}
