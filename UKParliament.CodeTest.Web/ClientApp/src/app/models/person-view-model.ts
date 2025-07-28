export interface PersonViewModel {
  id: number;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  departmentId: number | null;
  departmentName?: string;
}
