import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { ContactsComponent } from './pages/contacts/contacts.component';
import { HeaderComponent } from './components/header/header.component';
import { NavigationComponent } from './components/navigation/navigation.component';

import { MatIconModule } from '@angular/material/icon';
import { HttpClientModule, provideHttpClient, withFetch, HTTP_INTERCEPTORS } from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from './pages/login/login.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { WeatherComponent } from './pages/weather/weather.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { TokenInterceptor } from './interceptor/token-interceptor.interceptor';
import { TicketsComponent } from './pages/tickets/tickets.component';
import { EditTicketsComponent } from './components/edit-tickets/edit-tickets.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { DatePipe } from '@angular/common';
import { PersonalChatComponent } from './pages/personal-chat/personal-chat.component';
import { AuthInterceptor } from './interceptor/auth-interceptor.interceptor';
import { ChatsComponent } from './pages/chats/chats.component';
import { SearchContactsComponent } from './pages/search-contacts/search-contacts.component';
import { FriendRequestsComponent } from './pages/friend-requests/friend-requests.component';
import { ImFollowingComponent } from './pages/im-following/im-following.component';
import { TicketsWathcingComponent } from './pages/tickets-wathcing/tickets-wathcing.component';
import { TicketsExecutingComponent } from './pages/tickets-executing/tickets-executing.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    ContactsComponent,
    HeaderComponent,
    NavigationComponent,
    LoginComponent,
    WeatherComponent,
    TicketsComponent, 
    EditTicketsComponent, 
    PersonalChatComponent, 
    ChatsComponent, 
    SearchContactsComponent, 
    FriendRequestsComponent, 
    ImFollowingComponent, 
    TicketsWathcingComponent, 
    TicketsExecutingComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatIconModule,
    NgbModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    NgSelectModule,
  ],
  providers: [
    provideClientHydration(),
    provideHttpClient(withFetch()),
    provideAnimationsAsync(),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptor,
      multi: true
    }, 
    { provide: HTTP_INTERCEPTORS, 
      useClass: AuthInterceptor, 
      multi: true },
    DatePipe
  ],
  bootstrap: [AppComponent]
})

export class AppModule { }
