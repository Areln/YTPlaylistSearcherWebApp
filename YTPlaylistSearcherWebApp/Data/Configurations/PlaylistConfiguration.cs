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
    public partial class PlaylistConfiguration : IEntityTypeConfiguration<Playlist>
    {
        public void Configure(EntityTypeBuilder<Playlist> entity)
        {
            entity.HasKey(e => new { e.Id, e.PlaylistId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("playlists");

            entity.HasIndex(e => e.Id, "playlistID_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ID");

            entity.Property(e => e.PlaylistId)
                .HasMaxLength(150)
                .HasColumnName("playlistID");

            entity.Property(e => e.PlaylistTitle)
                .HasMaxLength(150)
                .HasColumnName("playlistTitle");

            entity.Property(e => e.Thumbnail)
                .HasMaxLength(200)
                .HasColumnName("thumbnail");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Playlist> entity);
    }
}
