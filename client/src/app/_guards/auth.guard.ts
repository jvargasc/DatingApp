import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs';
import { AccountService } from '../_services/account.service';

export const AuthGuard = () => {
    const accountService = inject(AccountService);
    const toastr = inject(ToastrService);

    return accountService.currentUser$.pipe(
        map(user => {
            if (user) return true;
            else {
                toastr.error('You shall not pass !');
                return false;
            }
        })
    )
};
