using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TaskAssessment.Data;
using TaskAssessment.Dto.Ticket;
using TaskAssessment.Interfaces;
using TaskAssessment.Models;

namespace TaskAssessment.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly UserManager<WebUser> _userManager;
        private readonly ITicketRepostiory _repo;
        public TicketController(UserManager<WebUser> UserManger,ITicketRepostiory repo)
        {
            _userManager = UserManger;
            _repo = repo;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateTicket([FromBody] TicketDto TicketDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == TicketDto.UserName.ToLower());
            if (user == null)
            {
                return Unauthorized("Username not registered");
            }
            var creatingTicket = new Ticket()
            {

                Name = TicketDto.Name,
                DueDate = TicketDto.DueDate
            };
            if (await _repo.AddTicket(creatingTicket))
            return Ok(creatingTicket);
            return BadRequest();
        }
    }
}
