import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";

export interface HoneyMediaDTO {
  id: number;
  mediaTitle: string;
  pitch?: string;
  mediaTypeId?: number;
  mediaTypeName?: string;
  mediaTypeColor?: string;
  interestTypeId?: number;
  interestTypeName?: string;
  interestTypeColor?: string;
  requestingUserId?: number;
  requestingUserName?: string;
  requestingUserColor?: string;
  responseId?: number;
  response?: string;
  responseColor?: string;
  statusId?: number;
  status?: string;
  statusColor?: string;
  noelleRating?: number;
  aaronRating?: number;
  noelleComment?: string;
  aaronComment?: string;
  tracker?: string;
  dateRequested?: Date;
  dateFinished?: Date;
  lastUpdated?: Date;
}

export interface MediaTypeDTO {
  id: number;
  mediaTypeName?: string;
  color?: string;
}

export interface MediaInterestDTO {
  id: number;
  interestTypeName?: string;
  color?: string;
}

export interface MediaRequesterDTO {
  id: number;
  name?: string;
  color?: string;
}

export interface MediaResponseDTO {
  id: number;
  response?: string;
  color?: string;
}

export interface MediaStatusDTO {
  id: number;
  status?: string;
  color?: string;
}

@Injectable({ providedIn: 'root' })
export class HoneyMediaService {

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {
  }

  getAll() {
    return this.http.get<HoneyMediaDTO[]>(this.baseUrl + 'honeymedia/GetAll');
  }

  getById(id: number) {
    return this.http.get<HoneyMediaDTO>(this.baseUrl + 'honeymedia/GetById/' + id);
  }

  create(honeyMedia: HoneyMediaDTO) {
    return this.http.post<HoneyMediaDTO>(this.baseUrl + 'honeymedia/Create', honeyMedia);
  }

  update(id: number, honeyMedia: HoneyMediaDTO) {
    return this.http.put<HoneyMediaDTO>(this.baseUrl + 'honeymedia/Update/' + id, honeyMedia);
  }

  delete(id: number) {
    return this.http.delete<{ success: boolean }>(this.baseUrl + 'honeymedia/Delete/' + id);
  }

  getMediaTypes() {
    return this.http.get<MediaTypeDTO[]>(this.baseUrl + 'honeymedia/GetMediaTypes');
  }

  getMediaInterests() {
    return this.http.get<MediaInterestDTO[]>(this.baseUrl + 'honeymedia/GetMediaInterests');
  }

  getMediaRequesters() {
    return this.http.get<MediaRequesterDTO[]>(this.baseUrl + 'honeymedia/GetMediaRequesters');
  }

  getMediaResponses() {
    return this.http.get<MediaResponseDTO[]>(this.baseUrl + 'honeymedia/GetMediaResponses');
  }

  getMediaStatuses() {
    return this.http.get<MediaStatusDTO[]>(this.baseUrl + 'honeymedia/GetMediaStatuses');
  }
}

