using System;
using System.Collections.Generic;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class BankAccount
    {
        private const string NegativeOrZeroAmountMessage = "Amount should be greater than 0!";
        private const string InsufficientFundsMessage = "Insufficient funds!";

        public BankAccount()
        {
            this.Payments = new List<PaymentMethod>();
        }

        public BankAccount(decimal balance, string bankName, string swiftCode)
            : this()
        {
            this.Balance = balance;
            this.BankName = bankName;
            this.SwiftCode = swiftCode;
        }

        public int BankAccountId { get; private set; }

        public decimal Balance{ get; private set; }

        public string BankName { get; set; }

        public string SwiftCode { get; set; }          

        public virtual ICollection<PaymentMethod> Payments { get; }

        public void WithDraw(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException(NegativeOrZeroAmountMessage);
            }

            if (amount > this.Balance)
            {
                throw new InvalidOperationException(InsufficientFundsMessage);
            }

            this.Balance -= amount;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                throw new InvalidOperationException(NegativeOrZeroAmountMessage);
            }

            this.Balance += amount;
        }
    }
}