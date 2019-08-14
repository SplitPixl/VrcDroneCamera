import { GamepadService } from 'src/app/services/gamepad.service';
import { DroneConnectionService } from './../services/drone-connection.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';


@Injectable()
export class ConnectionActivate implements CanActivate {
  constructor(private gamepad: GamepadService, private drone: DroneConnectionService, private router: Router) { }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    if (!this.drone.isConnected()) {
      this.router.navigate(['/connect']);
    }
    return true;
  }
}