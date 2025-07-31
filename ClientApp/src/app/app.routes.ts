import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './shared/components/errors/not-found/not-found.component';
import { AccountModule } from './account/account.module';
import { PlayComponent } from './play/play.component';

export const routes: Routes = [
    {path:'',component: HomeComponent,title: 'Home Page',},
    {path:'play',component: PlayComponent},
    //implement lazyloading by the following format
    {path:'account',loadChildren: () =>import('./account/account.module').then(module => module.AccountModule)},
    {path:'not-found',component:NotFoundComponent},
    {path:'**',component:NotFoundComponent,pathMatch:'full',}
];
