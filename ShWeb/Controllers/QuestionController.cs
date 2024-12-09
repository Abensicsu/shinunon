using DataModels.Data;
using DataModels.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ShWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        public SHcx Cx { get; }
        private readonly UserManager<User> _userManager;

        public QuestionController(SHcx cx, UserManager<User> userManager)
        {
            Cx = cx;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<BaseQuestion[]> AllQuestionsAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            // Retrieve all questions
            var questions = Cx.BaseQuestions
                .Include(bq => bq.Subject)
                .Include(bq => bq.Answers)
                .Include(bq => (bq as Question).DerivedUserQuestions) // Include derived UserQuestions
                .ToList();

            // Replace any base question with the user's version if it exists
            var resultQuestions = questions.Select(q =>
            {
                // Check if there's a UserQנuestion derived from this question for the specified user
                var userQuestion = (q as Question)?.DerivedUserQuestions
                    .FirstOrDefault(uq => uq.UserId == currentUser.Id);

                // If a UserQuestion exists, return it; otherwise, return the original BaseQuestion
                return userQuestion ?? q;
            }).ToList();

            return resultQuestions.Distinct().ToArray();
        }

        [HttpPost]
        public IActionResult Add(UserQuestion question)
        {
            if (question == null)
            {
                return NotFound();
            }

            if (question.Answers == null || question.Answers.Count == 0)
            {
                return BadRequest("At least one answer is required.");
            }

            Cx.UserQuestions.Add(question);
            Cx.SaveChanges();

            return Ok();
        }

        [HttpDelete("{questionId}")]
        public async Task<IActionResult> DeleteAsync(int questionId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var question = Cx.UserQuestions
               .Include(q => q.Answers) // Include answers to delete them too
               .FirstOrDefault(q => q.BaseQuestionId == questionId && q.UserId == currentUser.Id);

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

        [HttpPut]
        public IActionResult Edit(UserQuestion question)
        {
            if (question == null)
            {
                return NotFound();
            }

            if (question.Answers == null || question.Answers.Count == 0)
            {
                return BadRequest("At least one answer is required.");
            }

            // Create a new UserQuestion
            if (question.BaseQuestionId == 0)
            {
                Cx.UserQuestions.Add(question);
                Cx.SaveChanges();
                return Ok();
            }

            // Check if editing existing UserQuestion
            var existingUserQuestion = Cx.UserQuestions
                    .Include(q => q.Answers) // Include answers for deletion
            .FirstOrDefault(q => q.BaseQuestionId == question.BaseQuestionId && q.UserId == question.UserId);

            // Update existing UserQuestion
            if (existingUserQuestion != null)
            {
                existingUserQuestion.SubjectId = question.SubjectId;
                existingUserQuestion.QuestionText = question.QuestionText;
                existingUserQuestion.QuestionType = question.QuestionType;
                // Remove existing answers
                Cx.Answers.RemoveRange(existingUserQuestion.Answers);

                // Add new answers
                existingUserQuestion.Answers = question.Answers;

                // Update the existing question
                Cx.UserQuestions.Update(existingUserQuestion);
            }

            Cx.SaveChanges();

            return Ok();
        }
    }
}
