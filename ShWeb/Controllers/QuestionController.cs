using DataModels.Data;
using DataModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ShWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        public QuestionController(SHcx cx)
        {
            Cx = cx;
        }

        public SHcx Cx { get; }

        [HttpGet]
        public object AllQuestions(int userId)
        {
            // All questions
            var questions = Cx.BaseQuestions
                .Include(bq => bq.Subject)
                .Include(bq => (bq as Question).DerivedUserQuestions)
                .Include(bq => bq.Answers)
                .ToList();

            return questions;
        }

        [HttpPost]
        public void Add(UserQuestion question)
        {
            Cx.BaseQuestions.Add(question);
            Cx.SaveChanges();
        }

        [HttpPut]
        public IActionResult Edit(UserQuestion updatedQuestion)
        {
            var question = Cx.BaseQuestions.FirstOrDefault(q => q.BaseQuestionId == updatedQuestion.BaseQuestionId);
            if (question == null)
            {
                return NotFound();
            }

            question.QuestionText = updatedQuestion.QuestionText;
            question.QuestionType = updatedQuestion.QuestionType;
            question.SubjectId = updatedQuestion.SubjectId;

            Cx.SaveChanges();
            return Ok();
        }

        [HttpDelete("{questionId}")]
        public IActionResult Delete(int questionId)
        {
            var question = Cx.BaseQuestions
               .Include(q => q.Answers) // Include answers to delete them too
               .FirstOrDefault(q => q.BaseQuestionId == questionId);

            if (question == null)
            {
                return NotFound();
            }

            // Remove the answers first
            if (question.Answers.Count > 0)
            {
                Cx.Answers.RemoveRange(question.Answers);
            }

            // Remove the question
            Cx.BaseQuestions.Remove(question);
            Cx.SaveChanges();
            return Ok();
        }
    }
}
