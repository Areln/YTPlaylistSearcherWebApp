﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.Models;

#nullable disable

namespace YTPlaylistSearcherWebApp.Data.Configurations
{
    public partial class PolicyConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> entity)
        {
            entity.ToTable("policies");

            entity.HasIndex(e => e.Id, "ID_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.PolicyName)
                .HasMaxLength(45)
                .HasColumnName("policyName");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Policy> entity);
    }
}
