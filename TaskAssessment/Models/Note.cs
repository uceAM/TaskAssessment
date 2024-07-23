using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskAssessment.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar")]
        public string Text { get; set; }
        public int TicketId { get; set; }
    }
}
