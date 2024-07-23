using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskAssessment.Interfaces;


namespace TaskAssessment.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITicketRepostiory _ticketRepostiory;
        public NotesController(INoteRepository noteRepository,ITicketRepostiory ticketRepository)
        {
            _noteRepository = noteRepository;
            _ticketRepostiory = ticketRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetNotes([FromQuery,BindRequired]int ticketId)
        {
            var ticket = await _ticketRepostiory.GetById(ticketId);
            if (ticket == null)
            {
                return NotFound($"Ticket with id {ticketId} not found!");
            }
            var notes = await _noteRepository.GetNotes(ticketId);
            if(notes.Count ==0)
            {
                return Ok("No notes exist for this ticket");
            }
            return Ok(notes);
        }
    }
}
