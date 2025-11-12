import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { UserLoginDto, UserResponseDto } from '../../shared/models/user';

// Service to handle authentication and user session management
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'http://localhost:7000/api/auth';
  private currentUserSubject = new BehaviorSubject<UserResponseDto | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadUserFromStorage();
  }

  private loadUserFromStorage(): void {
    const user = localStorage.getItem('currentUser');
    const token = localStorage.getItem('token');

    if (user && token) {
      this.currentUserSubject.next(JSON.parse(user));
    }
  }

  login(credentials: UserLoginDto): Observable<UserResponseDto> {
    return this.http.post<UserResponseDto>(`${this.baseUrl}/login`, credentials)
      .pipe(
        tap(user => {
          if (user && user.token) {
            localStorage.setItem('currentUser', JSON.stringify(user));
            localStorage.setItem('token', user.token);
            this.currentUserSubject.next(user);
          }
        })
      );
  }

  logout(): void {
    localStorage.removeItem('currentUser');
    localStorage.removeItem('token');
    this.currentUserSubject.next(null);
  }

  getCurrentUser(): UserResponseDto | null {
    return this.currentUserSubject.value;
  }

  getToken(): string | null {
    const token = localStorage.getItem('token');
    return token;
  }

  isLoggedIn(): boolean {
    const isLoggedIn = !!this.getToken();
    return isLoggedIn;
  }

  hasRole(role: string): boolean {
    const user = this.getCurrentUser();
    const hasRole = user?.roleName === role;
    return hasRole;
  }
}
