namespace bitirmeProje.Domain.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using bitirmeProje.Domain.Entities;

public class Context : DbContext
{
    public Context()
    {

    }
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Gender> Gender { get; set; }
    public DbSet<Group> Group { get; set; }
    public DbSet<GroupUser> GroupUser { get; set; }
    public DbSet<Option> Option { get; set; }
    public DbSet<Question> Question { get; set; }
    public DbSet<Survey> Survey { get; set; }
    public DbSet<Vote> Vote { get; set; }
}

