import { FormGroup } from '@angular/forms';

export class FormUtils {
  static markAllFieldsAsTouched(form: FormGroup) {
    Object.keys(form.controls).forEach(key => {
      form.get(key)?.markAsTouched();
    });
  }

  static focusFirstInvalidField() {
    setTimeout(() => {
      const invalid = document.querySelector('.ng-invalid');
      if (invalid) {
        (invalid as HTMLElement).focus();
      }
    }, 0);
  }
}
