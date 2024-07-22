using System.ComponentModel.DataAnnotations;

namespace TaskAssessment.Dto.Account;

public class AccountDto
{

    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }

}
