using DataModels.Data;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShWeb.Migrations;

namespace ShWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ForumController : Controller
    {
        public SHcx Cx { get; }
        public ForumController(SHcx cx)
        {
            Cx = cx;
        }

        [HttpGet]
        public List<ForumQuestion> AllForumQuestions()
        {
            // Retrieve all forumQuestions
            var fqs = Cx.ForumQuestions
                .Include(fq => fq.Comments)
                .Include(fq => fq.Subject)
                    .ThenInclude(s => s.Book) // אם יש הפניה למחלקה נוספת ב-Subject
                .Include(fq => fq.User)
                .OrderByDescending(fq => fq.CreateDate)
                .ToList();
            return fqs;
        }

        [HttpGet]
        public ForumQuestion GetQuestion(int questionId)
        {
            var fq = Cx.ForumQuestions
                       .Where(fq => fq.ForumQuestionId == questionId)
                       .Include(fq => fq.Comments.OrderBy(c => c.CreateDate))
                           .ThenInclude(comment => comment.Comments.OrderBy(c => c.CreateDate))
                               //.ThenInclude(innerComment => innerComment.User)
                       .Include(fq => fq.Comments)
                           .ThenInclude(comment => comment.User)
                       .Include(fq => fq.User)
                       .FirstOrDefault();
            return fq;
        }

        [HttpPost]
        public IActionResult AddForumQuestion(ForumQuestion forumQuestion)
        {
            if (forumQuestion == null)
            {
                return NotFound();
            }

            Cx.ForumQuestions.Add(forumQuestion);
            Cx.SaveChanges();
            return Ok();
        }

        [HttpPut]
        public IActionResult UpdateForumQuestion(ForumQuestion forumQuestion)
        {
            if (forumQuestion == null)
            {
                return NotFound();
            }

            Cx.ForumQuestions.Update(forumQuestion);
            Cx.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult CommentForumQuestion(ForumComment forumComment)
        {
            if (forumComment == null || forumComment.ForumQuestionId == 0)
            {
                return BadRequest("Invalid comment data.");
            }

            var question = Cx.ForumQuestions
                .Include(q => q.Comments)
                .FirstOrDefault(q => q.ForumQuestionId == forumComment.ForumQuestionId);

            if (question == null)
            {
                return NotFound("Question not found.");
            }

            Cx.ForumComments.Add(forumComment);
            Cx.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult AddCommentToComment(ForumComment comment)
        {
            if (comment == null || comment.ParentCommentId == null)
                return BadRequest();

            var parentComment = Cx.ForumComments
                .FirstOrDefault(c => c.ForumCommentId == comment.ParentCommentId);

            if (parentComment == null)
                return NotFound();

            parentComment.Comments ??= new List<ForumComment>();
            parentComment.Comments.Add(comment);

            Cx.SaveChanges();

            return Ok();
        }

    }
}
