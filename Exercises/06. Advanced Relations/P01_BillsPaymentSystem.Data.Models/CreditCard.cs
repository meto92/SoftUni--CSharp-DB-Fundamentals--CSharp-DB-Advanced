using System;
using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class CreditCard
    {
        private const string NegativeOrZeroAmountMessage = "Amount should be greater than 0!";
        private const string InsufficientFundsMessage = "Insufficient funds!";

        public CreditCard()
        {
            this.Payments = new List<PaymentMethod>();
        }

        public CreditCard(decimal limit, DateTime expirationDate)
            : this()
        {
            this.Limit = limit;
            this.MoneyOwed = 0;
            this.ExpirationDate = expirationDate;
        }

        public int CreditCardId { get; private set; }

        public decimal Limit { get; private set; }

        public decimal MoneyOwed { get; private set; }

        public decimal LimitLeft => this.Limit - this.MoneyOwed;

        public DateTime ExpirationDate { get; set; }

        public virtual ICollection<PaymentMethod> Payments { get; }

        public void WithDraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException(NegativeOrZeroAmountMessage);
            }

            if (this.MoneyOwed + amount > this.Limit)
            {
                throw new InvalidOperationException(InsufficientFundsMessage);
            }

            this.MoneyOwed += amount;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException(NegativeOrZeroAmountMessage);
            }
            
            this.Limit += Math.Max(0, amount - this.MoneyOwed);
            this.MoneyOwed = Math.Max(0, this.MoneyOwed - amount);
        }
    }
}