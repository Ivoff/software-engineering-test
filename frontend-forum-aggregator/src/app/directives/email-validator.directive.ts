import { Directive } from '@angular/core';
import { AbstractControl, Validator, NG_VALIDATORS } from '@angular/forms';
import * as yup from 'yup';

@Directive({
  selector: '[appEmailValidator]',
  providers: [{
    provide: NG_VALIDATORS,
    useExisting: EmailValidatorDirective,
    multi: true
  }]
})
export class EmailValidatorDirective implements Validator 
{
  validator = yup.string().required().email();

  validate(control: AbstractControl) : {[key: string]: any} | null {
    return this.validator.validate(control.value)
      .then(() => {
        true;
      })
      .catch((err) => {
        return false;
      })
  } 
}
