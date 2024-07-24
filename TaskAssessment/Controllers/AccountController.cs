using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAssessment.Dto.Account;
using TaskAssessment.Interfaces;
using TaskAssessment.Models;
using TaskAssessment.Data.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using TaskAssessment.Extensions;

namespace TaskAssessment.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<WebUser> _signInManager;
    private readonly UserManager<WebUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IAssignTeamService _assignTeamService;
    public AccountController(SignInManager<WebUser> SignInManager, UserManager<WebUser> UserManager, ITokenService tokenService, IAssignTeamService assignTeamService)
    {
        _signInManager = SignInManager;
        _userManager = UserManager;
        _tokenService = tokenService;
        _assignTeamService = assignTeamService;
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] AccountDto NewAccount, [FromQuery,BindRequired]string role)
    {
        try
        {
            var assigningRole = RolesConstants.employee;
            switch (role)
            {
                case RolesConstants.manager:
                    {
                        assigningRole = RolesConstants.manager;
                        break;
                    }
                case RolesConstants.admin:
                    {
                        assigningRole = RolesConstants.admin;
                        break;
                    }
                default:
                    {
                        break; //assign as employee
                    }
            }
            if (!ModelState.IsValid || NewAccount.Password == null)
            {
                return BadRequest(ModelState);
            }

            WebUser creatingUser = new()
            {
                UserName = NewAccount.UserName,
            };

            var createdUser = await _userManager.CreateAsync(creatingUser, NewAccount.Password);
            if (!createdUser.Succeeded)
            {
                return StatusCode(500, createdUser.Errors);
            }

            var createdRole = await _userManager.AddToRoleAsync(creatingUser, assigningRole);
            if (!createdRole.Succeeded)
            {
                return StatusCode(500, createdRole.Errors);
            }

            return Ok($"User {creatingUser.UserName} created");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }

    }

    [HttpPost("login")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login([FromBody] AccountDto LoginDetails)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == LoginDetails.UserName.ToLower());
        if (user == null)
        {
            return Unauthorized("Username not registered");
        }
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles[0];
        if (role == null)
        {
            return StatusCode(500, new { error = "No Role found for user" }); //Fatal error. Should be assigned employee by default
        }
        var signInObj = await _signInManager.CheckPasswordSignInAsync(user, LoginDetails.Password, false);
        if (!signInObj.Succeeded)
        {
            return Unauthorized("Username or Password is incorrect");
        }
        var token = _tokenService.CreateToken(user, role);
        return Ok(token);

    }
    [HttpPost("assign-team")]
    [Authorize(Roles = $"{RolesConstants.manager},{RolesConstants.admin}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AssignToTeam([FromQuery,BindRequired]string username)
    {
        if (User.IsInRole(RolesConstants.admin))
        {
            return Ok("This endpoint is for managers only");
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var name = User.GetUsername();
        var manager = await _userManager.FindByNameAsync(name);
        var assigningUser = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());
        if (assigningUser == null)
        {
            return Unauthorized("Username not registered");
        }
        if(await _assignTeamService.AssignToTeam(manager,assigningUser))
        return Ok($"{assigningUser.UserName} assigned to {manager.UserName}'s team");
        return StatusCode(500, new { Error = "team not assigned please try again later" });

    }
}
