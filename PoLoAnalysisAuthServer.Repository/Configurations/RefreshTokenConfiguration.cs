using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PoLoAnalysisAuthServer.Core.Models;

namespace PoLoAnalysisAuthServer.Repository.Configurations;

public class RefreshTokenConfiguration:IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.HasKey(t => t.UserId);
        builder.Property(t => t.Token).IsRequired();
    }
}