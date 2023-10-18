﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using YTPlaylistSearcherWebApp.Data.Configurations;
using YTPlaylistSearcherWebApp.Models;
#nullable enable

namespace YTPlaylistSearcherWebApp.Data
{
    public partial class YTPSContext : DbContext
    {
        public YTPSContext()
        {
        }

        public YTPSContext(DbContextOptions<YTPSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accountstatus> Accountstatuses { get; set; } = null!;
        public virtual DbSet<Follower> Followers { get; set; } = null!;
        public virtual DbSet<Playlist> Playlists { get; set; } = null!;
        public virtual DbSet<Playlistsearchhistory> Playlistsearchhistories { get; set; } = null!;
        public virtual DbSet<Policy> Policies { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Rolepolicy> Rolepolicies { get; set; } = null!;
        public virtual DbSet<Sharedpost> Sharedposts { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Userauthentication> Userauthentications { get; set; } = null!;
        public virtual DbSet<Video> Videos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.ApplyConfiguration(new Configurations.AccountstatusConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.FollowerConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PlaylistConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PlaylistsearchhistoryConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.PolicyConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RoleConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.RolepolicyConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SharedpostConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UserauthenticationConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.VideoConfiguration());
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
