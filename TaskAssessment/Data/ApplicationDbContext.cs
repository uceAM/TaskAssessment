using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskAssessment.Models;
using TaskAssessment.Data.Constants;

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
                Name = RolesConstants.employee,
                NormalizedName = RolesConstants.employee.ToUpper(),
            },
            new IdentityRole()
            {
               Name = RolesConstants.manager,
               NormalizedName = RolesConstants.manager.ToUpper(),
            },
            new IdentityRole()
            {
               Name =RolesConstants.admin,
               NormalizedName = RolesConstants.admin.ToUpper(),
            },

        };
        builder.Entity<IdentityRole>().HasData(Roles);
    }
    public DbSet<WebUser> WebUsers { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Upload> Uploads { get; set; }
    public DbSet<Note> Notes { get; set; }
}
