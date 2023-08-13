import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { PlaylistDTO } from "../DTOs/PlaylistDTO";

@Injectable({providedIn: 'root'})
export class PlaylistService {

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private sanitizer: DomSanitizer) {

  }

  GetPlaylist(id: string) {
    return this.http.get<PlaylistDTO>(this.baseUrl + 'playlist/GetPlaylist?playlistID=' + id);
  }

  RefreshPlaylist(id: string) {
    return this.http.get<PlaylistDTO>(this.baseUrl + 'playlist/RefreshPlaylist?playlistID=' + id);
  }

  GetPlaylists() {
    return this.http.get<PlaylistDTO[]>(this.baseUrl + 'playlist/GetPlaylists')
  }
}
