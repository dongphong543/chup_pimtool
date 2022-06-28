import { Pipe, PipeTransform } from '@angular/core';
/*
 * Raise the value exponentially
 * Takes an exponent argument that defaults to 1.
 * Usage:
 *   value | exponentialStrength:exponent
 * Example:
 *   {{ 2 | exponentialStrength:10 }}
 *   formats to: 1024
*/
@Pipe({name: 'statusPipe'})
export class StatusPipe implements PipeTransform {
  transform(STA: string): string {

    if (STA == "NEW") return "New";
    if (STA == "PLA") return "Planned";
    if (STA == "INP") return "In progress";
    if (STA == "FIN") return "Finished";
    return "<Unknown>";
  }
}