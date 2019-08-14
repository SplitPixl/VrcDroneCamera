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
    try {
      const ws = new WebSocket(url);
      ws.addEventListener('open', (ev) => { this.events.dispatchEvent(new event.constructor(ev.type, ev)); });
      ws.addEventListener('message', (ev) => { this.events.dispatchEvent(new event.constructor(ev.type, ev)); });
      ws.addEventListener('close', (ev) => { this.events.dispatchEvent(new event.constructor(ev.type, ev)); });
      ws.addEventListener('error', (ev) => { this.events.dispatchEvent(new event.constructor(ev.type, ev)); });
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  public send(data) {
    this.ws.send(data);
  }

}
