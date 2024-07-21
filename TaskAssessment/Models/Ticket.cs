using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TaskAssessment.Data.Constants.Enums;

namespace TaskAssessment.Models;

public class Ticket
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "varchar")]
    public string Name { get; set; }
    [Required]
    [Column(TypeName = "varchar")]
    public string Status { get; set; } = StatusEnum.pending.ToString();
    [Required]
    public DateTime DueDate { get;}
    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public WebUser? WebUser { get; set; }
    public ICollection<Note>? Notes { get; set; }
    public ICollection<Upload>? Uploads { get; set; }
}
