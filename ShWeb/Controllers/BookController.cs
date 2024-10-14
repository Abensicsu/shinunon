using DataModels.Data;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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

        //[HttpGet]
        //public List<Book> AllBooks()
        //{
        //    return Cx.Books.ToList();
        //}

        //[HttpGet]
        //public async Task<List<Book>> AllBooksIncludeSubjects()
        //{
        //    return await Cx.Books.Include(b => b.Subjects).ToListAsync();
        //}

        //[HttpGet]
        //public async Task<List<Book>> AllBooksIncludeSubjectsAndText()
        //{
        //    return await Cx.Books
        //        .Include(b => b.Subjects)
        //        .ThenInclude(s => s.SubjectText).ToListAsync();
        //}

        [HttpGet]
        public List<Book> AllBooks()
        {
            return Cx.Books.ToList(); 
        }

        [HttpGet("{bookId}")]
        public async Task<List<Subject>> GetBookSubjects(int bookId)
        {
            // Retrieve subjects for a specific book
            var book = await Cx.Books.Include(b => b.Subjects)
                                     .FirstOrDefaultAsync(b => b.BookId == bookId);
            return book?.Subjects.ToList() ?? new List<Subject>();
        }

        //[HttpGet]
        //public async Task<List<Book>> AllBooksIncludeSubjectsAndText()
        //{
        //    return await Cx.Books
        //        .Include(b => b.Subjects)
        //        .ThenInclude(s => s.SubjectText).ToListAsync();
        //}

        [HttpGet("{SubjectID}")]
        public async Task<SubjectText> GetSubjectText(int SubjectID)
        {
            return await Cx.Subjects.Where(x=>x.SubjectId==SubjectID)
                        .Select(x=>x.SubjectText).FirstAsync();
        }

    }
}
