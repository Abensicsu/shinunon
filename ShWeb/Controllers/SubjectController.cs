using DataModels.Data;
using DataModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        public SHcx Cx { get; }
        public SubjectController(SHcx cx)
        {
            Cx = cx;
        }

        [HttpGet]
        public Subject[] AllSubjects()
        {
            return Cx.Subjects.ToArray();
        }

        [HttpPost]
        public void Add(Subject subject)
        {
            Cx.Add(subject);
            Cx.SaveChanges();
        }

        [HttpGet]
        public List<Subject> GetAllSubjectsWithText()
        {
            return Cx.Subjects
                .Include(s => s.SubjectText)
                .ToList();
        }
    }
}
