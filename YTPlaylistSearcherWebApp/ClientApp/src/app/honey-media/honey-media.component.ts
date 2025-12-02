import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HoneyMediaService, HoneyMediaDTO, MediaTypeDTO, MediaInterestDTO, MediaRequesterDTO, MediaResponseDTO, MediaStatusDTO } from '../services/HoneyMediaService';

@Component({
  selector: 'app-honey-media',
  templateUrl: './honey-media.component.html',
  styleUrls: ['./honey-media.component.css']
})
export class HoneyMediaComponent implements OnInit {
  honeyMediaList: HoneyMediaDTO[] = [];
  displayedColumns: string[] = [
    'id', 'mediaTitle', 'pitch', 'mediaTypeName', 'interestTypeName',
    'requestingUserName', 'response', 'status', 'noelleRating', 'aaronRating',
    'dateRequested', 'dateFinished', 'actions'
  ];

  mediaTypes: MediaTypeDTO[] = [];
  mediaInterests: MediaInterestDTO[] = [];
  mediaRequesters: MediaRequesterDTO[] = [];
  mediaResponses: MediaResponseDTO[] = [];
  mediaStatuses: MediaStatusDTO[] = [];

  isEditing: boolean = false;
  editingId: number | null = null;
  showForm: boolean = false;
  isLoading: boolean = false;
  errorMessage: string | null = null;

  honeyMediaForm: FormGroup;

  constructor(
    private honeyMediaService: HoneyMediaService,
    private formBuilder: FormBuilder
  ) {
    this.honeyMediaForm = this.formBuilder.group({
      mediaTitle: ['', Validators.required],
      pitch: [''],
      mediaTypeId: [null],
      interestTypeId: [null],
      requestingUserId: [null],
      responseId: [null],
      statusId: [null],
      noelleRating: [null],
      aaronRating: [null],
      noelleComment: [''],
      aaronComment: [''],
      tracker: [''],
      dateRequested: [null],
      dateFinished: [null]
    });
  }

  ngOnInit() {
    this.loadData();
    this.loadLookupData();
  }

  loadData() {
    this.isLoading = true;
    this.errorMessage = null;
    this.honeyMediaService.getAll().subscribe({
      next: (data) => {
        this.honeyMediaList = data;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading honey media:', error);
        this.errorMessage = 'Error loading data. Please try again.';
        this.isLoading = false;
      }
    });
  }

  loadLookupData() {
    this.honeyMediaService.getMediaTypes().subscribe(data => this.mediaTypes = data);
    this.honeyMediaService.getMediaInterests().subscribe(data => this.mediaInterests = data);
    this.honeyMediaService.getMediaRequesters().subscribe(data => this.mediaRequesters = data);
    this.honeyMediaService.getMediaResponses().subscribe(data => this.mediaResponses = data);
    this.honeyMediaService.getMediaStatuses().subscribe(data => this.mediaStatuses = data);
  }

  openAddForm() {
    this.isEditing = false;
    this.editingId = null;
    this.honeyMediaForm.reset();
    this.showForm = true;
  }

  openEditForm(item: HoneyMediaDTO) {
    this.isEditing = true;
    this.editingId = item.id;
    this.honeyMediaForm.patchValue({
      mediaTitle: item.mediaTitle,
      pitch: item.pitch || '',
      mediaTypeId: item.mediaTypeId,
      interestTypeId: item.interestTypeId,
      requestingUserId: item.requestingUserId,
      responseId: item.responseId,
      statusId: item.statusId,
      noelleRating: item.noelleRating,
      aaronRating: item.aaronRating,
      noelleComment: item.noelleComment || '',
      aaronComment: item.aaronComment || '',
      tracker: item.tracker || '',
      dateRequested: item.dateRequested ? new Date(item.dateRequested).toISOString().split('T')[0] : null,
      dateFinished: item.dateFinished ? new Date(item.dateFinished).toISOString().split('T')[0] : null
    });
    this.showForm = true;
  }

  cancelForm() {
    this.showForm = false;
    this.isEditing = false;
    this.editingId = null;
    this.honeyMediaForm.reset();
  }

  onSubmit() {
    if (this.honeyMediaForm.valid) {
      const formValue = this.honeyMediaForm.value;
      const honeyMedia: HoneyMediaDTO = {
        id: this.editingId || 0,
        mediaTitle: formValue.mediaTitle,
        pitch: formValue.pitch || null,
        mediaTypeId: formValue.mediaTypeId || null,
        interestTypeId: formValue.interestTypeId || null,
        requestingUserId: formValue.requestingUserId || null,
        responseId: formValue.responseId || null,
        statusId: formValue.statusId || null,
        noelleRating: formValue.noelleRating || null,
        aaronRating: formValue.aaronRating || null,
        noelleComment: formValue.noelleComment || null,
        aaronComment: formValue.aaronComment || null,
        tracker: formValue.tracker || null,
        dateRequested: formValue.dateRequested ? new Date(formValue.dateRequested) : undefined,
        dateFinished: formValue.dateFinished ? new Date(formValue.dateFinished) : undefined
      };

      this.isLoading = true;
      if (this.isEditing && this.editingId) {
        this.honeyMediaService.update(this.editingId, honeyMedia).subscribe({
          next: () => {
            this.loadData();
            this.cancelForm();
            this.isLoading = false;
          },
          error: (error) => {
            console.error('Error updating honey media:', error);
            this.errorMessage = 'Error updating entry. Please try again.';
            this.isLoading = false;
          }
        });
      } else {
        this.honeyMediaService.create(honeyMedia).subscribe({
          next: () => {
            this.loadData();
            this.cancelForm();
            this.isLoading = false;
          },
          error: (error) => {
            console.error('Error creating honey media:', error);
            this.errorMessage = 'Error creating entry. Please try again.';
            this.isLoading = false;
          }
        });
      }
    }
  }

  deleteItem(id: number) {
    if (confirm('Are you sure you want to delete this entry?')) {
      this.isLoading = true;
      this.honeyMediaService.delete(id).subscribe({
        next: () => {
          this.loadData();
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error deleting honey media:', error);
          this.errorMessage = 'Error deleting entry. Please try again.';
          this.isLoading = false;
        }
      });
    }
  }

  formatDate(date: Date | string | null | undefined): string {
    if (!date) return '';
    const d = new Date(date);
    return d.toLocaleDateString();
  }
}

