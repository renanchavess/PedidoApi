import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TokenService } from './services/token.service';
import { Token } from './models/token.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, FormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [TokenService]
})
export class AppComponent {
  tokens: Token[] = [];
  descricao: string = ''; // Campo para a descrição do token
  expiracao: string = ''; // Campo para a data de expiração do token

  constructor(private tokenService: TokenService) { }

  ngOnInit() {
    this.obterTokens();
  }

  obterTokens() {
    this.tokenService.getTokens().subscribe((tokens) => {
      this.tokens = tokens;
      console.log("Tokens: ", this.tokens);
    });
  }

  gerarToken() {
    if (!this.descricao || !this.expiracao) {
      console.error('Descrição e expiração são obrigatórios.');
      return;
    }

    this.tokenService.criarToken(this.descricao, this.expiracao).subscribe((novoToken) => {
      console.log('Token gerado com sucesso:', novoToken);
      this.obterTokens(); 
      this.descricao = ''; 
      this.expiracao = '';
    }, (error) => {
      console.error('Erro ao gerar o token:', error);
    });
  }

  revogarToken(tokenId: number) {
    this.tokenService.revogarToken(tokenId).subscribe(() => {
      this.obterTokens();
    }, (error) => {
      console.error(`Erro ao revogar o token ${tokenId}:`, error);
    });
  }
}
