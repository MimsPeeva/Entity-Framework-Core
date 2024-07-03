using System.Globalization;
using System.Linq;
using System.Text;
using BookShop.Models.Enums;

namespace BookShop
{
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            // DbInitializer.ResetDatabase(db);
            // Console.WriteLine(GetBooksByAgeRestriction(db,"miNor"));
            // Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksByPrice(db));
            //Console.WriteLine(GetBooksNotReleasedIn(db,2000));
            //Console.WriteLine(GetBooksByCategory(db, "horror mystery drama"));
            //Console.WriteLine(GetBooksReleasedBefore(db, "12-04-1992"));
            //Console.WriteLine(GetAuthorNamesEndingIn(db, "e"));
            //Console.WriteLine(GetBookTitlesContaining(db, "sK"));
            //Console.WriteLine(GetBooksByAuthor(db, "po"));
            //Console.WriteLine(CountBooks(db, 12));
            //Console.WriteLine(CountCopiesByAuthor(db));
            //Console.WriteLine(GetTotalProfitByCategory(db));
            //Console.WriteLine(GetMostRecentBooks(db));
            //IncreasePrices(db);
            //RemoveBooks(db);
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
         {
             if (!Enum.TryParse(command,true,out AgeRestriction ageRestriction))
             {
                return String.Empty;
             }

             var booksTitles =  context.Books
                 .Where(x => x.AgeRestriction == ageRestriction)
                 .Select(b => b.Title)
                 .OrderBy(b=>b).ToArray();
             return string.Join(Environment.NewLine,booksTitles);
         }

         public static string GetGoldenBooks(BookShopContext context)
         {
             var booksTitles = context.Books
                 .Where(b=>b.Copies<5000 && b.EditionType == EditionType.Gold)
                 .Select(b => new
                 {
                     b.Title,
                     b.BookId
                 })
                 .OrderBy(b => b.BookId)
                 .ToArray();
             return string.Join(Environment.NewLine, booksTitles.Select(b=>b.Title));

        }

         public static string GetBooksByPrice(BookShopContext context)
         {
             var bookTitles = context.Books
                 .Where(b=>b.Price>40)
                 .Select(b => new
                     {
                         Title = b.Title,
                         Price = b.Price
                     }
                 )
                 .OrderByDescending(b => b.Price).ToArray();

             StringBuilder sb = new StringBuilder();
             foreach (var b in bookTitles)
             {
                 sb.AppendLine($"{b.Title} - ${b.Price:f2}");
             }
             return sb.ToString().ToString().TrimEnd();
            //second option
            // return string.Join(Environment.NewLine,bookTitles.Select(b=> $"{b.Title} - ${b.Price:f2}"));
         }

         public static string GetBooksNotReleasedIn(BookShopContext context, int year)
         {
             var booksTitles = context.Books
                 .Where(b=>b.ReleaseDate.HasValue && b.ReleaseDate.Value.Year !=year)
                 .Select(b => new
                 {
                     b.Title,
                     b.BookId
                 })
                 .OrderBy(b => b.BookId).ToArray();

             return string.Join(Environment.NewLine, booksTitles.Select(b => b.Title));
         }

         public static string GetBooksByCategory(BookShopContext context, string input)
         {
            string[] categories = input.ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var booksTitles = context.BooksCategories
                .Where(b=>categories.Contains(b.Category.Name.ToLower()))
                .Select(b => b.Book.Title)
                .OrderBy(b => b)
                .ToArray();

            return string.Join(Environment.NewLine, booksTitles);

            
        }

         public static string GetBooksReleasedBefore(BookShopContext context, string date)
         {
             DateTime dt = DateTime.ParseExact(date, "dd-MM-yyyy",
                 CultureInfo.InvariantCulture);
             var booksTitles = context.Books
                 .Where(b=>b.ReleaseDate<dt)
                 .OrderByDescending(b=>b.ReleaseDate)
                 .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:f2}")
                 .ToArray();
             return string.Join(Environment.NewLine, booksTitles);
        }

         public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
         {
             var authorNames = context.Authors
                 .Where(a=>a.FirstName.EndsWith(input))
                 .Select(a =>  $"{a.FirstName} {a.LastName}").ToArray()
                 .OrderBy(a => a);
             return string.Join(Environment.NewLine, authorNames);
         }

         public static string GetBookTitlesContaining(BookShopContext context, string input)
         {
             var booksTitles = context.Books
                 .Where(b=>b.Title.ToLower().Contains(input.ToLower()))
                 .Select(b => b.Title)
                 .OrderBy(b => b).ToArray();

             return string.Join(Environment.NewLine, booksTitles);
         }

         public static string GetBooksByAuthor(BookShopContext context, string input)
         {
             var booksTitles = context.Books
                 .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                 .Select(b => new
                 {
                     b.Title,
                    authorName = $"{b.Author.FirstName} {b.Author.LastName}",
                    b.BookId
                 })
                 .OrderBy(b => b.BookId).ToArray();

             return string.Join(Environment.NewLine, 
                 (booksTitles.Select(b=>$"{b.Title} ({b.authorName})")));
        }

         public static int CountBooks(BookShopContext context, int lengthCheck)
         {
             

             return context.Books.Count(b=>b.Title.Length>lengthCheck);
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(a => new
                {
                    a.FirstName,
                    a.LastName,
                    Copies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(b => b.Copies).ToArray();


            return string.Join(Environment.NewLine,authors.Select(a=>$"{a.FirstName} {a.LastName} - {a.Copies}"));
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Categories
                .Select(c=>new
                {
                   Category = c.Name,
                    Profit = c.CategoryBooks.Sum(c=>c.Book.Copies*c.Book.Price)
                })
                .OrderByDescending(b => b.Profit)
                .ThenBy(b=>b.Category).ToArray();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Category} ${b.Profit:f2}"));
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories
                .Select(c => new
                {
                    c.Name,
                    MostRecentBooks = c.CategoryBooks
                        .OrderByDescending(b => b.Book.ReleaseDate)
                        .Take(3)
                        .Select(b => $"{b.Book.Title} ({b.Book.ReleaseDate.Value.Year})")

                }).ToArray()
                .OrderBy(c => c.Name).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var b in books)
            {
                sb.AppendLine($"--{b.Name}");
                foreach (var mrb in b.MostRecentBooks)
                {
                    sb.AppendLine(mrb);
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate.Value.Year < 2010).ToArray();

            foreach (var b in books)
            {
                b.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksToDelete = context.Books
                .Where(b => b.Copies < 4200).ToArray();

            context.RemoveRange(booksToDelete);
            context.SaveChanges();
            return booksToDelete.Length;
        }
    }

}


