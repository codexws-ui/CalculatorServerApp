using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;

namespace ServerApp.Models
{
    public class Calculator
    {
        [Display(Name = "Value A")]
        [Required(ErrorMessage = "Required")]
        public required string ValueA { get; set; }

        [Display(Name = "Value B")]
        [Required(ErrorMessage = "Required")]
        public required string ValueB { get; set; }

        [Display(Name = "Operation")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Required")]
        public required OperationType Operation { get; set; }

        [Display(Name = "Result")]
        public string? Result { get; set; }
    }

    public enum OperationType
    {
        [Display(Name = "Addition (A+B)")]
        Addition = 1,
        [Display(Name = "Subtraction (A-B)")]
        Subtraction,
        [Display(Name = "Multiplication (A*B)")]
        Multiplication,
        [Display(Name = "Division (A/B)")]
        Division,
        [Display(Name = "Modulus (A mod B)")]
        Modulus,
        [Display(Name = "Percent (a from B)")]
        Percent,
        [Display(Name = "Exponentiation (A^b)")]
        Exponentiation,
        [Display(Name = "N-Roots (b√A)")]
        NRoot,
        [Display(Name = "Random (from A to B)")]
        Random,
        [Display(Name = "Add Days (date A days B)")]
        AddDays,
        [Display(Name = "Concatenation (A & B)")]
        Concatenation
    }
}
