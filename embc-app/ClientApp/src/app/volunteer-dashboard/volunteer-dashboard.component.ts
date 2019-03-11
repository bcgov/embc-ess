import { Component, OnInit } from '@angular/core';
import { Registration } from '../core/models';
import { RegistrationService } from '../core/services/registration.service';

interface Stub{
  id: string; // the guid to link them to their file
  restrictedAccess: boolean; // should this file be shown or not?
  essFileNumber: number; // what is the ESS file number
  firstName: string;
  lastName: string;
  incidentTaskTaskNumber: string;
  requiresIncidentals: boolean; // do they need vouchers
  personType: string; // HOH || FMBR || VOLN
  evacuatedFrom: string; // community name
  evacuatedTo: string; // community name
  registrationCompletionDate: Date;

}
@Component({
  selector: 'app-volunteer-dashboard',
  templateUrl: './volunteer-dashboard.component.html',
  styleUrls: ['./volunteer-dashboard.component.scss']
})
export class VolunteerDashboardComponent implements OnInit {

  registrations: Stub[];
  raw;

  constructor(
    private registrationService: RegistrationService
  ) { }

  ngOnInit() {
    // go get the data
    this.registrationService.getRegistries()
      .subscribe((registrations: Registration[]) => {
        this.raw = registrations;
        // save the registrations into the local data blob
        this.registrations = this.unreduceRegistrationToStubs(registrations);
    });
  }
  unreduceRegistrationToStubs(registrations: Registration[]): Stub[]{
    const stubCollector: Stub[] = [];
    // loop through registrations and get each family member
    for (const registration of registrations){
      // push the head of household as a stub
      const hoh: Stub = {
        id: registration.id, // the guid to link them to their file
        restrictedAccess: registration.restrictedAccess, // should this file be shown or not?
        essFileNumber: registration.essFileNumber, // what is the ESS file number
        firstName: registration.headOfHousehold.firstName,
        lastName: registration.headOfHousehold.lastName,
        incidentTaskTaskNumber: registration.incidentTask.taskNumber,
        requiresIncidentals: registration.requiresIncidentals, // do they need vouchers
        personType: 'HOH', // HOH || FMBR || VOLN
        evacuatedFrom: registration.incidentTask.community.name, // community name
        evacuatedTo: registration.hostCommunity.name, // community name
        registrationCompletionDate: registration.registrationCompletionDate
      };
      stubCollector.push(hoh);

      // push the family members of the HOH as stubs
      for (const familyMember of registration.familyMembers){
        const fmbr = {
          id: registration.id, // the guid to link them to their file
          restrictedAccess: registration.restrictedAccess, // should this file be shown or not?
          essFileNumber:  registration.essFileNumber, // what is the ESS file number
          firstName: familyMember.firstName,
          lastName: familyMember.lastName,
          incidentTaskTaskNumber: registration.incidentTask.taskNumber,
          requiresIncidentals: registration.requiresIncidentals, // do they need vouchers
          personType: familyMember.personType, // HOH || FMBR || VOLN
          evacuatedFrom: registration.incidentTask.community.name, // community name
          evacuatedTo: registration.hostCommunity.name, // community name
          registrationCompletionDate: registration.registrationCompletionDate

        };
        stubCollector.push(fmbr);
      }
    }
    return stubCollector;
  }
}
