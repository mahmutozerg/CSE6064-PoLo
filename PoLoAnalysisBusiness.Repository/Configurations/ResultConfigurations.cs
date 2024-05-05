using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLibrary.Models;
using SharedLibrary.Models.business;
using File = SharedLibrary.Models.business.File;

namespace PoLoAnalysisBusiness.Repository.Configurations;

public class ResultConfigurations : IEntityTypeConfiguration<Result>
{
    public void Configure(EntityTypeBuilder<Result> builder)
    {
        builder
            .HasOne(r => r.File)
            .WithOne(f => f.Result)
            .HasForeignKey<Result>(r => r.FileId);

    }
}
