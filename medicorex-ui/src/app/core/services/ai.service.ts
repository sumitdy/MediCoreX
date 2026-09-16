import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AiService {

  private baseUrl = environment.apiUrl + '/Ai';

  constructor(private http: HttpClient) {}

  generatePatientSummary(
    fullName: string,
    age: number,
    gender: string
  ) {
    const params = new HttpParams()
      .set('fullName', fullName)
      .set('age', age)
      .set('gender', gender);

    return this.http.post<{
      patientName: string;
      summary: string;
    }>(
      `${this.baseUrl}/patient-summary`,
      null,
      { params }
    );
  }
}