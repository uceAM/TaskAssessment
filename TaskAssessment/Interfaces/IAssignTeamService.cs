using TaskAssessment.Models;

namespace TaskAssessment.Interfaces
{
    public interface IAssignTeamService
    {
        public Task<bool> AssignToTeam(WebUser manager, WebUser teamMember);
    }
}
