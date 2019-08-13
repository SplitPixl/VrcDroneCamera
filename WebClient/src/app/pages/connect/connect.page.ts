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

  public defaultUrl = 'ws://localhost:35000/control';
  public errorMessage: string;
  public loading = false;

  constructor(private vrc: VrcConnectionService, private ws: WebsocketService, private router: Router) { }

  ngOnInit() {
    this.ws.events.addEventListener('open', () => { this.router.navigate(['/control']); });
  }

  connect(form) {
    this.loading = true;
    try {
      this.vrc.connect(form.value.url || this.defaultUrl);
    } catch (err) {
      this.loading = false;
      this.errorMessage = err.errorMessage;
      console.error(err);
    }
  }

}
