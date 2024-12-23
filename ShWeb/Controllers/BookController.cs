﻿using DataModels.Data;
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

        [HttpGet]
        public async Task<List<Book>> AllBooksIncludeSubjects()
        {
            return await Cx.Books.Include(b => b.Subjects).ToListAsync();
        }
    }
}
