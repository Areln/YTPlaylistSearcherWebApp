import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { PlaylistDTO } from "../DTOs/PlaylistDTO";
import { SharedPostDTO } from "../DTOs/SharedPostDTO";
import { AuthenticatedResponse } from "../login/login.component";

@Injectable({providedIn: 'root'})
export class PlaylistService {

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private sanitizer: DomSanitizer) {

  }

  SubmitLogin(loginData: any)
  {
    return this.http.post<AuthenticatedResponse>(this.baseUrl + 'login/Submit', loginData);
  }

  SubmitRegistration(registerData: any) {
    return this.http.post<AuthenticatedResponse>(this.baseUrl + 'login/Register', registerData);
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

  GetSharedPosts() {
    return this.http.get<SharedPostDTO[]>(this.baseUrl + 'playlist/GetSharedPosts')
  }

  CreateSharedPost(post: any) {
    return this.http.post<number>(this.baseUrl + 'playlist/CreateSharedPost', post)
  }
}
