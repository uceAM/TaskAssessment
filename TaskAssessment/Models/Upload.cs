using System.ComponentModel.DataAnnotations;

namespace TaskAssessment.Models;

public class Upload
{
    [Key]
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string Name { get; set; }
    [Required]
    public string path { get; set; }
}
