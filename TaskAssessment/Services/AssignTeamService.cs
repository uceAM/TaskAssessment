using TaskAssessment.Data;
using TaskAssessment.Interfaces;
using TaskAssessment.Models;

namespace TaskAssessment.Services;

public class AssignTeamService : IAssignTeamService
{
    private readonly ApplicationDbContext _context;
    public AssignTeamService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> AssignToTeam(WebUser manager, WebUser teamMember)
    {
        teamMember.Manager = manager;
        await _context.SaveChangesAsync();
        return true;
    }
}
