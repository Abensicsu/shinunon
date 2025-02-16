using DataModels.Data;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static ShWeb.Components.Pages.ReviewExamComponent;

namespace ShWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookController : Controller
    {
        public SHcx Cx { get; }
        public BookController(SHcx cx)
        {
            Cx = cx;
        }

        [HttpGet]
        public async Task<List<Book>> AllBooksIncludeSubjects()
        {
            return await Cx.Books.Include(b => b.Subjects).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<List<Book>>> AdvancedSearch([FromBody] AdvancedSearchCriteria criteria)
        {
            var query = Cx.Books.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.BookName))
            {
                query = query.Where(b => b.HebrewBookName.Contains(criteria.BookName));
            }

            //if (!string.IsNullOrEmpty(criteria.AuthorName))
            //{
            //    query = query.Where(b => b.AuthorName.Contains(criteria.AuthorName));
            //}

            if (criteria.Ordinal >= 1)
            {
                query = query.Where(b => b.Subjects.Any(s => s.Ordinal == criteria.Ordinal));
            }

            //if (!string.IsNullOrEmpty(criteria.Description))
            //{
            //    query = query.Where(b => b.Description.Contains(criteria.Description));
            //}

            var result = await query.Include(b => b.Subjects).ToListAsync();
            return Ok(result);
        }

    }
}
