﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace YTPlaylistSearcherWebApp.Models
{
    public partial class Follower
    {
        public int Id { get; set; }
        public int Follower1 { get; set; }
        public int Followee { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual User FolloweeNavigation { get; set; } = null!;
        public virtual User Follower1Navigation { get; set; } = null!;
    }
}