using Microsoft.AspNetCore.Identity;

namespace TaskAssessment.Models;

public class WebUser:IdentityUser
{
    public WebUser? Manager { get; set; }
    public ICollection<Ticket>? Ticket { get; set; }
}
