import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormsModule,
  ReactiveFormsModule,
  FormBuilder,
  Validators
} from '@angular/forms';

import { PatientService } from '../../core/services/patient.service';
import { Patient } from '../../core/models/patient.model';
import { CreatePatient } from '../../core/models/create-patient.model';
import { UpdatePatient } from '../../core/models/update-patient.model';
import { AiService } from '../../core/services/ai.service';

@Component({
  selector: 'app-patient-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './patient-list.component.html',
  styleUrl: './patient-list.component.scss'
})
export class PatientListComponent implements OnInit {

  patients: Patient[] = [];

  page = 1;
  pageSize = 5;

  totalPages = 1;
  totalRecords = 0;

  loading = false;
  errorMessage = '';

  aiSummary = '';
aiPatientName = '';
showAiSummary = false;

  searchName = '';
  selectedGender = '';
  selectedSortBy = '';
  selectedSortOrder = 'asc';

  hasActiveFilters = false;

  showAddForm = false;
  editingPatientId: number | null = null;

  addPatientForm;


  constructor(
    private service: PatientService,
    private fb: FormBuilder,
    private aiService: AiService
  ) {

    this.addPatientForm = this.fb.group({

      fullName: [
        '',
        [
          Validators.required,
          Validators.minLength(3)
        ]
      ],

      age: [
        null as number | null,
        [
          Validators.required,
          Validators.min(1),
          Validators.max(120)
        ]
      ],

      gender: [
        '',
        Validators.required
      ]

    });
  }


  ngOnInit(): void {
    this.loadPatients();
  }


  // ========================================
  // LOAD PATIENTS
  // ========================================

  loadPatients(): void {

    this.loading = true;
    this.errorMessage = '';

    this.service.getPatients({

      page: this.page,
      pageSize: this.pageSize,

      search: this.searchName.trim(),

      gender: this.selectedGender,

      sortBy: this.selectedSortBy,

      sortOrder: this.selectedSortOrder

    })
    .subscribe({

      next: (res) => {

        console.log('Patient response:', res);

        this.patients = res.data;

        this.totalPages = res.totalPages;

        this.totalRecords = res.totalRecords;

        this.page = res.page;

        this.loading = false;
      },

      error: (error) => {

        console.error(
          'Patient API failed:',
          error
        );

        this.loading = false;

        this.errorMessage =
          'Failed to load patients.';
      }

    });
  }


  // ========================================
  // SEARCH / FILTER / SORT
  // ========================================

  search(): void {

    const name =
      this.searchName.trim();

    this.hasActiveFilters =
      !!name ||
      !!this.selectedGender ||
      !!this.selectedSortBy ||
      this.selectedSortOrder !== 'asc';

    this.page = 1;

    this.loadPatients();
  }


  // ========================================
  // CLEAR FILTERS
  // ========================================

  clearSearch(): void {

    this.searchName = '';

    this.selectedGender = '';

    this.selectedSortBy = '';

    this.selectedSortOrder = 'asc';

    this.hasActiveFilters = false;

    this.page = 1;

    this.loadPatients();
  }


  // ========================================
  // PAGINATION
  // ========================================

  next(): void {

    if (this.page < this.totalPages) {

      this.page++;

      this.loadPatients();
    }
  }


  prev(): void {

    if (this.page > 1) {

      this.page--;

      this.loadPatients();
    }
  }


  // ========================================
  // OPEN ADD FORM
  // ========================================

  openAddForm(): void {

    this.showAddForm = true;

    this.editingPatientId = null;

    this.addPatientForm.reset({

      fullName: '',

      age: null,

      gender: ''

    });
  }


  // ========================================
  // CANCEL FORM
  // ========================================

  cancelForm(): void {

    this.showAddForm = false;

    this.editingPatientId = null;

    this.addPatientForm.reset({

      fullName: '',

      age: null,

      gender: ''

    });
  }


  // ========================================
  // ADD PATIENT
  // ========================================

