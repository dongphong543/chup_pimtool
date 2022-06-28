import {
  HttpErrorResponse,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { BroadcastService } from "./broadcast.service";

@Injectable({
  providedIn: "root",
})
export class HttpErrorInterceptorService implements HttpInterceptor {
  constructor(
    public broadcastService: BroadcastService,
    private router: Router
  ) {}
  intercept(request: HttpRequest<any>, next: HttpHandler) {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        console.log(error);
        this.broadcastService.err.next(error);

        if (error.status.toString() == "409") {
          alert("This has been modified or deleted earlier. Navigating back to home screen...");
          this.router.navigate(["/"]);
        }

        else if (error.status.toString() == "400") {
          alert("HTTP STATUS 400: Bad Request!");
        }

        else {
          this.router.navigate(["/error"]);
        }
        return throwError(error.error);
      })
    );
  }
}
