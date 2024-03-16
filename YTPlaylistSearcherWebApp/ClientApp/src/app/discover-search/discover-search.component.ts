import { Component, SecurityContext } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { DomSanitizer } from '@angular/platform-browser';
import { VideoDTO } from '../DTOs/PlaylistDTO';
import { PlaylistService } from '../services/PlaylistService';
import {
  MatDialog,
  MatDialogRef,
  MatDialogActions,
  MatDialogClose,
  MatDialogTitle,
  MatDialogContent,
} from '@angular/material/dialog';
import { DiscoverSearchDetailsComponent } from './discover-search-details/discover-search-details.component';

@Component({
  selector: 'app-discover-search',
  templateUrl: './discover-search.component.html',
  styleUrls: ['./discover-search.component.css']
})
export class DiscoverSearchComponent {

  searchForm = this.formBuilder.group({
    searchInput: [''],
  });
  results: VideoDTO[] | undefined;
  isLoading: boolean = false;
  errorMessage: string | null | undefined;

  constructor(
    private formBuilder: FormBuilder,
    private sanitizer: DomSanitizer,
    private service: PlaylistService,
    private dialog: MatDialog) {

  }

  private ngOnInit() {
    this.GetResults();
  }

  public GetResults() {

    var searchString = (String)(this.searchForm.controls['searchInput'].value).toLowerCase();

    if (searchString != null) {
      this.service.SearchVideos({ SearchPhrase: searchString } as AdvancedSearchRequestDTO).subscribe(searchResults => {
        this.results = searchResults;
      },
        error => {
          console.error(error);
          this.errorMessage = 'Error!';
          this.isLoading = false;
        });
    }
  }

  public SelectVideo(video: VideoDTO) {
    this.dialog.open(DiscoverSearchDetailsComponent,
      {
        data: video,
        maxWidth: "100%",
        disableClose: false
      });
  }

}

export interface AdvancedSearchRequestDTO {
  SearchPhrase: string,
  orderByDesc: boolean,
  searchChips: SearchChipDTO[]
}

export interface SearchChipDTO {
  chipType: string,
  value: string,
  modifier: string
}
