using TaskAssessment.Dto.Ticket;
using TaskAssessment.Models;

namespace TaskAssessment.Interfaces;

public interface ITicketRepostiory
{
    public Task<ICollection<Ticket>> ListMyTickets(); //use name claim
    public Task<ICollection<Ticket>> ListTeamTickets();
    public Task<Ticket?> GetById(int TicketId);
    public Task<bool> AddTicket(Ticket Ticket);
    public Task<bool> UpdateTicket(Ticket Ticket,TicketDto TicketDto);
    public Task<bool> RemoveTicket(Ticket Ticket);
    public Task<bool> UpdateStatus(Ticket Ticket, string status);
}
