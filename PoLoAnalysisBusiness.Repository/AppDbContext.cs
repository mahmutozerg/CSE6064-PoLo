using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Repository.Configurations;
using SharedLibrary.Models;
using SharedLibrary.Models.business;
using File = SharedLibrary.Models.business.File;

namespace PoLoAnalysisBusiness.Repository;

public class AppDbContext:DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<File> Files { get; set; }
    public DbSet<AppUser> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {   
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.ApplyConfiguration(new FileConfigurations());
        modelBuilder.ApplyConfiguration(new ResultConfigurations());



        
        base.OnModelCreating(modelBuilder);
    }
}