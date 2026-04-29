using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Naloga1_Dinamicna.Models
{
    public class EmsoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            // Modulo 11 izračun: uteži so fiksne po standardu za EMŠO.
            // Ostanek 1 pomeni neveljaven EMŠO, ostalo pa mora ustrezati zadnji (13.) cifri.



            if (value == null) return ValidationResult.Success;

            string emso = value.ToString();

            // 1. Osnovno preverjanje dolžine
            if (emso.Length != 13 || !emso.All(char.IsDigit))
            {
                return new ValidationResult("EMŠO mora vsebovati natanko 13 številk.");
            }

            // 2. Izračun kontrolne številke (Modulo 11)
            // Formule: 7*L1 + 6*L2 + 5*L3 + 4*L4 + 3*L5 + 2*L6 + 7*L7 + 6*L8 + 5*L9 + 4*L10 + 3*L11 + 2*L12
            int[] weights = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            int sum = 0;

            for (int i = 0; i < 12; i++)
            {
                sum += (emso[i] - '0') * weights[i];
            }

            int remainder = sum % 11;
            int controlDigit = 11 - remainder;

            if (controlDigit == 11) controlDigit = 0;

            // Če je ostanek 1, EMŠO ni veljaven (po standardu)
            if (remainder == 1)
                return new ValidationResult("Neveljaven EMŠO (kontrolna številka).");

            // Preverimo, če se zadnja številka ujema z izračunano
            int lastDigit = emso[12] - '0';
            if (lastDigit != controlDigit)
            {
                return new ValidationResult("EMŠO ni matematično pravilen.");
            }

            return ValidationResult.Success;
        }
    }
}