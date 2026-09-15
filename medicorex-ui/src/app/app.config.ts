import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter } from '@angular/router';

import { authInterceptor } from './core/interceptors/auth.interceptor';
import { routes } from './app.routes';

export const appConfig = {

  providers: [

    provideRouter(routes),

    provideHttpClient(
      withInterceptors([authInterceptor])
    )

  ]

};