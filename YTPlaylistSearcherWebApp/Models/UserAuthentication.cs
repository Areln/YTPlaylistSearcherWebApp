﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace YTPlaylistSearcherWebApp.Models
{
    public partial class Userauthentication
    {
        public Userauthentication()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string Password { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public string? RecoveryCode { get; set; }
        public DateTime LastUpdated { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshDate { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}