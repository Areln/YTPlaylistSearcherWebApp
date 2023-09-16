﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using YTPlaylistSearcherWebApp.Data.CS;
using YTPlaylistSearcherWebApp.Models.CS;

#nullable disable

namespace YTPlaylistSearcherWebApp.Data.CS.Configurations
{
    public partial class ThrowstyleConfiguration : IEntityTypeConfiguration<Throwstyle>
    {
        public void Configure(EntityTypeBuilder<Throwstyle> entity)
        {
            entity.ToTable("throwstyles");

            entity.HasIndex(e => e.Id, "id_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Throwstyle> entity);
    }
}
