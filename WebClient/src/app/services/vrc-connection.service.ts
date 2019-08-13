import { Injectable } from '@angular/core';
import { WebsocketService } from './websocket.service';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VrcConnectionService {
  private wsService;
  private encoder = new TextEncoder();
  public messages: Subject<Uint8Array>;

  constructor(wsService: WebsocketService) {
    this.wsService = wsService;
  }

  public connect(url: string) {
    try {
      this.wsService.connect(url);
    } catch (err) {
      throw err;
    }
  }
}
