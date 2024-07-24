using TaskAssessment.Dto.Ticket;
using TaskAssessment.Models;

namespace TaskAssessment.Interfaces;

public interface ITicketRepostiory
{
    public Task<ICollection<MyTicketsDto>> ListMyTickets(string id); //use name claim
    public Task<ICollection<ReportDto>> ListTeamTickets(string managerId);
    public Task<Ticket?> GetById(int TicketId);
    public Task<bool> AddTicket(Ticket Ticket);
    public Task<bool> UpdateTicket(Ticket Ticket,TicketDto TicketDto);
    public Task<bool> RemoveTicket(Ticket Ticket);
    public Task<bool> UpdateStatus(Ticket Ticket, string status);
    public Task<ICollection<ReportDto>> GetReports(DateTime StartDate, string Interval);
}
