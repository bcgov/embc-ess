import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { WatchdogService, RefreshReason } from '../services/watchdog.service';

@Injectable()
export class WatchdogInterceptor implements HttpInterceptor {

  constructor(
    private watchdog: WatchdogService,
  ) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      map(event => {
        if (event instanceof HttpResponse && event.status === 200) {
          // refresh session watchdog timer
          this.watchdog.refreshWatchdog(RefreshReason.ResponseOk);
        }
        return event;
      })
    );
  }

}
