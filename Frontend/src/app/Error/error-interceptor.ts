import {
  HttpErrorResponse,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { BroadcastService } from "./broadcast.service";

@Injectable({
  providedIn: "root",
})
export class HttpErrorInterceptorService implements HttpInterceptor {
  constructor(public broadcastService: BroadcastService) {}
  intercept(request: HttpRequest<any>, next: HttpHandler) {
    return next.handle(request).pipe(
      catchError((error: any) => {
        console.log("Interceptor works");
        this.broadcastService.msg.next(error.status.toString());
        this.broadcastService.err.next(error);

        return throwError(error.error); 
      })
    );
  }
}
