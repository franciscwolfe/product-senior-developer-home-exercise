import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PersonViewModel } from '../models/person-view-model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  // Below is some sample code to help get you started calling the API
  getById(id: number): Observable<PersonViewModel> {
    return this.http.get<PersonViewModel>(`${this.baseUrl}api/person/${id}`);
  }

  getAll(): Observable<PersonViewModel[]> {
    return this.http.get<PersonViewModel[]>(`${this.baseUrl}api/person`);
  }

  create(person: PersonViewModel): Observable<PersonViewModel> {
    return this.http.post<PersonViewModel>(`${this.baseUrl}api/person`, person);
  }

  update(person: PersonViewModel): Observable<PersonViewModel> {
    return this.http.put<PersonViewModel>(`${this.baseUrl}api/person/${person.id}`, person);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}api/person/${id}`);
  }
}
