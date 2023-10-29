
export interface PlaylistDTO {
  id: number,
  playlistID: string;
  playlistTitle: string;
  channelOwner: string;
  videos: VideoDTO[];
}

export interface VideoDTO {
  id: number,
  videoID: string;
  title: string;
  channelTitle: string;
  description: string;
  thumbnail: string;
  publishedDate: string | null;
}
