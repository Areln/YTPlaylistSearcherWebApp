import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-video-player',
  templateUrl: './video-player.component.html',
  styleUrls: ['./video-player.component.css']
})
export class VideoPlayerComponent implements OnInit {

  videoId: string | null = null;
  contentType: string | null = null;
  iframeSrc: SafeResourceUrl = '';
  isLoading: boolean = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit() {
    // Get video ID and content type from route parameters
    this.route.paramMap.subscribe(params => {
      this.contentType = params.get('type');
      this.videoId = params.get('id');
      if (this.videoId && this.contentType) {
        this.loadVideo();
      } else {
        this.router.navigate(['/submit-search']);
      }
    });
  }

  loadVideo() {
    if (this.videoId && this.contentType) {
      // Construct the iframe source URL based on content type
      let url: string;
      
      if (this.contentType === 'movie') {
        url = `https://www.vidking.net/embed/movie/${this.videoId}?color=e50914&nextEpisode=true&episodeSelector=true`;
      } else if (this.contentType === 'tv') {
        url = `https://www.vidking.net/embed/tv/${this.videoId}/1/1?color=e50914&nextEpisode=true&episodeSelector=true`;
      } else {
        // Default to movie if content type is not recognized
        url = `https://www.vidking.net/embed/movie/${this.videoId}?color=e50914&nextEpisode=true&episodeSelector=true`;
      }
      
      this.iframeSrc = this.sanitizer.bypassSecurityTrustResourceUrl(url);
      this.isLoading = false;
    }
  }

  goBack() {
    this.location.back();
  }

  goToSearch() {
    this.router.navigate(['/submit-search']);
  }
}
