import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MovieCard } from 'src/app/shared/models/movie-card';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  constructor(private apiService: ApiService) { }
  getTopRevenueMovies(): Observable<MovieCard[]> {
    return this.apiService.getAll('movies/toprevenue');
  }
}
