using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskAssessment.Models;
using static TaskAssessment.Data.Constants.Enums;

namespace TaskAssessment.Data;

public class ApplicationDbContext : IdentityDbContext<WebUser>
{
    public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        List<IdentityRole> Roles = new()
        {
            new IdentityRole()
            {
                Name = RoleEnum.employee.ToString(),
                NormalizedName = RoleEnum.employee.ToString().ToUpper(),
            },
            new IdentityRole()
            {
               Name = RoleEnum.manager.ToString(),
               NormalizedName = RoleEnum.manager.ToString().ToUpper(),
            },
            new IdentityRole()
            {
               Name = RoleEnum.admin.ToString(),
               NormalizedName = RoleEnum.admin.ToString().ToUpper(),
            },

        };
        builder.Entity<IdentityRole>().HasData(Roles);
    }
    public DbSet<WebUser> WebUsers { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<TicketUser> TicketUsers { get; set; }
    public DbSet<Upload> Uploads { get; set; }
    public DbSet<Note> Notes { get; set; }
}
