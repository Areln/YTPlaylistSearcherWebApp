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
    public partial class GrenadetypeConfiguration : IEntityTypeConfiguration<Grenadetype>
    {
        public void Configure(EntityTypeBuilder<Grenadetype> entity)
        {
            entity.ToTable("grenadetypes");

            entity.HasIndex(e => e.Id, "id_UNIQUE")
                .IsUnique();

            entity.HasIndex(e => e.Name, "name_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Grenadetype> entity);
    }
}