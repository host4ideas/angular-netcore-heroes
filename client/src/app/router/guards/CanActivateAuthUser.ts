import { inject } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivateFn,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { UserService } from '../../services/user.service';
import { ROUTES } from '../routes';

export const authGuard: CanActivateFn = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot
):
  | boolean
  | UrlTree
  | Observable<boolean | UrlTree>
  | Promise<boolean | UrlTree> => {
  return new Promise((resolve) => {
    const userService = inject(UserService);
    const router = inject(Router);

    if (!userService.userSubject$.getValue())
      resolve(router.parseUrl(`/${ROUTES.login}`));

    resolve(true);
  });
};
