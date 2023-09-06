import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/User';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;

  constructor(private toastr: ToastrService) { }

  createHubConnection(user: User) {
    console.log("createHubConnection()");
    console.log(user);
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection?.start().catch(error => console.log(error));

    this.hubConnection.on('UsersIsOnline', username => {
      this.toastr.info(username + ' has connected');
    });

    this.hubConnection.on('UsersIsOffline', username => {
      this.toastr.info(username + ' has disconnected');
    });

  }

  stopHubConnection() {
    this.hubConnection?.stop().catch(error => console.log(error));
  }

}
