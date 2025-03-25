import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { TokenService } from './services/token.service';
import { Token } from './models/token.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [TokenService]
})
export class AppComponent {
  tokens: Token[] = [];

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
}
