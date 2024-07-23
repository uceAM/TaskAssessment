﻿using Microsoft.EntityFrameworkCore;
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
            await _context.Tickets.AddAsync(Ticket);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> RemoveTicket(Ticket Ticket)
    {
        try
        {
            _context.Tickets.Remove(Ticket);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Task<ICollection<Ticket>> ListMyTickets()
    {
        throw new NotImplementedException();
    }

    public Task<ICollection<Ticket>> ListTeamTickets()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateTicket(Ticket Ticket, TicketDto TicketDto)
    {
        Ticket.Name = TicketDto.Name;
        Ticket.DueDate = TicketDto.DueDate;
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

    public async Task<Ticket?> GetById(int TicketId)
    {
        return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == TicketId);
    }

    public async Task<bool> UpdateStatus(Ticket Ticket, string status)
    {
        Ticket.Status = status;
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

    public async Task<ICollection<ReportDto>> GetReports(DateTime StartDate, string Interval)
    {
        DateTime? endDate = null;
        switch (Interval)
        {
            case "week":
                {
                    endDate = StartDate.AddDays(7);
                }
                break;
            case "month":
                {
                    endDate = StartDate.AddDays(30);
                }
                break;
        }
        return _context.Tickets.Where(x => x.DueDate >= StartDate && x.DueDate <= endDate).Select(t => new ReportDto
        {
            Name = t.Name,
            UserName = t.WebUser.UserName,
            DueDate = t.DueDate,
            Status = t.Status
        }).ToList();
    }
}
