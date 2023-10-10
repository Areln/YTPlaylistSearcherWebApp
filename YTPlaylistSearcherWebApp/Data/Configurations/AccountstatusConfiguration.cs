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
    public partial class AccountstatusConfiguration : IEntityTypeConfiguration<Accountstatus>
    {
        public void Configure(EntityTypeBuilder<Accountstatus> entity)
        {
            entity.ToTable("accountstatuses");

            entity.HasIndex(e => e.Id, "ID_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.StatusName)
                .HasMaxLength(45)
                .HasColumnName("statusName");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Accountstatus> entity);
    }
}
