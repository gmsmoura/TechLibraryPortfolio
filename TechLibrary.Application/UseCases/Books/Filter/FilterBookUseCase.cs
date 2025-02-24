using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Domain.Entities;
using TechLibrary.Infrastructure.DataAccess;
using TechLibrary.Infrastructure.Security.Tokens.Access;

namespace TechLibrary.Application.UseCases.Books.Filter
{
    public class FilterBookUseCase
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        private const int PAGE_SIZE = 10;
        public FilterBookUseCase(JwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public ResponseBooksJson Execute(RequestFilterBooksJson requestFilterBooks)
        {
            var dbContext = new TechLibraryDbContext();

            var skip = (requestFilterBooks.PageNumber - 1) * PAGE_SIZE;

            var query = dbContext.Books.AsQueryable();

            if (string.IsNullOrWhiteSpace(requestFilterBooks.Title) == false)
            {
                query = dbContext.Books.Where(x => x.Title.Contains(requestFilterBooks.Title));
            }

           var books = query
                .OrderBy(x => x.Title).ThenBy(x => x.Author)
                .Skip(skip)
                .Take(PAGE_SIZE)
                .ToList();

            var totalCount = 0;

            if (string.IsNullOrWhiteSpace(requestFilterBooks.Title)) 
                totalCount = dbContext.Books.Count();
            else
                totalCount = dbContext.Books.Count(x => x.Title.Contains(requestFilterBooks.Title));

            return new ResponseBooksJson
            {
                Pagination = new ResponsePaginationJson
                {
                    PageNumber = requestFilterBooks.PageNumber,
                    TotalCount = totalCount
                },
                Books = books.Select(book => new ResponseBookJson
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                }).ToList()
            };
        }
    }
}
