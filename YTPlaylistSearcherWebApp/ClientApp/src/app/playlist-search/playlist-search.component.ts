import { Component, SecurityContext } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { PlaylistDTO, VideoDTO } from '../DTOs/PlaylistDTO';
import { ActivatedRoute } from '@angular/router'
import { PlaylistService } from '../services/PlaylistService';

@Component({
  selector: 'app-playlist-search',
  templateUrl: './playlist-search.component.html',
  styleUrls: ['./playlist-search.component.scss']
})
export class PlaylistSearchComponent {

  isLoading: boolean = false;
  errorMessage: string | null | undefined;
  loadPlaylistForm!: FormGroup;
  resultSearchForm!: FormGroup;
  playlist!: PlaylistDTO | null;
  playlistID!: string;
  defaultSearch: string | null;
  videosToDisplay!: VideoDTO[];
  ytEmbedSource: string = 'https://www.youtube.com/embed/';
  ytEmbed!: SafeResourceUrl;
  displayedColumns: string[] = ['position', 'result'];

  constructor(
    private formBuilder: FormBuilder,
    private sanitizer: DomSanitizer,
    private route: ActivatedRoute,
    private _playlistService: PlaylistService) {

    this.defaultSearch = this.route.snapshot.paramMap.get('id');

    this.loadPlaylistForm = formBuilder.group({
      playlistLink: [this.defaultSearch],
    });

    this.resultSearchForm = formBuilder.group({
      searchInput: [''],
    });

    if (this.defaultSearch != null && this.defaultSearch != '') {
      this.defaultSearch = sanitizer.sanitize(SecurityContext.URL, this.defaultSearch);
      this.PlaylistSubmit();
    }
  }

  public getPlaylistLinkInput(): string {
    var url = this.loadPlaylistForm.controls.playlistLink.value as string;

    if (url == null || url == undefined) {
      return '';
    }

    if (url.includes('playlist?list=')) {
      return this.loadPlaylistForm.controls.playlistLink.value.split('=')[1];
    }
    else {
      return this.loadPlaylistForm.controls.playlistLink.value;
    }
  }

  public getSearchInput(): string {
    return this.resultSearchForm.controls.searchInput.value as string;
  }

  public PlaylistSubmit() {
    this.playlistID = this.getPlaylistLinkInput();

    if (this.playlistID == '') {
      return;
    }

    this.isLoading = true;

    this._playlistService.GetPlaylist(this.playlistID).subscribe(result => {
      this.playlist = result;
      this.videosToDisplay = this.playlist.videos;
      this.isLoading = false;
      this.errorMessage = null;
    },
      error => {
        console.error(error);
        this.errorMessage = 'Error!';
        this.isLoading = false;
      });
  }

  public ResultsSearch() {
    var searchString = this.getSearchInput().toLowerCase();

    if (this.playlist != null) {
      this.videosToDisplay = this.playlist.videos.filter(x => {
        if (x.channelTitle != undefined) {
          if (x.channelTitle.toLowerCase().includes(searchString)) {
            return true;
          }
        }
        if (x.title != undefined) {
          if (x.title.toLowerCase().includes(searchString)) {
            return true;
          }
        }
        return false;
      });
    }
  }

  public ClearSearchString() {
    this.resultSearchForm.controls.searchInput.patchValue('');
    this.ResultsSearch();
  }

  public RefreshPlaylist() {
    this.isLoading = true;

    this._playlistService.RefreshPlaylist(this.playlistID).subscribe(result => {
      this.playlist = result;
      this.videosToDisplay = this.playlist.videos;
      this.isLoading = false;
      this.errorMessage = null;
    },
      error => {
        console.error(error);
        this.errorMessage = 'Error!';
        this.isLoading = false;
      });

  }

  public selectVideo(id: string) {
    this.ytEmbed = this.sanitizer.bypassSecurityTrustResourceUrl((this.ytEmbedSource + id + '?list=' + this.playlistID));
  }

  public DeselectPlaylist() {
    this.ClearSearchString();
    this.playlist = null;
    this.videosToDisplay = [];
    this.loadPlaylistForm.controls.playlistLink.patchValue('');
  }

  public CreateSharedPost(type: string, id: number) {
    this._playlistService.CreateSharedPost({ type: type, contentID: id}).subscribe(result => {
      console.log('new post ID: ' + result);
    });
  }

  public SharePlaylist(playlist: PlaylistDTO) {
    this.CreateSharedPost("playlist", playlist.id);
  }

  public SharePlaylist2() {
    if (this.playlist != null) {
      this.CreateSharedPost("playlist", this.playlist?.id);
    }
  }

  public PlaylistSelected(playlistID: string) {
    this.loadPlaylistForm.controls.playlistLink.patchValue(playlistID);
    this.PlaylistSubmit();
  }
}
