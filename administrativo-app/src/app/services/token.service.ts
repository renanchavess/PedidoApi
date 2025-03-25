import { HttpClient } from "@angular/common/http";
import { Token } from "../models/token.model";
import { Injectable } from "@angular/core";
import { environment } from "../../environments/environment";

@Injectable({
  providedIn: 'root'
})

export class TokenService {
    url = `${environment.api}/token`;

    constructor(private httpClient: HttpClient) { }
    
    public getTokens() {
        return this.httpClient.get<Token[]>(
            this.url,
            {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Basic ' + 'YWRtaW46cGFzc3dvcmQxMjM=' // localStorage.getItem('token')
                }
            }
        );
    }

    public criarToken(descricao: string, expiracao: string) {
        return this.httpClient.post<Token>(
            this.url,
            {
                descricao: descricao,
                expiracao: expiracao
            },
            {
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': 'Basic ' +  'YWRtaW46cGFzc3dvcmQxMjM=' // localStorage.getItem('token')
                }
            }
        );
    }

    public revogarToken(id: number) {
        return this.httpClient.delete(
            `${this.url}/${id}`,
            {
                headers: {
                    'Content-Type': 'application/json',
                }
            }
        );
    }
    
}