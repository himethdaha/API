import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { NavigationExtras, Router } from '@angular/router';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  //using Router cos for certain errors we need to redirect the user to certain pages
  constructor(private router: Router) { }

  /*Can either intercept the request that goes out or the response that comes back in the next*/
  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      //error = what we got in the console from TestError Component
      catchError(error => {
        if (error) {
          switch (error.status) {
            case 400:
              //First 400 error, not the 400 ValidationError
              if (error.error.errors)
              {
                //Validation errors are known as modalStateErrors
                const modalStateErrors = [];
                //Loop over each key in the erros object
                for (const key in error.error.errors) {
                  if (error.error.errors[key]) {
                    modalStateErrors.push(error.error.errors[key]);

                  }
                }

                /*throw modalStateErrors back to the component cos if we take
                 the registration form, then what i need to do is show the validation
                 errors i get from the api beneath the form*/
                throw modalStateErrors.flat();

              
              }
              else {
                console.error(error.statusText, error.status)
              }
              break;

            case 401:
              console.error(error.statusText, error.status)
              break;

            case 404:
              this.router.navigateByUrl('/not-found');
              break;

            case 500:
              /*use the feature of the router to pass in a state to get
              error details from the api*/
              //error: error.error - Exception given by the API
              const navigationExtras: NavigationExtras = { state: { error: error.error } }
              this.router.navigateByUrl('/server-error', navigationExtras);
              break;

            default:
              console.log("Something unexpected went wrong");
              console.log(error);
              break;
          }
        }
        //If we dont catch the error return the error to what was calling the HttpRequest
        return throwError(error);
      }))
  }
}
