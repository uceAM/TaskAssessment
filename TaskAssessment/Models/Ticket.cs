using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskAssessment.Data.Constants;

namespace TaskAssessment.Models;

public class Ticket
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "varchar")]
    public string Name { get; set; }
    [Required]
    [Column(TypeName = "varchar")]
    public string Status { get; set; } = StatusConstants.pending;
    [Required]
    public DateTime DueDate { get; set; }
    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public WebUser? WebUser { get; set; }
    public ICollection<Note>? Notes { get; set; }
    public ICollection<Upload>? Uploads { get; set; }
}
