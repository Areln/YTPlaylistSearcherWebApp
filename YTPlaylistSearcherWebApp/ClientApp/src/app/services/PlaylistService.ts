import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { AdvancedSearchRequestDTO } from "../discover-search/discover-search.component";
import { PlaylistDTO, VideoDTO } from "../DTOs/PlaylistDTO";
import { SharedPostDTO } from "../DTOs/SharedPostDTO";
import { AuthenticatedResponse } from "../login/login.component";
import { IVideoCommentDTO } from "../DTOs/IVideoCommentDTO";

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
    return this.http.get<PlaylistDTO[]>(this.baseUrl + 'playlist/GetPlaylists');
  }

  GetSharedPosts() {
    return this.http.get<SharedPostDTO[]>(this.baseUrl + 'playlist/GetSharedPosts');
  }

  CreateSharedPost(post: any) {
    return this.http.post<number>(this.baseUrl + 'playlist/CreateSharedPost', post);
  }

  DeletePost(id: number) {
    return this.http.post<boolean>(this.baseUrl + 'playlist/DeletePost', id);
  }

  SearchVideos(search: AdvancedSearchRequestDTO) {
    return this.http.post<any>(this.baseUrl + 'playlist/SearchVideos', search);
  }

  GetVideoComments(id: number) {
    return this.http.get<IVideoCommentDTO[]>(this.baseUrl + 'playlist/GetVideoComments/' + id);
  }

  CreateComment(videoID: number, commentBody: string) {
    return this.http.post(this.baseUrl + "playlist/CreateComment", { VideoID: videoID, Comment: commentBody });
  }

  DeleteComment(commentID: number) {
    return this.http.post(this.baseUrl + "playlist/DeleteComment", { Id: commentID });
  }

  TmdbSearch(searchString: string) {
    return this.http.get<IVideoCommentDTO[]>(this.baseUrl + 'tmdb/Search/' + searchString);
  }
}
