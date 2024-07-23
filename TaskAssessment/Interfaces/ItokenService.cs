using TaskAssessment.Models;

namespace TaskAssessment.Interfaces;

public interface ITokenService
{
    public string CreateToken(WebUser user,string role);
}
