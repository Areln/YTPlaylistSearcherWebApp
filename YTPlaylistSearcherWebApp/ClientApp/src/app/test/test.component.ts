import { HttpClient } from '@angular/common/http';
import { Component, Directive, ElementRef, Inject, Input, Renderer2, SecurityContext } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { PlaylistDTO, VideoDTO } from '../DTOs/PlaylistDTO';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent {

  http!: HttpClient;
  baseUrl!: string;
  formBuilder!: FormBuilder;
  sanitizer!: DomSanitizer;
  isLoading: boolean = false;
  errorMessage: string | null | undefined;
  loadPlaylistForm!: FormGroup;
  resultSearchForm!: FormGroup;
  playlist!: PlaylistDTO;
  playlistID!: string;
  videosToDisplay!: VideoDTO[];
  ytEmbedSource: string = 'https://www.youtube.com/embed/';

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, formBuilder: FormBuilder, sanitizer: DomSanitizer) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.formBuilder = formBuilder;
    this.sanitizer = sanitizer;
    this.loadPlaylistForm = formBuilder.group({
      //playlistLink: ['https://www.youtube.com/playlist?list=PLNL_Z6NDFLJZafBMO4kA9PbNBq0t-f9hx'],
      playlistLink: [''],
    });

    this.resultSearchForm = formBuilder.group({
      searchInput: [''],
    });
  }

  public getPlaylistLinkInput(): string {
    var url = this.loadPlaylistForm.controls.playlistLink.value as string;
    if (url.includes('playlist?list=')) {
      this.playlistID = this.loadPlaylistForm.controls.playlistLink.value.split('=')[1];
    }
    else {
      this.playlistID = this.loadPlaylistForm.controls.playlistLink.value;
    }

    return this.playlistID;
  }

  public getSearchInput(): string {
    return this.resultSearchForm.controls.searchInput.value as string;
  }

  public PlaylistSubmit() {

    this.getPlaylistLinkInput();

    this.isLoading = true;

    this.http.get<PlaylistDTO>(this.baseUrl + 'playlist/GetPlaylist?playlistID=' + this.playlistID).subscribe(result => {
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

  public ClearSearchString() {
    this.resultSearchForm.controls.searchInput.patchValue('');
    this.ResultsSearch();
  }

  public RefreshPlaylist() {
    this.isLoading = true;

    this.http.get<PlaylistDTO>(this.baseUrl + 'playlist/RefreshPlaylist?playlistID=' + this.playlistID).subscribe(result => {
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

}

@Directive({
  selector: 'iframe'
})
export class CachedSrcDirective {

  @Input()
  public get cachedSrc(): string {
    return this.elRef.nativeElement.src;
  }
  public set cachedSrc(src: string) {
    if (this.elRef.nativeElement.src !== src) {
      this.renderer.setAttribute(this.elRef.nativeElement, 'src', src);
    }
  }

  constructor(
    private elRef: ElementRef,
    private renderer: Renderer2
  ) { }
}
