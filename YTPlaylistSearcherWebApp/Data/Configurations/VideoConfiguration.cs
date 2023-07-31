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
    public partial class VideoConfiguration : IEntityTypeConfiguration<Video>
    {
        public void Configure(EntityTypeBuilder<Video> entity)
        {
            entity.ToTable("videos");

            entity.HasIndex(e => e.PlaylistId, "FK_VIDEOS_PLAYLISTS_idx");

            entity.HasIndex(e => e.Id, "ID_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.ChannelTitle)
                .HasMaxLength(100)
                .HasColumnName("channelTitle");

            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");

            entity.Property(e => e.PlaylistId).HasColumnName("playlistID");

            entity.Property(e => e.PublishedDate).HasColumnType("datetime");

            entity.Property(e => e.Thumbnail)
                .HasMaxLength(150)
                .HasColumnName("thumbnail");

            entity.Property(e => e.Title).HasMaxLength(100);

            entity.Property(e => e.VideoId)
                .HasMaxLength(100)
                .HasColumnName("videoID");

            entity.HasOne(d => d.Playlist)
                .WithMany(p => p.Videos)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.PlaylistId)
                .HasConstraintName("FK_VIDEOS_PLAYLISTS");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Video> entity);
    }
}
