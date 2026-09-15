import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';

import { PatientService } from '../../core/services/patient.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {

  totalPatients = 0;
  malePatients = 0;
  femalePatients = 0;

  recentPatients: any[] = [];

  loading = false;
  errorMessage = '';

  constructor(
    private patientService: PatientService
  ) {}

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(): void {

    this.loading = true;
    this.errorMessage = '';

    this.patientService.getPatients({
  page: 1,
  pageSize: 100
})
      .subscribe({
        next: (response) => {

          console.log('Dashboard response:', response);

          // Total patients
          this.totalPatients = response.totalRecords;

          // Recent patients
          this.recentPatients = response.data.slice(0, 5);

          // Male patients
          this.malePatients = response.data.filter(
            (patient: any) =>
              patient.gender?.toLowerCase() === 'male'
          ).length;

          // Female patients
          this.femalePatients = response.data.filter(
            (patient: any) =>
              patient.gender?.toLowerCase() === 'female'
          ).length;

          this.loading = false;
        },

        error: (error) => {

          console.error('Dashboard API failed:', error);

          this.loading = false;
          this.errorMessage =
            'Failed to load dashboard data.';
        }
      });
  }
}