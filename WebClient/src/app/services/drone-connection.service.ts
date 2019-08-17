import { Injectable } from '@angular/core';
import { WebsocketService } from './websocket.service';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DroneConnectionService {
  private ws: WebSocket;
  public events = new EventTarget();
  private encoder = new TextEncoder();
  public messages: Subject<Uint8Array>;

  public connect(url) {
    try {
      this.ws = new WebSocket(url);
      this.ws.addEventListener('open', (ev) => { this.events.dispatchEvent(new Event(ev.type, ev)); });
      this.ws.addEventListener('message', (ev) => { this.events.dispatchEvent(new Event(ev.type, ev)); });
      this.ws.addEventListener('close', (ev) => { this.events.dispatchEvent(new Event(ev.type, ev)); });
      this.ws.addEventListener('error', (ev) => { this.events.dispatchEvent(new Event(ev.type, ev)); });
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

  public close() {
    this.ws.close();
  }

  public sendInput(mx, my, mz, rx, ry, rz) {
    const data = new Uint8Array([DataType.INPUT]
      .concat(this.axisToBytes(mx))
      .concat(this.axisToBytes(my))
      .concat(this.axisToBytes(mz))
      .concat(this.axisToBytes(rx))
      .concat(this.axisToBytes(ry))
      .concat(this.axisToBytes(rz))
    );
    this.send(data);
  }

  private toShort(num) {
    const int16 = new Int16Array(1);
    int16[0] = num;
    return int16[0];
  }

  private axisToBytes(input) {
    const asShort = this.toShort(input * 0x7FFF);
    // tslint:disable-next-line:no-bitwise
    return [asShort & 0xFF, asShort >> 8];
  }

  private asByteArray(val) {
    return Array.from(new Uint8Array(val));
  }

  public updateFloat(id, val) {
    console.log(id, val);
    const parsedVal = parseFloat(val);
    const data = new Uint8Array([id].concat(this.asByteArray(this.floatBytes(parsedVal))));
    this.send(data);
  }

  public setFlightMode(mode) {
    mode = parseInt(mode, 10);
    this.send(new Uint8Array([DataType.SETMODE, mode]));
  }

  private floatBytes(val) {
    return new Float32Array([val]).buffer;
  }

  public teleport(px, py, pz, rx, ry, rz) {

    const data = new Uint8Array([DataType.TELEPORT]
      .concat(this.asByteArray(this.floatBytes(px)))
      .concat(this.asByteArray(this.floatBytes(py)))
      .concat(this.asByteArray(this.floatBytes(pz)))
      .concat(this.asByteArray(this.floatBytes(rx)))
      .concat(this.asByteArray(this.floatBytes(ry)))
      .concat(this.asByteArray(this.floatBytes(rz)))
    );


    this.send(data);
  }

  public setFov(val) {
    const data = new Uint8Array([DataType.CAMERASETTING, CameraSetting.FOV]
      .concat(this.asByteArray(this.floatBytes(val)))
    );

    this.send(data);
  }

  public setProjection(val) {
    const data = new Uint8Array([DataType.CAMERASETTING, CameraSetting.PROJECTION, parseInt(val, 10)]);
    console.log(data)
    this.send(data);
  }

}

export enum DataType {
  SETMODE = 0,
  INPUT = 1,
  SETMOVESPEED = 2,
  SETROTSPEED = 3,
  SETMOVESMOOTH = 4,
  SETROTSMOOTH = 5,
  TELEPORT = 6,
  RESET = 7,
  CAMERASETTING = 8
}

export enum CameraSetting {
  FOV = 0,
  PROJECTION = 1
}
