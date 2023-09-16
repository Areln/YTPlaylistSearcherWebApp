import { HttpClient } from "@angular/common/http";
import { Inject, Injectable } from "@angular/core";
import { DomSanitizer } from "@angular/platform-browser";
import { ILineUpDTO } from "../DTOs/CounterStrikeLineUp";

@Injectable({ providedIn: 'root' })
export class CounterStrikeService {

  constructor(private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private sanitizer: DomSanitizer) {

  }

  SubmitSearch(searchString: string) {
    return this.http.get<ILineUpDTO[]>(this.baseUrl + 'lineups/GetLineUps');
    //return this.http.get<ILineUpDTO[]>(this.baseUrl + 'LineUps/GetLineUps?ss=' + searchString);
  }

}
