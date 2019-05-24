import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CoreModule } from '../core.module';
import { RestService } from './rest.service';

//
// Ref: https://github.com/bcgov/MyGovBC-CAPTCHA-Widget
//

const BaseUrl = 'https://embcess-captcha.pathfinder.gov.bc.ca';

// payload returned from the server
@Injectable()
export class ServerPayload {
  nonce: string;
  captcha: string;
  validation: string;
  expiry: string;
}

@Injectable({
  providedIn: CoreModule
})
export class CaptchaDataService extends RestService {

  fetchData(nonce: string): Observable<HttpResponse<ServerPayload>> {
    return this.http.post<ServerPayload>(
      BaseUrl + '/captcha',
      { nonce },
      { observe: 'response' }
    );
  }

  verifyCaptcha(nonce: string, answer: string, encryptedAnswer: string): Observable<HttpResponse<ServerPayload>> {
    return this.http.post<ServerPayload>(
      BaseUrl + '/verify/captcha',
      { nonce, answer, validation: encryptedAnswer },
      { observe: 'response' }
    );
  }

  fetchAudio(validation: string, translation?: string): Observable<HttpResponse<string>> {
    const payload: any = { validation };
    if (translation) {
      payload.translation = translation;
    }
    return this.http.post<string>(
      BaseUrl + '/captcha/audio',
      payload,
      { observe: 'response' }
    );
  }

}
