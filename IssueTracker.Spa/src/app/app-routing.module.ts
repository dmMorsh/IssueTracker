import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { ContactsComponent } from './pages/contacts/contacts.component';
import { LoginComponent } from './pages/login/login.component';
import { WeatherComponent } from './pages/weather/weather.component';
import { TicketsComponent } from './pages/tickets/tickets.component';
import { PersonalChatComponent } from './pages/personal-chat/personal-chat.component';
import { ChatsComponent } from './pages/chats/chats.component';
import { SearchContactsComponent } from './pages/search-contacts/search-contacts.component';
import { FriendRequestsComponent } from './pages/friend-requests/friend-requests.component';
import { ImFollowingComponent } from './pages/im-following/im-following.component';
import { TicketsWathcingComponent } from './pages/tickets-wathcing/tickets-wathcing.component';
import { TicketsExecutingComponent } from './pages/tickets-executing/tickets-executing.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    title: 'Home',
  },{
    path: 'contacts',
    component: ContactsComponent,
    title: 'contacts'
  },{
    path: 'search-contacts',
    component: SearchContactsComponent,
    title: 'search-contacts'
  },{
    path: 'riend-requests',
    component: FriendRequestsComponent,
    title: 'riend-requests'
  },{
    path: 'im-following',
    component: ImFollowingComponent,
    title: 'im-following'
  },{
    path: 'login',
    component: LoginComponent,
    title: 'login'
  },{
    path: 'weather',
    component: WeatherComponent,
    title: 'Weather'
  },{
    path: 'chats',
    component: ChatsComponent,
    title: 'chats'
  },{
    path: 'tickets',
    component: TicketsComponent,
    title: 'tickets'
  },{
    path: 'tickets-watching',
    component: TicketsWathcingComponent,
    title: 'tickets-watching'
  },{
    path: 'tickets-executing',
    component: TicketsExecutingComponent,
    title: 'tickets-executing'
  },{
    path: 'personalChat/:chatId/:userId',
    component: PersonalChatComponent,
    title: 'chat'
  },{
    path: '**',
    component: HomeComponent,
    redirectTo: ''
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
