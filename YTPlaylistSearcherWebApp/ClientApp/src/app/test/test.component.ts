import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
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

  isLoading: boolean = false;
  errorMessage: string | null | undefined;
  loadPlaylistForm!: FormGroup;
  resultSearchForm!: FormGroup;
  playlist!: PlaylistDTO;
  videosToDisplay!: VideoDTO[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, formBuilder: FormBuilder) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.formBuilder = formBuilder;

    this.loadPlaylistForm = formBuilder.group({
      //playlistLink: ['https://www.youtube.com/playlist?list=PLNL_Z6NDFLJZafBMO4kA9PbNBq0t-f9hx'],
      playlistLink: [''],
    });

    this.resultSearchForm = formBuilder.group({
      searchInput: [''],
    });
  }

  public getPlaylistLinkInput(): string {
    return this.loadPlaylistForm.controls.playlistLink.value as string;
  }

  public getSearchInput(): string {
    return this.resultSearchForm.controls.searchInput.value as string;
  }

  public PlaylistSubmit() {
    var playlistID = this.getPlaylistLinkInput().includes('https://www.youtube.com/playlist?list=') ? this.loadPlaylistForm.controls.playlistLink.value.split('=')[1] : this.loadPlaylistForm.controls.playlistLink.value;
    this.isLoading = true;

    this.http.get<PlaylistDTO>(this.baseUrl + 'playlist/GetPlaylistFromYT?playlistID=' + playlistID).subscribe(result => {
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
}
