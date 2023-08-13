import { Component, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-scroll-to-top',
  templateUrl: './scroll-to-top.component.html',
  styleUrls: ['./scroll-to-top.component.css']
})
export class ScrollToTopComponent {
  constructor(@Inject(DOCUMENT) private dom: Document) {
  }

  scroll() {
    this.dom.body.scrollTop = 0;
    this.dom.documentElement.scrollTop = 0;
  }
}