  onAddPatient(): void {

    if (this.addPatientForm.invalid) {

      this.addPatientForm.markAllAsTouched();

      return;
    }


    const patient: CreatePatient = {

      fullName:
        this.addPatientForm.value.fullName ?? '',

      age:
        this.addPatientForm.value.age ?? 0,

      gender:
        this.addPatientForm.value.gender ?? ''

    };


    this.loading = true;


    this.service.addPatient(patient)
      .subscribe({

        next: () => {

          console.log(
            'Patient added successfully'
          );

          this.cancelForm();

          this.page = 1;

          this.loadPatients();
        },

        error: (error) => {

          console.error(
            'Add patient failed:',
            error
          );

          this.loading = false;

          this.errorMessage =
            'Failed to add patient.';
        }

      });
  }


  // ========================================
  // EDIT PATIENT
  // ========================================

  editPatient(
    patient: Patient
  ): void {

    this.showAddForm = true;

    this.editingPatientId =
      patient.id;


    this.addPatientForm.patchValue({

      fullName:
        patient.fullName,

      age:
        patient.age,

      gender:
        patient.gender

    });
  }


  // ========================================
  // UPDATE PATIENT
  // ========================================

  updatePatient(): void {

    if (
      this.addPatientForm.invalid ||
      this.editingPatientId === null
    ) {

      this.addPatientForm.markAllAsTouched();

      return;
    }


    const patient: UpdatePatient = {

      fullName:
        this.addPatientForm.value.fullName ?? '',

      age:
        this.addPatientForm.value.age ?? 0,

      gender:
        this.addPatientForm.value.gender ?? ''

    };


    this.loading = true;


    this.service.updatePatient(

      this.editingPatientId,

      patient

    )
    .subscribe({

      next: () => {

        console.log(
          'Patient updated successfully'
        );

        this.cancelForm();

        this.loadPatients();
      },

      error: (error) => {

        console.error(
          'Update patient failed:',
          error
        );

        this.loading = false;

        this.errorMessage =
          'Failed to update patient.';
      }

    });
  }


  // ========================================
  // SAVE PATIENT
  // ========================================

  savePatient(): void {

    if (
      this.editingPatientId !== null
    ) {

      this.updatePatient();

    } else {

      this.onAddPatient();

    }
  }


  // ========================================
  // DELETE PATIENT
  // ========================================

  deletePatient(
    id: number
  ): void {

    const confirmed =
      confirm(
        'Are you sure you want to delete this patient?'
      );


    if (!confirmed) {
      return;
    }


    this.loading = true;


    this.service.deletePatient(id)
      .subscribe({

        next: () => {

          console.log(
            'Patient deleted successfully'
          );


          if (
            this.patients.length === 1 &&
            this.page > 1
          ) {

            this.page--;
          }


          this.loadPatients();
        },

        error: (error) => {

          console.error(
            'Delete patient failed:',
            error
          );

          this.loading = false;

          this.errorMessage =
            'Failed to delete patient.';
        }

      });
  }

  
generateAiSummary(patient: Patient): void {
  this.loading = true;
  this.errorMessage = '';
  this.showAiSummary = false;

  this.aiService.generatePatientSummary(
    patient.fullName,
    patient.age,
    patient.gender
  )
  .subscribe({
    next: (res) => {
      console.log('AI Summary:', res);

      this.aiPatientName = res.patientName;
      this.aiSummary = res.summary;
      this.showAiSummary = true;

      this.loading = false;
    },
    error: (error) => {
      console.error('AI Summary failed:', error);

      this.loading = false;
      this.errorMessage = 'Failed to generate AI summary.';
    }
  });
}
  // ========================================
  // FORM VALIDATION
  // ========================================

  isInvalid(
    controlName: string
  ): boolean {

    const control =
      this.addPatientForm.get(
        controlName
      );


    return !!(
      control &&
      control.invalid &&
      control.touched
    );
  }


  // ========================================
  // FORM CONTROLS
  // ========================================

  get fullNameControl() {

    return this.addPatientForm.get(
      'fullName'
    );

  }


  get ageControl() {

    return this.addPatientForm.get(
      'age'
    );

  }


  get genderControl() {

    return this.addPatientForm.get(
      'gender'
    );

  }

}