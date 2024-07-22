using TaskAssessment.Dto.Ticket;
using TaskAssessment.Models;

namespace TaskAssessment.Interfaces;

public interface ITicketRepostiory
{
    public Task<ICollection<Ticket>> ListMyTickets();
    public Task<ICollection<Ticket>> ListTeamTickets();
    public Task<bool> AddTicket(Ticket Ticket);
    public Task<bool> UpdateTicket(Ticket Ticket);
    public Task<bool> DeleteTicket(int TicketId);
}
