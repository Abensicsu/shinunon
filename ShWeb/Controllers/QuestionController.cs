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
        public async Task<BaseQuestion[]> AllQuestions(int userId)
        {
            // All questions
            BaseQuestion[] questions = await Cx.BaseQuestions
                .Include(bq => bq.Subject)
                .Include(bq => (bq as Question).DerivedUserQuestions)
                .Include(bq => bq.Answers)
                .ToArrayAsync();

            return questions;
        }

        [HttpPost]
        public void Add(UserQuestion question)
        {
            Cx.BaseQuestions.Add(question);
            Cx.SaveChanges();
        }

        //[HttpPut]
        //public IActionResult Edit(UserQuestion updatedQuestion)
        //{
        //    return Ok();
        //}

        [HttpDelete("{questionId}/{userId}")]
        public IActionResult Delete(int questionId, int userId)//delete original question or only userquestion ??
        {
            var question = Cx.UserQuestions
               .Include(q => q.Answers) // Include answers to delete them too
               .FirstOrDefault(q => q.BaseQuestionId == questionId && q.UserId == userId);

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
