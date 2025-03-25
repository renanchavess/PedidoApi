import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TokenService } from './services/token.service';
import { Token } from './models/token.model';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: [TokenService]
})
export class AppComponent {
  tokens: Token[] = [];

  constructor(private TokenService: TokenService) { }

  ngOnInit() {
    this.obterTokens();
    console.log("Tokens: ", this.tokens);
  }

  obterTokens() {
    this.TokenService.getTokens().subscribe((tokens) => {
      this.tokens = tokens;
    });
  }
}
