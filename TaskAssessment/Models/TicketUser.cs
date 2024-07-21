
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace TaskAssessment.Models;

public class TicketUser
{
    [Key]
    public int Id { get; set; }
    public WebUser WebUser { get; set; }
    public Ticket Ticket { get; set;}
}
