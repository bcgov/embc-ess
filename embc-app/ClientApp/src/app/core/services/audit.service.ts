import { Injectable } from '@angular/core';
import { RestService } from './rest.service';
import { retry, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuditService extends RestService {

  getAccessAudit(record: string): Observable<string> {
    // collect and return the audit record for access
    return this.http.get<string>(`/api/reports/registration/audit/${record}/json`, { headers: this.headers })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
  getAccessAuditCsv(record: string): Observable<string> {
    // collect and return the audit record for access
    return this.http.get<string>(`/api/reports/registration/audit/${record}`, { headers: this.headers, responseType: 'blob' as 'json' })
      .pipe(
        retry(3),
        catchError(this.handleError)
      );
  }
}
