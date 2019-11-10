using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class User
    {
        public User()
        {
            this.PaymentMethods = new List<PaymentMethod>();
        }

        public User(string firstName, string lastName, string email, string password)
            : this()
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.Password = password;
        }

        public int UserId { get; private set; }

        public string FirstName  { get; set; }

        public string LastName  { get; set; }

        public string Email  { get; set; }

        public string Password  { get; set; }

        public virtual ICollection<PaymentMethod> PaymentMethods { get; }
    }
}