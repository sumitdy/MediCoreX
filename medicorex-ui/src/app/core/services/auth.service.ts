import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private baseUrl = environment.apiUrl + '/auth';

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) private platformId: object
  ) {}

  login(data: any) {
    return this.http.post<any>(`${this.baseUrl}/login`, data);
  }

  register(data: any) {
  return this.http.post<any>(
    `${this.baseUrl}/register`,
    data
  );
}

  saveToken(token: string) {
    if (isPlatformBrowser(this.platformId)) {
      localStorage.setItem('token', token);
    }
  }

  saveRefreshToken(refreshToken: string): void {
  if (isPlatformBrowser(this.platformId)) {
    localStorage.setItem('refreshToken', refreshToken);
  }
}

  getToken() {
    return isPlatformBrowser(this.platformId)
      ? localStorage.getItem('token')
      : null;
  }

  getRefreshToken(): string | null {
  return isPlatformBrowser(this.platformId)
    ? localStorage.getItem('refreshToken')
    : null;
}

refreshToken() {
  const refreshToken = this.getRefreshToken();

  return this.http.post<any>(
    `${this.baseUrl}/refresh`,
    {
      refreshToken: refreshToken
    }
  );
}

  logout(): void {
  if (isPlatformBrowser(this.platformId)) {
    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
  }
}
}
