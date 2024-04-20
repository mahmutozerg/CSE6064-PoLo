using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoLoAnalysisBusiness.Core.Models;
using SharedLibrary.Models;
using File = PoLoAnalysisBusiness.Core.Models.File;

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
