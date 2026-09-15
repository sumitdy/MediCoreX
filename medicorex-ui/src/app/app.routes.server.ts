import { RenderMode, ServerRoute } from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: '**',
    // This is an authenticated SPA. Rendering it on the server triggers
    // browser-only storage access and protected API requests during refresh.
    renderMode: RenderMode.Client
  }
];
