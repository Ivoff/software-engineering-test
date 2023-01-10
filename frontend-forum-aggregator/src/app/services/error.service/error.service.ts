import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  constructor(
    private snackBar: MatSnackBar,
    private router: Router
  ) { }

  dealWith(error: any)
  {
    console.error(error);
    this.snackBar.open(error.error, "Dismiss");
    if (error.status == 405)
            this.router.navigate(['/posts']);
  }
}
