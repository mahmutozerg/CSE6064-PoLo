﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedLibrary.Models;
using File = SharedLibrary.Models.business.File;

namespace PoLoAnalysisBusiness.Repository.Configurations;

public class FileConfigurations : IEntityTypeConfiguration<File>
{
    public void Configure(EntityTypeBuilder<File> builder)
    {
        builder
            .HasOne(f => f.Result) // File has one Result
            .WithOne()
            .HasForeignKey<File>(f => f.Id); // Assuming CourseId is the foreign key

        builder
            .Property(f => f.Path)
            .HasColumnType("nvarchar(450)");

    }
}