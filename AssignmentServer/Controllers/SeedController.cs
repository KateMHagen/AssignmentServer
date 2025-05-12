using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model;
using AssignmentServer.Data;

namespace AssignmentServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController(BooksPublishersContext context, IHostEnvironment environment,
        UserManager<BooksPublishersUser> userManager) : ControllerBase
    {
        string _pathName = Path.Combine(environment.ContentRootPath, "Data/books.csv");

        [HttpPost("Users")]
        public async Task<IActionResult> ImportUsersAsync()
        {
            var existingUser = await userManager.FindByNameAsync("user");
            if (existingUser != null)
                return Ok("User already exists");

            BooksPublishersUser user = new()
            {
                UserName = "user",
                Email = "user@email.com",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await userManager.CreateAsync(user, "Password123!");

            if (!result.Succeeded)
            {
                // Return reasons to Swagger response
                return BadRequest(result.Errors.Select(e => e.Description));
            }

            return Ok("User created");
        }


        [HttpPost("Publishers")]
        public async Task<ActionResult> ImportPublishersAsync()
        {
            Dictionary<string, Publisher> publisherByName = context.Publishers
                .AsNoTracking().ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            List<BooksDto> records = csv.GetRecords<BooksDto>().ToList();
            foreach (BooksDto record in records)
            {
                if (publisherByName.ContainsKey(record.publisher))
                    continue;

                Publisher publisher = new()
                {
                    Name = record.publisher,
                };
                await context.Publishers.AddAsync(publisher);
                publisherByName.Add(record.publisher, publisher);
            }

            await context.SaveChangesAsync();
            return new JsonResult(publisherByName.Count);
        }

        [HttpPost("Books")]
        public async Task<ActionResult> ImportBooksAsync()
        {
            Dictionary<string, Publisher> publishers = await context.Publishers.ToDictionaryAsync(c => c.Name);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            int bookCount = 0;

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            IEnumerable<BooksDto> records = csv.GetRecords<BooksDto>();
            foreach (BooksDto record in records)
            {
                if (!publishers.TryGetValue(record.publisher, out Publisher? publisher))
                {
                    Console.WriteLine($"Not found publisher for {record.title}");
                    continue;
                }
                Console.WriteLine($"Importing book: {record.title}, Description: {record.description}");
                Console.WriteLine($"Description length: {record.description?.Length} | Title: {record.title}");

                if (string.IsNullOrWhiteSpace(record.pages) || !int.TryParse(record.pages, out int parsedPages))
                {
                    Console.WriteLine($"Skipping book '{record.title}' with invalid pages value '{record.pages}'");
                    continue;
                }

                // CSV has European decimal style numbers.
                decimal? parsedRating = null;
                if (!string.IsNullOrWhiteSpace(record.rating))
                {
                    var cleaned = record.rating.Replace(",", ".");
                    if (decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var r))
                    {
                        parsedRating = r;
                    }
                    else
                    {
                        Console.WriteLine($"Could not parse rating '{record.rating}' for '{record.title}'");
                    }
                }


                var existing = await context.Books.FirstOrDefaultAsync(b => b.Title == record.title);
                if (existing != null)
                {
                    existing.Description = record.description;
                    existing.Author = record.author.Length > 555 ? record.author.Substring(0, 555) : record.author;
                }
                else
                {
                    Book book = new()
                    {
                        Title = record.title,
                        Author = record.author,
                        Pages = parsedPages,
                        Rating = parsedRating,
                        Description = record.description,
                        PublisherId = publisher.Id
                    };
                    context.Books.Add(book);
                    bookCount++;
                }

            }

            await context.SaveChangesAsync();
            Console.WriteLine($"Books imported: {bookCount}");
            return new JsonResult(bookCount);
        }
    }
}