import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";

export class AuthService {
    url = `${environment.api}/auth`;

    constructor(private httpClient: HttpClient) { }
    
    public login(email: string, senha: string) {
        return this.httpClient.post<string>(                
            this.url,
            {
                email: email,
                senha: senha
            },
            {
                headers: {
                    'Content-Type': 'application/json'
                }
            }
        ).subscribe((token) => {
            // localStorage.setItem('token', token);
        });
    }
    
}