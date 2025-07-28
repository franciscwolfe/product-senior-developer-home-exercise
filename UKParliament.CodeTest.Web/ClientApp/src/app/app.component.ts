import { Component, ViewChild } from '@angular/core';
import { PersonListComponent } from './components/person-list/person-list.component';
import { PersonViewModel } from './models/person-view-model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  @ViewChild(PersonListComponent) listComponent!: PersonListComponent;
  selectedPerson: PersonViewModel | null = null;

  onPersonSelected(person: PersonViewModel) {
    this.selectedPerson = person;
  }

  onPersonSaved(person: PersonViewModel) {
    this.selectedPerson = null;
    this.listComponent.loadPersons();
  }

  onCancel() {
    this.selectedPerson = null;
  }

  onPersonDeleted(id: number) {
    this.selectedPerson = null;
    this.listComponent.loadPersons();
  }
}
