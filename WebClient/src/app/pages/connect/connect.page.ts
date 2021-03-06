import { Component, OnInit, OnDestroy } from '@angular/core';
import { DroneConnectionService } from '../../services/drone-connection.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-connect',
  templateUrl: './connect.page.html',
  styleUrls: ['./connect.page.scss'],
})
export class ConnectPage implements OnInit, OnDestroy {

  public defaultUrl = localStorage.getItem('url') || `ws://${location.host}/control`;
  public errorMessage: string;
  public loading = false;
  public newUrlUi = false;
  allowEvents = true;

  constructor(private drone: DroneConnectionService, private router: Router) { }


  ngOnInit() {
    this.allowEvents = true;
    this.drone.events.addEventListener('open', () => {
      this.loading = false;
      if (this.allowEvents) {
        this.router.navigate(['/fly']);
      }
    });
    this.drone.events.addEventListener('close', () => {
      this.loading = false;
      if (this.allowEvents) {
        // tslint:disable-next-line:max-line-length
        this.errorMessage = 'Connection lost.';
        this.loading = false;
      }
    });
    // this.loading = true;
    // if (this.router.)
    // this.drone.connect(this.defaultUrl);
  }

  ngOnDestroy() {
    this.allowEvents = false;
  }

  connect(form) {
    this.loading = true;
    try {
      const url = form.value.url || this.defaultUrl;
      this.drone.connect(url);
      localStorage.setItem('url', url);
    } catch (err) {
      this.loading = false;
      this.errorMessage = err.errorMessage;
      console.error(err);
    }
  }

}
