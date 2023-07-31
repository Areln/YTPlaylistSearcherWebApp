import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { FlexLayoutModule } from '@angular/flex-layout';

import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';

@NgModule({
  imports: [
    CommonModule,
    MatSlideToggleModule,
    FlexLayoutModule
  ],
  declarations: [

  ],
  exports: [
    CommonModule,
    FormsModule,
    MatSlideToggleModule,
    MatProgressBarModule,
    MatFormFieldModule,
    MatInputModule,
    MatIconModule,
    MatDividerModule,
    MatButtonModule,
    ReactiveFormsModule,
    MatCardModule,
    FlexLayoutModule
  ]
})
export class SharedModule { }
