import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { IonicModule } from '@ionic/angular';

import { FlyPage } from './fly.page';

import { StatusComponent } from './../../components/status/status.component';
import { FlyVarsComponent } from './../../components/fly-vars/fly-vars.component';
import { TeleportComponent } from 'src/app/components/teleport/teleport.component';
import { CameraVarsComponent } from 'src/app/components/camera-vars/camera-vars.component';

const routes: Routes = [
  {
    path: '',
    component: FlyPage
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    IonicModule,
    RouterModule.forChild(routes)
  ],
  declarations: [
    FlyPage,
    StatusComponent,
    FlyVarsComponent,
    TeleportComponent,
    CameraVarsComponent
  ]
})
export class FlyPageModule {}
