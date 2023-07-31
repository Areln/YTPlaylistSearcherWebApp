
export interface PlaylistDTO {
  playlistID: string;
  playlistTitle: string;
  channelOwner: string;
  videos: VideoDTO[];
}

export interface VideoDTO {
  videoID: string;
  title: string;
  channelTitle: string;
  description: string;
  thumbnail: string;
  publishedDate: string | null;
}
