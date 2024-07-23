using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TaskAssessment.Dto.Note;
using TaskAssessment.Interfaces;
using TaskAssessment.Models;


namespace TaskAssessment.Controllers
{
    [Route("api/notes")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITicketRepostiory _ticketRepostiory;
        public NotesController(INoteRepository noteRepository, ITicketRepostiory ticketRepository)
        {
            _noteRepository = noteRepository;
            _ticketRepostiory = ticketRepository;
        }
        [HttpGet("{ticketId:int}")]
        [Authorize]
        public async Task<IActionResult> GetNotes(int ticketId)
        {
            var ticket = await _ticketRepostiory.GetById(ticketId);
            if (ticket == null)
            {
                return NotFound($"Ticket with id {ticketId} not found!");
            }
            var notes = await _noteRepository.GetNotes(ticketId);
            if (notes.Count == 0)
            {
                return Ok("No notes exist for this ticket");
            }
            return Ok(notes);
        }
        [HttpPost("{ticketId:int}")]
        [Authorize]
        public async Task<IActionResult> CreateNote(int ticketId, [FromBody] NoteDto newNote)
        {
            if (!ModelState.IsValid || newNote == null)
            {
                return BadRequest(ModelState);
            }
            var ticket = await _ticketRepostiory.GetById(ticketId);
            if (ticket == null)
            {
                return NotFound($"Ticket with id {ticketId} not found!");
            }
            var creatingNote = new Note()
            {
                Text = newNote.Text,
                TicketId = ticketId,
            };
            if (await _noteRepository.AddNote(creatingNote))
            {
                return Ok($"{newNote.Text} added to {ticket.Name}");
            }
            return StatusCode(500, new { Error = "DB Error" });
        }
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateNote([FromRoute] int id, [FromBody] NoteDto updateData)
        {
            if (id < 0 || updateData == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbNote = await _noteRepository.GetById(id);
            if (dbNote == null)
                return NotFound();
            if (await _noteRepository.UpdateNote(dbNote, updateData))
                return Ok();
            return BadRequest();
        }
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var dbNote = await _noteRepository.GetById(id);
            if (dbNote == null)
                return NotFound($"Note with id {id} does not exist");
            if (await _noteRepository.DeleteNote(dbNote))
                return Ok($"Deleted {dbNote.Text}");
            return BadRequest();
        }
    }
}
