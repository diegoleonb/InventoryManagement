import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

// Bootstrap the main application component with the specified configuration
bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));
