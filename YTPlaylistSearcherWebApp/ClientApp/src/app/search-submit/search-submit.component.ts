import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { VideoSearchDTO } from '../DTOs/VideoSearchDTO';
import { TMDBService, TMDBSearchRequest } from '../services/TMDBService';

@Component({
  selector: 'app-search-submit',
  templateUrl: './search-submit.component.html',
  styleUrls: ['./search-submit.component.css']
})
export class SearchSubmitComponent {

  searchForm: FormGroup;
  isSubmitting: boolean = false;
  submitMessage: string | null = null;
  searchResults: VideoSearchDTO[] = [];
  hasSearched: boolean = false;
  totalResults: number = 0;
  currentPage: number = 1;
  totalPages: number = 1;
  includeAdult: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private tmdbService: TMDBService
  ) {
    this.searchForm = this.formBuilder.group({
      searchQuery: ['', [Validators.required, Validators.minLength(1)]],
      contentType: ['movie', [Validators.required]],
      includeAdult: [false]
    });
  }

  ngOnInit() {
    // Component initialization
  }

  onSubmit() {
    if (this.searchForm.valid) {
      this.isSubmitting = true;
      this.submitMessage = null;
      this.searchResults = [];
      this.hasSearched = true;

      const searchQuery = this.searchForm.get('searchQuery')?.value;
      const contentType = this.searchForm.get('contentType')?.value;
      const includeAdult = this.searchForm.get('includeAdult')?.value || false;
      
      console.log('Submitting search request:', { query: searchQuery, type: contentType, includeAdult });
      
      // Call TMDB service
      const searchRequest: TMDBSearchRequest = {
        query: searchQuery,
        page: 1,
        includeAdult: includeAdult
      };

      this.tmdbService.searchContent(contentType, searchRequest).subscribe({
        next: (response) => {
          this.isSubmitting = false;
          this.searchResults = response.results;
          this.totalResults = response.totalResults;
          this.currentPage = response.page;
          this.totalPages = response.totalPages;
          this.includeAdult = includeAdult;
          
          this.submitMessage = `Found ${this.totalResults} results for "${searchQuery}"`;
        },
        error: (error) => {
          this.isSubmitting = false;
          console.error('Error searching content:', error);
          this.submitMessage = 'Error searching for content. Please try again.';
          this.searchResults = [];
        }
      });
    } else {
      this.submitMessage = 'Please enter a valid search query.';
    }
  }

  onClear() {
    this.searchForm.reset();
    this.searchForm.patchValue({ contentType: 'movie', includeAdult: false }); // Reset to default values
    this.submitMessage = null;
    this.searchResults = [];
    this.hasSearched = false;
    this.totalResults = 0;
    this.currentPage = 1;
    this.totalPages = 1;
    this.includeAdult = false;
  }

  onPageChange(page: number) {
    if (page >= 1 && page <= this.totalPages && page !== this.currentPage) {
      this.currentPage = page;
      this.performSearch();
    }
  }

  private performSearch() {
    const searchQuery = this.searchForm.get('searchQuery')?.value;
    const contentType = this.searchForm.get('contentType')?.value;
    const includeAdult = this.searchForm.get('includeAdult')?.value || false;
    
    this.isSubmitting = true;
    this.submitMessage = null;
    this.searchResults = [];
    this.hasSearched = true;

    const searchRequest: TMDBSearchRequest = {
      query: searchQuery,
      page: this.currentPage,
      includeAdult: includeAdult
    };

    this.tmdbService.searchContent(contentType, searchRequest).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.searchResults = response.results;
        this.totalResults = response.totalResults;
        this.currentPage = response.page;
        this.totalPages = response.totalPages;
        this.includeAdult = includeAdult;
        
        this.submitMessage = `Found ${this.totalResults} results for "${searchQuery}"`;
      },
      error: (error) => {
        this.isSubmitting = false;
        console.error('Error searching content:', error);
        this.submitMessage = 'Error searching for content. Please try again.';
        this.searchResults = [];
      }
    });
  }

  selectVideo(video: VideoSearchDTO) {
    // Navigate to video player page with video ID and content type
    this.router.navigate(['/video-player', video.contentType, video.id]);
  }
}
