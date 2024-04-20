using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoLoAnalysisBusiness.Core.Models;
using SharedLibrary.Models;
using File = PoLoAnalysisBusiness.Core.Models.File;

namespace PoLoAnalysisBusiness.Repository.Configurations;

public class FileConfigurations : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder
            .HasOne(f => f.Course)
            .WithOne(c => c.File)
            .HasForeignKey<Course>(c => c.Id);

    }
}