using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Repository.Configurations;
using SharedLibrary.Models;
using File = SharedLibrary.Models.File;

namespace PoLoAnalysisBusiness.Repository;

public class AppDbContext:DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {   
        //fill it
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.ApplyConfiguration(new FileConfigurations());
        modelBuilder.ApplyConfiguration(new ResultConfigurations());



        
        base.OnModelCreating(modelBuilder);
    }
}