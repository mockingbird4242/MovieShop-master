import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { FavoritesComponent } from './account/favorites/favorites.component';
import { PurchasesComponent } from './account/purchases/purchases.component';
import { CreateCastComponent } from './admin/create-cast/create-cast.component';
import { CreateMovieComponent } from './admin/create-movie/create-movie.component';
import { LoginComponent } from './auth/login/login.component';
import { SignUpComponent } from './auth/sign-up/sign-up.component';
import { HomeComponent } from './home/home.component';
import { MovieDetailsComponent } from './movies/movie-details/movie-details.component';
import { MoviesListComponent } from './movies/movies-list/movies-list.component';
import { NotFoundComponent } from './shared/components/not-found/not-found.component';

const routes: Routes = [
  {path: "", component : HomeComponent},
  {path: 'movies/toprated', component: MoviesListComponent},
  {path: 'movies/topgrossing', component: MoviesListComponent},
  {path: 'movies/:id', component: MovieDetailsComponent},
  {path: 'login', component: LoginComponent},
  {path: 'register', component: SignUpComponent},
  {path: 'account/favorites', component: FavoritesComponent},
  {path: 'account/purchases', component: PurchasesComponent},
  {path: 'admin/createmovie', component: CreateMovieComponent},
  {path: 'admin/createcast', component: CreateCastComponent},
  {path: "**", component: NotFoundComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
