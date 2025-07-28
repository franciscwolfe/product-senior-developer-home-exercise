import { Component, OnInit, EventEmitter, Output, Input, OnChanges, SimpleChanges } from '@angular/core';
import { PersonService } from '../../services/person.service';
import { PersonViewModel } from '../../models/person-view-model';

@Component({
  selector: 'app-person-list',
  templateUrl: './person-list.component.html',
  styleUrls: ['./person-list.component.scss']
})
export class PersonListComponent implements OnInit, OnChanges {
  persons: PersonViewModel[] = [];
  selectedPerson: PersonViewModel | null = null;
  @Input() selectedPersonFromParent: PersonViewModel | null = null;
  @Output() personSelected = new EventEmitter<PersonViewModel>();

  constructor(private personService: PersonService) { }

  ngOnInit(): void {
    this.loadPersons();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedPersonFromParent']) {
      this.selectedPerson = this.selectedPersonFromParent;
    }
  }

  loadPersons(): void {
    this.personService.getAll().subscribe(data => this.persons = data);
  }

  selectPerson(person: PersonViewModel): void {
    this.selectedPerson = { ...person };
    this.personSelected.emit(this.selectedPerson);
  }
} 