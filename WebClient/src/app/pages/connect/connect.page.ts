import { Component, OnInit } from '@angular/core';
import { VrcConnectionService } from './../../services/vrc-connection.service';
import { WebsocketService } from './../../services/websocket.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-connect',
  templateUrl: './connect.page.html',
  styleUrls: ['./connect.page.scss'],
})
export class ConnectPage implements OnInit {

  public defaultUrl = localStorage.getItem('url') || 'ws://localhost:35000/control';
  public errorMessage: string;
  public loading = false;
  public newUrlUi = false;

  constructor(private vrc: VrcConnectionService, private ws: WebsocketService, private router: Router) { }

  ngOnInit() {
    this.ws.events.addEventListener('open', () => { this.router.navigate(['/controller']); });
    this.ws.events.addEventListener('error', () => {
      // tslint:disable-next-line:max-line-length
      this.errorMessage = 'Could not connect to drone server. Make sure the mod is installed and the game is running, or change the connection url below.';
      this.loading = false;

    });
    this.loading = true;
    this.vrc.connect(this.defaultUrl);
  }

  connect(form) {
    this.loading = true;
    try {
      this.vrc.connect(form.value.url || this.defaultUrl);
      localStorage.setItem('url', form.value.url);
    } catch (err) {
      this.loading = false;
      this.errorMessage = err.errorMessage;
      console.error(err);
    }
  }

}
