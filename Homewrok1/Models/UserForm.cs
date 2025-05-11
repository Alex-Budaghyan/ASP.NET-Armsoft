using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Homework1.Models
{
    public class UserForm
    {
        [Required]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters.")]
        public required string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public required string Email { get; set; }

        [Required]
        [CustomValidation(typeof(UserForm), nameof(ValidatePassword))]
        public required string Password { get; set; }

        [DataType(DataType.Date)]
        [CustomValidation(typeof(UserForm), nameof(ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number.")]
        public int Quantity { get; set; }

        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Price must be a decimal value.")]
        public decimal Price { get; set; }

        [Range(0, 49.99, ErrorMessage = "Amount must be less than 50.")]
        public decimal Amount { get; set; }

        public static ValidationResult ValidatePassword(string password, ValidationContext context)
        {
            var instance = (UserForm)context.ObjectInstance;

            if (string.IsNullOrWhiteSpace(password))
                return new ValidationResult("Password is required.");

            if (password.Length < 6)
                return new ValidationResult("Password must be at least 6 characters long.");

            if (!Regex.IsMatch(password, "[A-Z]"))
                return new ValidationResult("Password must have at least one uppercase letter.");

            if (!Regex.IsMatch(password, "[a-z]"))
                return new ValidationResult("Password must have at least one lowercase letter.");

            if (!Regex.IsMatch(password, "[0-9]"))
                return new ValidationResult("Password must have at least one digit.");

            if (!Regex.IsMatch(password, @"[\W_]"))
                return new ValidationResult("Password must have at least one symbol.");

            if (!Regex.IsMatch(password, "[\u0531-\u058F]"))
                return new ValidationResult("Password must have at least one Armenian letter.");

            if (!string.IsNullOrEmpty(instance.Username) &&
                password.ToLower().Contains(instance.Username.ToLower()))
                return new ValidationResult("Password must not contain the username.");

            return ValidationResult.Success!;
        }

        public static ValidationResult ValidateDateOfBirth(DateTime dob, ValidationContext context)
        {
            return dob < DateTime.Today
                ? ValidationResult.Success!
                : new ValidationResult("Date of birth must be in the past.");
        }
    }
}
