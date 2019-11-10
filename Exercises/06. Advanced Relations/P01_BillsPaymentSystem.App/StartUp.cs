using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace P01_BillsPaymentSystem
{
    public class StartUp
    {
        private const string UserNotFoundMessage = "User with id {0} not found!";

        #region Seed

        private static User[] GenerateUsers()
        {
            string[] firstNames =
            {
                "Jossef",
                "Terri",
                "Sidney",
                "Taylor",
                "Jeffrey",
                "Jo",
                "Doris",
                "John",
                "Diane",
                "Steven"
            };

            string[] lastNames =
            {
                "Goldberg",
                "Duffy",
                "Higa",
                "Maxwell",
                "Ford",
                "Brown",
                "Hartwig",
                "Campbell",
                "Glimp",
                "Selikoff"
            };

            string[] domains =
            {
                "mail.bg",
                "abv.bg",
                "gmail.com",
                "hotmail.com",
                "softuni.bg",
                "students.softuni.bg"
            };

            string[] passwords =
            {
                "123456",
                "password",
                "bestpassword",
                "pogjdfhgsdf",
                "pqoeyghvb34",
                "asld(@#TGsdg",
                ";laskj&%^gh34J",
                "askdjY!%hgasd25",
                "qwertyuiop",
                "!@#$%^&*()",
            };

            int count = 10;

            Random rnd = new Random();

            User[] users = new User[count];

            for (int i = 0; i < count; i++)
            {
                string firstName = firstNames[rnd.Next(firstNames.Length)];
                string lastName = lastNames[rnd.Next(lastNames.Length)];
                string domai = domains[rnd.Next(domains.Length)];
                string email = $"{firstName.ToLower()}.{lastName.ToLower()}@{domai}";
                string password = passwords[rnd.Next(passwords.Length)];

                users[i] = new User(firstName, lastName, email, password);
            }

            return users;
        }

        private static BankAccount[] GenerateBankAccounts()
        {
            BankAccount[] bankAccounts =
            {
                new BankAccount(2000, "Unicredit Bulbank", "UNCRBGSF"),
                new BankAccount(1000, "First Investment Bank", "FINVBGSF"),
                new BankAccount(3000, "Allianz Bank", "ALBGSF"),
                new BankAccount(5000, "Bulgarian National Bank", "BNBBGSF"),
                new BankAccount(15000, "Piraeus Bank Bulgaria AD", "PBBGSF")
            };

            return bankAccounts;
        }

        private static CreditCard[] GenerateCreditCards()
        {
            CreditCard[] creditCards =
            {
                new CreditCard(800, new DateTime(2020, 3, 1)),
                new CreditCard(1500, new DateTime(2023, 2, 1)),
                new CreditCard(1000, new DateTime(2021, 5, 1)),
                new CreditCard(1024, new DateTime(2021, 8, 1)),
                new CreditCard(500, new DateTime(2022, 12, 1)),
                new CreditCard(750, new DateTime(2020, 4, 1)),
                new CreditCard(1200, new DateTime(2019, 9, 1)),
                new CreditCard(2000, new DateTime(2024, 7, 1)),
                new CreditCard(1750, new DateTime(2018, 11, 1)),
                new CreditCard(2500, new DateTime(2025, 1, 1))
            };

            return creditCards;
        }

        private static PaymentMethod[] GeneratePayments(User[] users, BankAccount[] bankAccounts, CreditCard[] creditCards)
        {
            Random rnd = new Random();

            int count = 50,
                usersCount = users.Length,
                bankAccountsCount = bankAccounts.Length,
                creditCardsCount = creditCards.Length;

            PaymentMethod[] payments = new PaymentMethod[count];

            for (int i = 0; i < count; i++)
            {
                User user = users[rnd.Next(usersCount)];

                if (i % 2 == 0)
                {
                    BankAccount bankAccount = bankAccounts[rnd.Next(bankAccountsCount)];

                    payments[i] = new PaymentMethod(user, bankAccount);
                }
                else
                {
                    CreditCard creditCard = creditCards[rnd.Next(creditCardsCount)];

                    payments[i] = new PaymentMethod(user, creditCard);
                }
            }

            return payments.GroupBy(p => new { p.User, p.BankAccount, p.CreditCard })
                .Select(g => g.First())
                .ToArray();
        }

        private static void Seed(BillsPaymentSystemContext db)
        {
            User[] users = GenerateUsers();

            BankAccount[] bankAccounts = GenerateBankAccounts();

            CreditCard[] creditCards = GenerateCreditCards();

            PaymentMethod[] paymentMethods = GeneratePayments(users, bankAccounts, creditCards);

            db.Users.AddRange(users);
            db.BankAccounts.AddRange(bankAccounts);
            db.CreditCards.AddRange(creditCards);
            db.PaymentMethods.AddRange(paymentMethods);

            db.SaveChanges();
        }

        #endregion

        private static void PrintUserInfo(BillsPaymentSystemContext db, int userId)
        {
            User user = db.Users.Find(userId);

            if (user == null)
            {
                Console.WriteLine(UserNotFoundMessage, userId);

                return;
            }

            StringBuilder result = new StringBuilder();

            result.AppendLine($"User: {user.FirstName} {user.LastName}");

            IEnumerable<BankAccount> bankAccounts = user.PaymentMethods
                .Where(p => p.Type == PaymentMethodType.BankAccount)
                .Select(p => p.BankAccount);

            if (bankAccounts.Any())
            {
                result.AppendLine("Bank Accounts:");
            }

            foreach (BankAccount bankAccount in bankAccounts)
            {
                result.AppendLine($"-- ID: {bankAccount.BankAccountId}");
                result.AppendLine($"--- Balance: {bankAccount.Balance:f2}");
                result.AppendLine($"--- Bank: {bankAccount.BankName}");
                result.AppendLine($"--- SWIFT: {bankAccount.SwiftCode}");
            }

            IEnumerable<CreditCard> creditCards = user.PaymentMethods
                .Where(p => p.Type == PaymentMethodType.CreditCard)
                .Select(p => p.CreditCard);

            if (creditCards.Any())
            {
                result.AppendLine("Credit Cards:");
            }

            foreach (CreditCard creditCard in creditCards)
            {
                result.AppendLine($"-- ID: {creditCard.CreditCardId}");
                result.AppendLine($"--- Limit: {creditCard.Limit:f2}");
                result.AppendLine($"--- Money Owed: {creditCard.MoneyOwed:f2}");
                result.AppendLine($"--- Limit Left:: {creditCard.LimitLeft}");
                result.AppendLine($"--- Expiration Date: {creditCard.ExpirationDate.ToString("yyyy/MM", CultureInfo.InvariantCulture)}");
            }

            Console.Write(result);
        }

        private static void PayBills(BillsPaymentSystemContext db, IEnumerable<BankAccount> bankAccounts, ref decimal amount)
        {
            foreach (BankAccount bankAccount in bankAccounts)
            {
                decimal moneyToWithdraw = Math.Min(amount, bankAccount.Balance);

                if (moneyToWithdraw == 0)
                {
                    return;
                }

                bankAccount.WithDraw(moneyToWithdraw);
                amount -= moneyToWithdraw;
            }
        }

        private static void PayBills(BillsPaymentSystemContext db, IEnumerable<CreditCard> creditCards, ref decimal amount)
        {
            foreach (CreditCard creditCard in creditCards)
            {
                decimal moneyToWithdraw = Math.Min(amount, creditCard.LimitLeft);

                if (moneyToWithdraw == 0)
                {
                    break;
                }

                creditCard.WithDraw(moneyToWithdraw);
                amount -= moneyToWithdraw;
            }
        }

        private static void PayBills(BillsPaymentSystemContext db, int userId, decimal amount)
        {
            User user = db.Users.Find(userId);

            if (user == null)
            {
                Console.WriteLine(UserNotFoundMessage, userId);

                return;
            }

            IEnumerable<BankAccount> bankAccounts = user.PaymentMethods
                .Where(p => p.Type == PaymentMethodType.BankAccount)
                .Select(p => p.BankAccount)
                .Where(b => b.Balance > 0)
                .OrderBy(ba => ba.BankAccountId)
                .ToArray();

            IEnumerable<CreditCard> creditCards = user.PaymentMethods
                .Where(p => p.Type == PaymentMethodType.CreditCard)
                .Select(p => p.CreditCard)
                .Where(cc => cc.LimitLeft > 0)
                .OrderBy(cc => cc.CreditCardId)
                .ToArray();

            if (bankAccounts.Sum(ba => ba.Balance) + creditCards.Sum(cc => cc.LimitLeft) < amount)
            {
                Console.WriteLine("Insufficient funds!");

                return;
            }

            PayBills(db, bankAccounts, ref amount);
            PayBills(db, creditCards, ref amount);

            db.SaveChanges();
        }

        public static void Main()
        {
            using (BillsPaymentSystemContext db = new BillsPaymentSystemContext())
            {
                PrintUserInfo(db, 1);
            }
        }
    }
}