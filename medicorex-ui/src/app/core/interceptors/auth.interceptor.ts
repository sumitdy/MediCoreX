import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';

import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const platformId = inject(PLATFORM_ID);
  const authService = inject(AuthService);
  const router = inject(Router);

  // SSR ke time localStorage available nahi hota
  if (!isPlatformBrowser(platformId)) {
    return next(req);
  }

  // Refresh API ko expired access token ke saath intercept nahi karna
  if (req.url.includes('/auth/refresh')) {
    return next(req);
  }

  const token = authService.getToken();

  // Access token attach karo
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(req).pipe(

    catchError((error: HttpErrorResponse) => {

      // Sirf 401 par refresh-token flow
      if (error.status !== 401 || !authService.getRefreshToken()) {
        return throwError(() => error);
      }

      // New access token lene ke liye refresh API call
      return authService.refreshToken().pipe(

        switchMap((response) => {

          // New tokens save karo
          authService.saveToken(response.accessToken);
          authService.saveRefreshToken(response.refreshToken);

          // Original request ko new access token ke saath retry karo
          const retryRequest = req.clone({
            setHeaders: {
              Authorization: `Bearer ${response.accessToken}`
            }
          });

          return next(retryRequest);
        }),

        catchError((refreshError) => {

          // Refresh token bhi invalid/expired hai
          authService.logout();
          router.navigate(['/login']);

          return throwError(() => refreshError);
        })
      );
    })
  );
};