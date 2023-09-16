import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ILineUpDTO } from '../DTOs/CounterStrikeLineUp';
import { CounterStrikeService } from '../services/CounterStrikeService';

@Component({
  selector: 'app-counter-strike-line-ups-search',
  templateUrl: './counter-strike-line-ups-search.component.html',
  styleUrls: ['./counter-strike-line-ups-search.component.css']
})
export class CounterStrikeLineUpsSearchComponent {

  isLoading: boolean = false;
  errorMessage: string | null | undefined;
  searchForm = new FormGroup({
    input: new FormControl(''),
  });
  results!: ILineUpDTO[];

  constructor(private formBuilder: FormBuilder,
    private _csService: CounterStrikeService) { }

  SubmitSearch() {
    // TODO get input from searchForm
    this._csService.SubmitSearch("").subscribe(data => {
      this.results = data;
    },
    error => {
      console.error(error);
      this.errorMessage = 'Error!';
      this.isLoading = false;
    });

  }

}
