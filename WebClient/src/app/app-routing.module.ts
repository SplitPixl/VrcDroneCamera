import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: 'connect', pathMatch: 'full' },
  { path: 'connect', loadChildren: './pages/connect/connect.module#ConnectPageModule' },
  { path: 'controller', loadChildren: './pages/controller/controller.module#ControllerPageModule' },
  { path: 'controller-config', loadChildren: './pages/controller-config/controller-config.module#ControllerConfigPageModule' },
  { path: 'fly', loadChildren: './pages/fly/fly.module#FlyPageModule' },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
