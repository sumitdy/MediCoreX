import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Patient } from '../models/patient.model';
import { CreatePatient } from '../models/create-patient.model';
import { UpdatePatient } from '../models/update-patient.model';

@Injectable({
  providedIn: 'root'
})
export class PatientService {

  private baseUrl = environment.apiUrl + '/patients';

  constructor(private http: HttpClient) {}

  getPatients(params: {
  page?: number;
  pageSize?: number;
  search?: string;
  gender?: string;
  sortBy?: string;
  sortOrder?: string;
}) {
  let queryParams = new URLSearchParams();

  if (params.page) {
    queryParams.set('Page', params.page.toString());
  }

  if (params.pageSize) {
    queryParams.set('PageSize', params.pageSize.toString());
  }

  if (params.search) {
    queryParams.set('Search', params.search);
  }

  if (params.gender) {
    queryParams.set('Gender', params.gender);
  }

  if (params.sortBy) {
    queryParams.set('SortBy', params.sortBy);
  }

  if (params.sortOrder) {
    queryParams.set('SortOrder', params.sortOrder);
  }

  console.log(
  'Patient Filter API:',
  `${this.baseUrl}/filter?${queryParams.toString()}`
);

 return this.http.get<{
  page: number;
  pageSize: number;
  totalRecords: number;
  totalPages: number;
  data: Patient[];
}>(
  `${this.baseUrl}/filter?${queryParams.toString()}`
);
}

  addPatient(patient: CreatePatient) {
    return this.http.post<any>(
      this.baseUrl,
      patient
    );
  }

  updatePatient(id: number, patient: UpdatePatient) {
    return this.http.put<any>(
      `${this.baseUrl}/${id}`,
      patient
    );
  }

  deletePatient(id: number) {
    return this.http.delete<any>(
      `${this.baseUrl}/${id}`
    );
  }
}