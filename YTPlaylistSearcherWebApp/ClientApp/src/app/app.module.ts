import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { ScrollToTopComponent } from './scroll-to-top/scroll-to-top.component';
import { PlaylistSearchComponent } from './playlist-search/playlist-search.component';
import { PlaylistsViewComponent } from './playlists-view/playlists-view.component';
import { CounterStrikeLineUpsSearchComponent } from './counter-strike-line-ups-search/counter-strike-line-ups-search.component';
import { LoginComponent } from './login/login.component';
import { JwtModule } from "@auth0/angular-jwt";
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './AuthGuard';
import { SharedPostsFeedComponent } from './shared-posts-feed/shared-posts-feed.component';
import { MyProfileComponent } from './my-profile/my-profile.component';
import { MyPostsComponent } from './my-profile/my-posts/my-posts.component';
import { SuikaCloneComponent } from './suika-clone/suika-clone.component';

export function tokenGetter() {
  return localStorage.getItem("jwt");
}

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
    CounterStrikeLineUpsSearchComponent,
    LoginComponent,
    SharedPostsFeedComponent,
    MyProfileComponent,
    MyPostsComponent,
    SuikaCloneComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7298"],
        disallowedRoutes: []
      }
    }),
    RouterModule.forRoot([
      { path: 'loggedout', component: LoginComponent },
      { path: 'search/:id', component: PlaylistSearchComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'search', component: PlaylistSearchComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: '', component: PlaylistSearchComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'cs/lineups', component: CounterStrikeLineUpsSearchComponent, canActivate: [AuthGuard] },
      { path: 'shared', component: SharedPostsFeedComponent },
      { path: 'suika', component: SuikaCloneComponent }
      //{ path: 'counter', component: CounterComponent },
      //{ path: 'fetch-data', component: FetchDataComponent },

    ]),
    BrowserAnimationsModule,
    SharedModule,
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
