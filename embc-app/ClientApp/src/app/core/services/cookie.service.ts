import { Injectable, Inject } from '@angular/core';
import { DOCUMENT } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class CookieService {

  constructor(@Inject(DOCUMENT) private document: Document) { }

  set(key: string, value: string, expires?: Date): void {
    let cookieValue = `${key}=${value}`;
    if (expires) {
      cookieValue += `;expires='${expires.toUTCString()}'`;
    }
    this.document.cookie = cookieValue;
  }

  setWithExpiryInYears(key: string, value: string, expires: number) {
    this.setWithExpiryInDays(key, value, expires * 365);
  }

  setWithExpiryInDays(key: string, value: string, expires: number) {
    this.setWithExpiryInHours(key, value, expires * 24);
  }

  setWithExpiryInHours(key: string, value: string, expires: number) {
    this.setWithExpiryInMinutes(key, value, expires * 60);
  }

  setWithExpiryInMinutes(key: string, value: string, expires: number) {
    this.setWithExpiryInSeconds(key, value, expires * 60);
  }

  setWithExpiryInSeconds(key: string, value: string, expires: number) {
    this.setWithExpiryInMilliseconds(key, value, expires * 1000);
  }

  setWithExpiryInMilliseconds(key: string, value: string, expires: number) {
    const expireDate = new Date();
    const time = expireDate.getTime() + expires;
    expireDate.setTime(time);
    this.set(key, value, expireDate);
  }

  get(key: string): string {
    const decodedCookie: string = decodeURIComponent(this.document.cookie);
    const pairs: string[] = decodedCookie.split(/;\s*/);

    const prefix = `${key}=`;
    for (const pair of pairs) {
      if (pair.indexOf(prefix) === 0) {
        return pair.substring(prefix.length);
      }
    }
    return '';
  }

  delete(key: string): void {
    this.set(key, '', new Date('Thu, 01 Jan 1970 00:00:01 GMT'));
  }

  clear(): void {
    const decodedCookie: string = decodeURIComponent(this.document.cookie);
    const pairs: string[] = decodedCookie.split(/;\s*/);

    for (const cookie of pairs) {
      const eqPos = cookie.indexOf('=');
      const cookieName = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
      this.delete(cookieName);
    }
  }
}
