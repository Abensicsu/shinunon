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
            // Get the current user using UserManager
            var currentUser = await _userManager.GetUserAsync(User);

            // Retrieve all Questions with related data
            var questionsWithUserVersions = Cx.BaseQuestions
                .Include(bq => bq.Subject)
                .Include(bq => bq.Answers)
                .OfType<Question>() // Fetch only Questions (not UserQuestions directly)
                .Select(q => new
                {
                    BaseQuestion = (BaseQuestion)q,
                    UserQuestion = q.DerivedUserQuestions
                        .FirstOrDefault(uq => uq.UserId == currentUser.Id) // Find user-specific version if it exists
                })
                .ToList();

            // Retrieve all standalone UserQuestions created by the user
            var userCreatedQuestions = Cx.BaseQuestions
                .Include(bq => bq.Subject)
                .Include(bq => bq.Answers)
                .OfType<UserQuestion>()
                .Where(uq => uq.UserId == currentUser.Id && uq.OriginalQuestionId == null) // Created by user and not linked to a Question
                .Cast<BaseQuestion>() // Cast to BaseQuestion
                .ToList();

            // Replace base questions with user versions if they exist
            var resultQuestions = questionsWithUserVersions
                .Select(q => (BaseQuestion)(q.UserQuestion ?? q.BaseQuestion)) // Replace with UserQuestion if available
                .Union(userCreatedQuestions) // Add standalone UserQuestions
                .Distinct() // Ensure no duplicates
                .ToArray();

            return resultQuestions;
        }

        [HttpGet]
        public async Task<UserQuestion> getUserQuestion(int questionId)
        {
            var question = Cx.BaseQuestions
                .Where(bq => bq.BaseQuestionId == questionId)
                .Include(bq => bq.Answers).FirstOrDefault();
            return (UserQuestion)question;
        }

        [HttpGet]
        public async Task<Question> getQuestion(int questionId)
        {
            var question = Cx.BaseQuestions
                .Where(bq => bq.BaseQuestionId == questionId)
                .Include(bq => bq.Answers).FirstOrDefault();
            return (Question)question;
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
                question.Answers.ToList().ForEach(a => a.AnswerId = 0); // Reset all answer IDs to 0
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
               
                // Loop through the answers to update the AnswerText and IsCorrectAnswer
                for (int i = 0; i < question.Answers.Count; i++)
                {
                    // Assuming the answers are in the same order
                    var existingAnswer = existingUserQuestion.Answers.ElementAtOrDefault(i);
                    if (existingAnswer != null)
                    {
                        existingAnswer.AnswerText = question.Answers.ElementAtOrDefault(i).AnswerText;
                        existingAnswer.IsCorrectAnswer = question.Answers.ElementAtOrDefault(i).IsCorrectAnswer;
                    }
                }

                // Save the updated question and answers
                Cx.UserQuestions.Update(existingUserQuestion);
            }

            Cx.SaveChanges();

            return Ok();
        }
    }
}
