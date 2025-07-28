import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { PersonViewModel } from '../../models/person-view-model';
import { DepartmentService } from '../../services/department.service';
import { DepartmentViewModel } from '../../models/department-view-model';
import { PersonService } from '../../services/person.service';

@Component({
  selector: 'app-person-editor',
  templateUrl: './person-editor.component.html',
  styleUrls: ['./person-editor.component.scss']
})
export class PersonEditorComponent implements OnChanges {
  @Input() person: PersonViewModel | null = null;
  @Output() saved = new EventEmitter<PersonViewModel>();
  @Output() cancelled = new EventEmitter<void>();
  @Output() deleted = new EventEmitter<number>();

  form: FormGroup;
  departments: DepartmentViewModel[] = [];
  showConfirmDialog = false;

  constructor(
    private fb: FormBuilder,
    private departmentService: DepartmentService,
    private personService: PersonService
  ) {
    this.form = this.fb.group({
      id: [0],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      departmentId: [null, [Validators.required, this.departmentValidator]]
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['person']) {
      if (this.person) {
        // Convert date to YYYY-MM-DD format for HTML date input
        let dateOfBirth = '';
        if (this.person.dateOfBirth) {
          const date = new Date(this.person.dateOfBirth);
          if (!isNaN(date.getTime())) {
            dateOfBirth = date.toISOString().split('T')[0];
          }
        }
        
        this.form.patchValue({
          id: this.person.id,
          firstName: this.person.firstName,
          lastName: this.person.lastName,
          dateOfBirth: dateOfBirth,
          departmentId: this.person.departmentId
        });
      } else {
        this.form.reset({ id: 0, firstName: '', lastName: '', dateOfBirth: '', departmentId: null });
      }
      this.loadDepartments();
    }
  }

  loadDepartments(): void {
    this.departmentService.getAll().subscribe(depts => this.departments = depts);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    
    const departmentId = this.form.value.departmentId;
    if (departmentId === null || departmentId === 0) {
      // This shouldn't happen due to validation, but add safety check
      this.form.get('departmentId')?.setErrors({ required: true });
      this.form.markAllAsTouched();
      return;
    }
    
    const vm: PersonViewModel = {
      id: this.form.value.id,
      firstName: this.form.value.firstName,
      lastName: this.form.value.lastName,
      dateOfBirth: this.form.value.dateOfBirth,
      departmentId: departmentId,
      departmentName: this.departments.find(d => d.id === departmentId)?.name || ''
    };
    
    if (vm.id === 0) {
      this.personService.create(vm).subscribe({
        next: (created) => {
          this.saved.emit(created);
        },
        error: (error) => {
          console.error('Error creating person:', error);
        }
      });
    } else {
      this.personService.update(vm).subscribe({
        next: (updated) => {
          this.saved.emit(updated);
        },
        error: (error) => {
          console.error('Error updating person:', error);
        }
      });
    }
  }

  onCancel(): void {
    this.cancelled.emit();
  }

  showDeleteConfirmation(): void {
    this.showConfirmDialog = true;
  }

  hideDeleteConfirmation(): void {
    this.showConfirmDialog = false;
  }

  getPersonFullName(): string {
    const firstName = this.form.value.firstName;
    const lastName = this.form.value.lastName;
    return `${firstName} ${lastName}`.trim() || 'this person';
  }

  confirmDelete(): void {
    const id = this.form.value.id;
    if (!id) {
      return;
    }
    
    this.personService.delete(id).subscribe({
      next: () => {
        this.hideDeleteConfirmation();
        this.deleted.emit(id);
      },
      error: (error) => {
        console.error('Error deleting person:', error);
        this.hideDeleteConfirmation();
        alert('Error deleting person. Please try again.');
      }
    });
  }

  // Custom validator for department selection
  departmentValidator(control: any) {
    const value = control.value;
    if (value === null || value === 0 || value === '') {
      return { departmentRequired: true };
    }
    return null;
  }
} 