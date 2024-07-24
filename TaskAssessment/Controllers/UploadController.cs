using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskAssessment.Dto.Upload;
using TaskAssessment.Interfaces;
using TaskAssessment.Models;
using TaskAssessment.Repositories;

namespace TaskAssessment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadRepository _uploadRepository;
        private readonly ITicketRepostiory _ticketRepostiory;
        public UploadController(IUploadRepository uploadRepository, ITicketRepostiory ticketRepostiory)
        {
            _uploadRepository = uploadRepository;
            _ticketRepostiory = ticketRepostiory;
        }
        [HttpGet("{ticketId:int}")]
        [Authorize]
        public async Task<IActionResult> GetUploads(int ticketId)
        {
            var ticket = await _ticketRepostiory.GetById(ticketId);
            if (ticket == null)
            {
                return NotFound($"Ticket with id {ticketId} not found!");
            }
            var uploads = await _uploadRepository.GetUploads(ticketId);
            if (uploads.Count == 0)
            {
                return Ok("No uploads exist for this ticket");
            }
            return Ok(uploads);
        }
        [HttpPost("{ticketId:int}")]
        [Authorize]
        public async Task<IActionResult> CreateUpload(int ticketId, [FromBody] UploadDto newUpload)
        {
            if (!ModelState.IsValid || newUpload == null)
            {
                return BadRequest(ModelState);
            }
            var ticket = await _ticketRepostiory.GetById(ticketId);
            if (ticket == null)
            {
                return NotFound($"Ticket with id {ticketId} not found!");
            }
            var creatingUpload = new Upload()
            {
                Name = newUpload.Name,
                path = newUpload.path,
                Ticket = ticket,
                TicketId = ticketId
            };
            if (await _uploadRepository.AddUpload(creatingUpload))
            {
                return Ok($"{newUpload.Name} added to {ticket.Name}");
            }
            return StatusCode(500, new { Error = "DB Error" });
        }
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteUpload(int id)
        {
            var dbUpload = await _uploadRepository.GetById(id);
            if (dbUpload == null)
                return NotFound($"Upload with id {id} does not exist");
            if (await _uploadRepository.DeleteUpload(dbUpload))
                return Ok($"Deleted {dbUpload.Name}");
            return BadRequest();
        }
    }
}
