import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import{FormsModule, ReactiveFormsModule} from'@angular/forms'
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { GenresComponent } from './genres/genres.component';
import { HeaderComponent } from './core/layout/header/header.component';
import { FooterComponent } from './core/layout/footer/footer.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import {HttpClientModule} from '@angular/common/http';
import { HomeComponent } from './home/home.component';
import { MoviesListComponent } from './movies/movies-list/movies-list.component';
import { MovieDetailsComponent } from './movies/movie-details/movie-details.component';
import { LoginComponent } from './auth/login/login.component';
import { SignUpComponent } from './auth/sign-up/sign-up.component';
import { FavoritesComponent } from './account/favorites/favorites.component';
import { PurchasesComponent } from './account/purchases/purchases.component';
import { CreateMovieComponent } from './admin/create-movie/create-movie.component';
import { CreateCastComponent } from './admin/create-cast/create-cast.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';
import { MovieCardComponent } from './shared/components/movie-card/movie-card.component'

@NgModule({
  declarations: [
    AppComponent,
    GenresComponent,
    HeaderComponent,
    FooterComponent,
    HomeComponent,
    MoviesListComponent,
    MovieDetailsComponent,
    LoginComponent,
    SignUpComponent,
    FavoritesComponent,
    PurchasesComponent,
    CreateMovieComponent,
    CreateCastComponent,
    NotFoundComponent,
    MovieCardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
