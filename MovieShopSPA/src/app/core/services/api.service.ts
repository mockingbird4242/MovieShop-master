import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(protected http: HttpClient) { }
  getAll(path: string): Observable<any[]> {
    return this.http.get(`${environment.apiUrl}${path}`).pipe(
      map(resp => resp as any[])
    )
  }
  getById(path: string, id?: number): Observable<any> {
    return this.http.get(`${environment.apiUrl}${path}` + '/' + id).pipe(
      map(resp => resp as any)
    )
  }
}
