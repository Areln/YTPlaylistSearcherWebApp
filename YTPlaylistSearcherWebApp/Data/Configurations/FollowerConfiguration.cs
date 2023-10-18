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
    public partial class FollowerConfiguration : IEntityTypeConfiguration<Follower>
    {
        public void Configure(EntityTypeBuilder<Follower> entity)
        {
            entity.ToTable("followers");

            entity.HasIndex(e => e.Followee, "FK_Followers_FolloweeUserID_idx");

            entity.HasIndex(e => e.Follower1, "FK_Followers_FollowerUserID_idx");

            entity.HasIndex(e => e.Id, "ID_UNIQUE")
                .IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime")
                .HasColumnName("createdDate");

            entity.Property(e => e.Followee).HasColumnName("followee");

            entity.Property(e => e.Follower1).HasColumnName("follower");

            entity.HasOne(d => d.FolloweeNavigation)
                .WithMany(p => p.FollowerFolloweeNavigations)
                .HasForeignKey(d => d.Followee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Followers_FolloweeUserID");

            entity.HasOne(d => d.Follower1Navigation)
                .WithMany(p => p.FollowerFollower1Navigations)
                .HasForeignKey(d => d.Follower1)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Followers_FollowerUserID");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Follower> entity);
    }
}
