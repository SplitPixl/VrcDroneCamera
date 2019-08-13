import { Injectable } from '@angular/core';
import { Subject, Observer, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WebsocketService {

  constructor() { }

  private ws: WebSocket;
  public events = new EventTarget();

  public connect(url) {
    const ws = new WebSocket(url);
    ws.addEventListener('open', (ev) => { this.events.dispatchEvent(ev); });
    ws.addEventListener('message', (ev) => { this.events.dispatchEvent(ev); });
    ws.addEventListener('close', (ev) => { this.events.dispatchEvent(ev); });
    ws.addEventListener('error', (ev) => { this.events.dispatchEvent(ev); });
  }

  public send(data) {
    this.ws.send(data);
  }

}
