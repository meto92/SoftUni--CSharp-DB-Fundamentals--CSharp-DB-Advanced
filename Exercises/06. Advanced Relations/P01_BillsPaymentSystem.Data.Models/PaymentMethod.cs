namespace P01_BillsPaymentSystem.Data.Models
{
    public class PaymentMethod
    {
        private PaymentMethod(PaymentMethodType type)
        {
            this.Type = type;
        }
        
        public PaymentMethod()
        { }

        public PaymentMethod(int userId, int? bankAccountId, int? creditCardId)
            : this(bankAccountId != null ? PaymentMethodType.BankAccount : PaymentMethodType.CreditCard)
        {
            this.UserId = userId;
            this.BankAccountId = bankAccountId;
            this.CreditCardId = creditCardId;
        }

        public PaymentMethod(User user, BankAccount bankAccount)
            : this(PaymentMethodType.BankAccount)
        {
            this.User = user;
            this.BankAccount = bankAccount;
        }

        public PaymentMethod(User user, CreditCard creditCard)
            : this(PaymentMethodType.CreditCard)
        {
            this.User = user;
            this.CreditCard = creditCard;
        }

        public int Id { get; private set; }

        public PaymentMethodType Type { get; set; }

        public int UserId { get; set; }

        public int? BankAccountId { get; set; }

        public int? CreditCardId { get; set; }

        public virtual User User { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        public virtual CreditCard CreditCard { get; set; }
    }
}