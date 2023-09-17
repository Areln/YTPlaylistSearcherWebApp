import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { ScrollToTopComponent } from './scroll-to-top/scroll-to-top.component';
import { PlaylistSearchComponent } from './playlist-search/playlist-search.component';
import { PlaylistsViewComponent } from './playlists-view/playlists-view.component';
import { CounterStrikeLineUpsSearchComponent } from './counter-strike-line-ups-search/counter-strike-line-ups-search.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    PlaylistSearchComponent,
    ScrollToTopComponent,
    PlaylistsViewComponent,
    CounterStrikeLineUpsSearchComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'search/:id', component: HomeComponent, pathMatch: 'full' },
      { path: 'playlists', component: PlaylistsViewComponent },
      { path: 'cs/lineups', component: CounterStrikeLineUpsSearchComponent }
      //{ path: 'counter', component: CounterComponent },
      //{ path: 'fetch-data', component: FetchDataComponent },

    ]),
    BrowserAnimationsModule,
    SharedModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
