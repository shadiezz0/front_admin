import { Component, OnInit, inject, PLATFORM_ID } from "@angular/core";
import { CommonModule, isPlatformBrowser } from "@angular/common";
import { ActivatedRoute, Router } from "@angular/router";
import { AuthService } from "../../core/services";

@Component({
  selector: "app-landing",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./landing.component.html",
  styleUrl: "./landing.component.css"
})
export class LandingComponent implements OnInit {
  private platformId = inject(PLATFORM_ID);

  user: any = null;
  userInitial = "?";
  displayName = "";
  isProcessingToken = false;
  isLoggedIn = false;
  showNoTokenWarning = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    // Read token from URL fragment (#token=eyJ...)
    // Fragment is never sent to IIS server so no HTTP 404.15 limit.
    const fragment = window.location.hash;
    const fragmentToken = this.extractTokenFromFragment(fragment);

    if (fragmentToken) {
      this.isProcessingToken = true;

      // Immediately clean the hash from the URL
      window.history.replaceState(null, "", window.location.pathname);

      this.authService.loginWithToken(fragmentToken).subscribe({
        next: (success) => {
          if (success) {
            this.router.navigate(["/dashboard"], { replaceUrl: true });
          } else {
            this.isProcessingToken = false;
            this.showNoTokenWarning = true;
          }
        },
        error: () => {
          this.isProcessingToken = false;
          this.showNoTokenWarning = true;
        }
      });
      return;
    }

    // Step 2: Already have a valid session (after page refresh or regular navigation)
    if (this.authService.isLoggedIn()) {
      this.isLoggedIn = true;
      this.user = this.authService.currentUser();
      this.displayName = this.authService.getDisplayName();
      this.userInitial = this.displayName.charAt(0).toUpperCase() || "?";
      return;
    }

    // Step 3: No token anywhere - not authenticated
    this.isLoggedIn = false;
    this.showNoTokenWarning = true;
  }

  private extractTokenFromFragment(fragment: string): string | null {
    if (!fragment || !fragment.startsWith("#")) return null;
    // Remove the leading '#' and parse as URL parameters
    const params = new URLSearchParams(fragment.substring(1));
    return params.get("token");
  }

  goToDashboard(): void {
    if (this.authService.isLoggedIn()) {
      this.router.navigate(["/dashboard"]);
    }
  }
}
