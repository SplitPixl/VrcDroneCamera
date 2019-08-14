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
      ws.addEventListener('open', (ev) => { this.events.dispatchEvent(new Event(ev.type, ev)); });
      ws.addEventListener('message', (ev) => { this.events.dispatchEvent(new Event(ev.type, ev)); });
      ws.addEventListener('close', (ev) => { this.events.dispatchEvent(new Event(ev.type, ev)); });
      ws.addEventListener('error', (ev) => { this.events.dispatchEvent(new Event(ev.type, ev)); });
    } catch (err) {
      console.error(err);
      throw err;
    }
  }

  public isConnected() {
    console.log(this.ws);
    if (this.ws) {
      return this.ws.readyState === 1;
    }

    return false;
  }

  public send(data) {
    if (this.ws) {
      this.ws.send(data);
    }
  }

}
