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
    public partial class SharedpostConfiguration : IEntityTypeConfiguration<Sharedpost>
    {
        public void Configure(EntityTypeBuilder<Sharedpost> entity)
        {
            entity.ToTable("sharedposts");

            entity.HasIndex(e => e.UserId, "FK_SharedPost_User_ID_idx");

            entity.HasIndex(e => e.Id, "ID_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.Content)
                .HasMaxLength(250)
                .HasColumnName("content");

            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");

            entity.Property(e => e.Link)
                .HasMaxLength(300)
                .HasColumnName("link");

            entity.Property(e => e.Thumbnail)
                .HasMaxLength(300)
                .HasColumnName("thumbnail");

            entity.Property(e => e.Type)
                .HasMaxLength(45)
                .HasColumnName("type");

            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.User)
                .WithMany(p => p.Sharedposts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SharedPost_User_ID");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Sharedpost> entity);
    }
}
