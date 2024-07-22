using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAssessment.Dto.Account;
using TaskAssessment.Models;
using static TaskAssessment.Data.Constants.Enums;

namespace TaskAssessment.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly SignInManager<WebUser> _signInManager;
    private readonly UserManager<WebUser> _userManager;
    public AccountController(SignInManager<WebUser> SignInManager, UserManager<WebUser> UserManager)
    {
        _signInManager = SignInManager;
        _userManager = UserManager;
    }
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] AccountDto NewAccount)
    {
        try
        {
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

            var createdRole = await _userManager.AddToRoleAsync(creatingUser, RoleEnum.employee.ToString());
            if (!createdRole.Succeeded)
            {
                return StatusCode(500, createdRole.Errors);
            }

            return Ok(new { creatingUser.UserName });
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
        var signInObj = await _signInManager.CheckPasswordSignInAsync(user, LoginDetails.Password, false);
        if (!signInObj.Succeeded)
        {
            return Unauthorized("Username or Password is incorrect");
        }
        return Ok($"{LoginDetails.UserName} Logged In");

    }
}
