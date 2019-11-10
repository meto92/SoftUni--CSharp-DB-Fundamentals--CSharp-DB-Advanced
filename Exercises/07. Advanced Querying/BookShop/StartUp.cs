namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class StartUp
    {
        // 01. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if (!Enum.TryParse(command, true, out AgeRestriction ageRestriction))
            {
                return "Incorrect age restriction!";
            }

            string result = string.Join(
                Environment.NewLine,
                context.Books
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .Select(b => b.Title)
                    .OrderBy(title => title)
                    .ToList());

            return result;
        }

        // 02. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            string result = string.Join(
                Environment.NewLine,
                context.Books
                    .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                    .OrderBy(b => b.BookId)
                    .Select(b => b.Title)
                    .ToList());

            return result;
        }

        // 03. Books by Price 
        public static string GetBooksByPrice(BookShopContext context)
        {
            string result = string.Join(
                Environment.NewLine,
                context.Books
                    .Where(b => b.Price > 40)
                    .OrderByDescending(b => b.Price)
                    .Select(b => $"{b.Title} - ${b.Price:f2}")
                    .ToList());

            return result;
        }

        // 04. Not Released In
        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            string result = string.Join(
                Environment.NewLine,
                context.Books
                    .Where(b => b.ReleaseDate == null ||
                        ((DateTime)b.ReleaseDate).Year != year)
                    .OrderBy(b => b.BookId)
                    .Select(b => b.Title)
                    .ToList());

            return result;
        }

        // 05. Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = Regex.Split(input, @"\s+")
                .Select(c => c.ToUpper())
                .ToArray();

            string result = string.Join(
                Environment.NewLine,
                context.Books
                    .Where(b => b.BookCategories
                        .Select(bc => bc.Category.Name.ToUpper())
                        .Any(c => categories.Contains(c)))
                    .OrderBy(b => b.Title)
                    .Select(b => b.Title)
                    .ToList());

            return result;
        }

        // 06. Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            string result = string.Join(
                Environment.NewLine,
                context.Books
                    .Where(b => b.ReleaseDate < parsedDate)
                    .OrderByDescending(b => b.ReleaseDate)
                    .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                    .ToList());

            return result;
        }

        // 07. Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            string result = string.Join(
                Environment.NewLine,
                context.Authors
                    .Where(a => a.FirstName.EndsWith(input))
                    .Select(a => string.Concat(a.FirstName, " ", a.LastName))
                    .OrderBy(name => name)
                    .ToArray());

            return result;
        }

        // 08. Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            string inputToUpper = input.ToUpper();

            string result = string.Join(
                Environment.NewLine,
                context.Books
                    .Where(b => b.Title.ToUpper().Contains(inputToUpper))
                    .Select(b => b.Title)
                    .OrderBy(title => title)
                    .ToArray());

            return result;
        }

        // 09. Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            string inputToUpper = input.ToUpper();

            string result = string.Join(
                Environment.NewLine,
                context.Books
                    .Where(b => b.Author.LastName.ToUpper().StartsWith(inputToUpper))
                    .OrderBy(b => b.BookId)
                    .Select(b => $"{b.Title} ({string.Concat(b.Author.FirstName, " ", b.Author.LastName)})")
                    .ToArray());

            return result;
        }

        // 10. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int count = context.Books
                .Count(b => b.Title.Length > lengthCheck);

            return count;

        }

        // 11. Total Book Copies 
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            string result = string.Join(
                Environment.NewLine,
                context.Authors
                    .Select(a => new
                    {
                        Author = string.Concat(a.FirstName, " ", a.LastName),
                        TotalAuthorCopies = a.Books.Sum(b => b.Copies)
                    })
                    .OrderByDescending(a => a.TotalAuthorCopies)
                    .Select(a => $"{a.Author} - {a.TotalAuthorCopies}")
                    .ToArray());

            return result;
        }

        // 12. Profit by Category 
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            string result = string.Join(
                Environment.NewLine,
                context.Categories
                    .Select(c => new
                    {
                        CategoryName = c.Name,
                        Profit = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                    })
                    .OrderByDescending(cp => cp.Profit)
                    .ThenBy(cp => cp.CategoryName)
                    .Select(cp => $"{cp.CategoryName} ${cp.Profit:f2}")
                    .ToArray());

            return result;
        }

        // 13. Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var booksByCategory = context.Categories
                .Select(c => new
                {
                    CategoryName = c.Name,
                    Books = c.CategoryBooks
                        .Select(bc => new
                        {
                            bc.Book.Title,
                            ReleaseYear = ((DateTime) bc.Book.ReleaseDate).Year,
                            bc.Book.ReleaseDate
                        })
                        .OrderByDescending(b => b.ReleaseDate)
                        .Take(3)
                        .ToArray()
                })
                .OrderBy(c => c.CategoryName);

            StringBuilder result = new StringBuilder();

            foreach (var category in booksByCategory)
            {
                result.AppendLine($"--{category.CategoryName}");

                foreach (var book in category.Books)
                {
                    result.AppendLine($"{book.Title} ({book.ReleaseYear})");
                }
            }

            return result.ToString().TrimEnd();
        }

        // 14. Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            context.Books
                .Where(b => ((DateTime) b.ReleaseDate).Year < 2010)
                .ToList()
                .ForEach(b => b.Price += 5);

            context.SaveChanges();
        }

        // 15. Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            List<Book> booksToRemove = context.Books
                .Where(b => b.Copies < 4200)
                .ToList();

            context.Books.RemoveRange(booksToRemove);

            context.SaveChanges();

            return booksToRemove.Count;
        }

        public static void Main()
        {
            using (BookShopContext db = new BookShopContext())
            {
                
            }
        }
    }
}