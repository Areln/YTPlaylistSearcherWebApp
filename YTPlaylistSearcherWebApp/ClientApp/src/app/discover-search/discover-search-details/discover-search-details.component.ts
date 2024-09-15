import { Component, Inject, Input, SecurityContext } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer } from '@angular/platform-browser';
import { VideoDTO } from '../../DTOs/PlaylistDTO';

@Component({
  selector: 'app-discover-search-details',
  templateUrl: './discover-search-details.component.html',
  styleUrls: ['./discover-search-details.component.css']
})
export class DiscoverSearchDetailsComponent {

  constructor(public dialogRef: MatDialogRef<DiscoverSearchDetailsComponent>,
    private sanitizer: DomSanitizer,
    @Inject(MAT_DIALOG_DATA) public data: VideoDTO)
  {

  }

  public GetVideoID(unsanitizedID: string): string {
    return 'https://www.youtube-nocookie.com/embed/' + unsanitizedID;
    //var sanitizedID = this.sanitizer.sanitize(SecurityContext.URL, 'https://www.youtube-nocookie.com/embed/' + unsanitizedID);
    //console.log(sanitizedID);
    //if (sanitizedID) {
    //  return sanitizedID;
    //}
    //return "";
  }
}
