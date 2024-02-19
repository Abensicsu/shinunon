using DataModels.Data;
using DataModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        public SubjectController(SHcx cx)
        {
            Cx = cx;
        }

        public SHcx Cx { get; }

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
    }
}
