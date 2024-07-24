using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using TaskAssessment.Dto.Ticket;
using TaskAssessment.Interfaces;
using TaskAssessment.Models;
using TaskAssessment.Data.Constants;
using TaskAssessment.Extensions;

namespace TaskAssessment.Controllers;

[Route("api/tickets")]
[ApiController]
public class TicketController : ControllerBase
{
    private readonly UserManager<WebUser> _userManager;
    private readonly ITicketRepostiory _ticketRepo;
    public TicketController(UserManager<WebUser> userManger, ITicketRepostiory ticketRepo)
    {
        _userManager = userManger;
        _ticketRepo = ticketRepo;
    }
    [HttpPost]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateTicket([FromBody] TicketDto newData)
    {
        if (!ModelState.IsValid || newData == null)
        {
            return BadRequest(ModelState);
        }
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == newData.UserName.ToLower());
        if (user == null)
        {
            return Unauthorized("Username not registered");
        }
        var creatingTicket = new Ticket()
        {
            Name = newData.Name,
            WebUser = user,
            DueDate = newData.DueDate
        };
        if (await _ticketRepo.AddTicket(creatingTicket))
            return Ok(newData);
        return BadRequest();
    }
    [HttpDelete("{id:int}")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        var dbTicket = await _ticketRepo.GetById(id);
        if (dbTicket == null)
            return NotFound($"Ticket with id {id} does not exist");
        if (await _ticketRepo.RemoveTicket(dbTicket))
            return Ok($"Deleted {dbTicket.Name}");
        return BadRequest();
    }
    [HttpPut("{id:int}")]
    [Authorize]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateTicket([FromRoute] int id, [FromBody] TicketDto updateData)
    {
        if (id < 0 || updateData == null || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var dbTicket = await _ticketRepo.GetById(id);
        if (dbTicket == null)
            return NotFound();
        if (await _ticketRepo.UpdateTicket(dbTicket, updateData))
            return Ok();
        return BadRequest();
    }
    [HttpPut("status/{id:int}")]
    [Authorize]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateTicketStatus(int id, [FromQuery, BindRequired] string status)
    {
        if (id < 0 || string.IsNullOrEmpty(status) || !ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var checkStatus = status.ToLower();
        if (checkStatus == StatusConstants.inProgress || checkStatus == StatusConstants.pending || checkStatus == StatusConstants.complete)
        {
            var dbTicket = await _ticketRepo.GetById(id);
            if (dbTicket == null)
            {
                return NotFound($"Ticket with {id} does not exist in the DB");
            }
            await _ticketRepo.UpdateStatus(dbTicket, status);
            return Ok($"{dbTicket.Name} updated with {dbTicket.Status}");
        }
        return BadRequest("Status cannot be validated"); //ideally never reach this
    }
    [HttpGet("/reports")]
    [Authorize(Roles =RolesConstants.admin)]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GenerateReport([FromQuery, BindRequired] DateTime StartDate,[FromQuery,BindRequired]string Interval)
    {
        if (Interval.ToLower() == "week" || Interval.ToLower() == "month")
        {
            var reports = await _ticketRepo.GetReports(StartDate, Interval);
            return Ok(reports);
        }
        return BadRequest();
    }
    [HttpGet("/team")]
    [Authorize(Roles = RolesConstants.manager)]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetTeamTickets()
    {
        var name =  User.GetUsername();
        var manager = await _userManager.FindByNameAsync(name);
        var tickets = await _ticketRepo.ListTeamTickets(manager.Id);
        return Ok(name);
    }
    [HttpGet]
    [Authorize]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetMyickets()
    {
        var name = User.GetUsername();
        var user = await _userManager.FindByNameAsync(name);
        var tickets = await _ticketRepo.ListTeamTickets(user.Id);
        return Ok(name);
    }

}

