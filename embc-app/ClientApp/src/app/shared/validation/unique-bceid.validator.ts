import { Injectable } from '@angular/core';
import { AsyncValidator, AbstractControl, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { VolunteerService } from 'src/app/core/services/volunteer.service';

@Injectable({ providedIn: 'root' })
export class UniqueBceidValidator implements AsyncValidator {

  constructor(private volunteerService: VolunteerService) { }

  validate(ctrl: AbstractControl): Observable<ValidationErrors | null> {
    return this.volunteerService.isBceidTaken(ctrl.value)
      .pipe(
        map(isTaken => (isTaken ? { uniqueBceid: true } : null)),
        catchError(() => null)
      );
  }
}
