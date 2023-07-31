﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable enable
using System;
using System.Collections.Generic;

namespace YTPlaylistSearcherWebApp.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            Videos = new HashSet<Video>();
        }

        public int Id { get; set; }
        public string PlaylistId { get; set; } = null!;
        public string? PlaylistTitle { get; set; }
        public string? Thumbnail { get; set; }

        public virtual ICollection<Video> Videos { get; set; }
    }
}