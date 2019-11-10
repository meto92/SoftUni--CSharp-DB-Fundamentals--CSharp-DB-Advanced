namespace PhotoShare.Client.Core.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    internal class PasswordAttribute : ValidationAttribute
    {
        private const string SpecialSymbols = "!@#$%^&*()_+<>,.?";
        private int minLength;
        private int maxLength;

        public PasswordAttribute(int minLength, int maxLength)
        {
            this.minLength = minLength;
            this.maxLength = maxLength;
        }

        public bool ContainsLowercase { get; set; }

        public bool ContainsUppercase { get; set; }

        public bool ContainsDigit { get; set; }

        public bool ContainsSpecialSymbol { get; set; }

        public override bool IsValid(object value)
        {
            string password = value.ToString();

            bool hasCorrectLength = 
                password.Length >= this.minLength && 
                password.Length <= this.maxLength;

            this.ContainsLowercase = password.Any(c => char.IsLower(c));

            this.ContainsUppercase = password.Any(c => char.IsUpper(c));

            this.ContainsDigit = password.Any(c => char.IsDigit(c));

            this.ContainsSpecialSymbol = password.Any(c => SpecialSymbols.Contains(c));

            if (!hasCorrectLength ||
                !this.ContainsLowercase ||
                !this.ContainsUppercase ||
                !this.ContainsDigit ||
                !this.ContainsSpecialSymbol)
            {
                return false;
            }
            
            return true;
        }
    }
}