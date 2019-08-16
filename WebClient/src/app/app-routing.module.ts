import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes, CanActivate } from '@angular/router';
import { ConnectionActivate } from './misc/ConnectionActivate';
import { GamepadActivate } from './misc/GamepadActivate';

const routes: Routes = [
  { path: '', redirectTo: 'connect', pathMatch: 'full' },
  { path: 'connect', loadChildren: './pages/connect/connect.module#ConnectPageModule' },
  { path: 'controller', loadChildren: './pages/controller/controller.module#ControllerPageModule', canActivate: [ConnectionActivate] },
  { path: 'controller-config', loadChildren: './pages/controller-config/controller-config.module#ControllerConfigPageModule', canActivate: [ConnectionActivate, GamepadActivate] },
  { path: 'fly', loadChildren: './pages/fly/fly.module#FlyPageModule', canActivate: [ConnectionActivate] },
];

@NgModule({
  imports: [
  RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
