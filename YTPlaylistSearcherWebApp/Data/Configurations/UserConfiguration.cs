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
    public partial class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("users");

            entity.HasIndex(e => e.Id, "ID_UNIQUE")
                .IsUnique();

            entity.HasIndex(e => e.AccountStatus, "users_accountstatues_id_idx");

            entity.HasIndex(e => e.Role, "users_roles_id_idx");

            entity.HasIndex(e => e.Authentication, "users_userauthentication_authID_idx");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.AccountStatus).HasColumnName("accountStatus");

            entity.Property(e => e.Authentication).HasColumnName("authentication");

            entity.Property(e => e.Email)
                .HasMaxLength(45)
                .HasColumnName("email");

            entity.Property(e => e.FirstName)
                .HasMaxLength(45)
                .HasColumnName("firstName");

            entity.Property(e => e.LastLoginDate)
                .HasColumnType("datetime")
                .HasColumnName("lastLoginDate")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.LastName)
                .HasMaxLength(45)
                .HasColumnName("lastName");

            entity.Property(e => e.ProfilePicture)
                .HasMaxLength(150)
                .HasColumnName("profilePicture");

            entity.Property(e => e.RegistrationDate)
                .HasColumnType("datetime")
                .HasColumnName("registrationDate")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.Role).HasColumnName("role");

            entity.Property(e => e.UserName)
                .HasMaxLength(45)
                .HasColumnName("userName");

            entity.HasOne(d => d.AccountStatusNavigation)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_accountstatues_id");

            entity.HasOne(d => d.AuthenticationNavigation)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.Authentication)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_userauthentication_authID");

            entity.HasOne(d => d.RoleNavigation)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("users_roles_id");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<User> entity);
    }
}
