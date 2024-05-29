using DataModels.Data;
using DataModels.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamController : Controller
    {
        public SHcx Cx { get; }
        public ExamController(SHcx cx)
        {
            Cx = cx;
        }

        [HttpGet]
        public ExamExecution GetExamExecution(int ExamExecutionId)
        {
            var exam = Cx.ExamExecutions
                .Include(e => e.ExamAnswers)
                .ThenInclude(ea => ea.Question)
                .ThenInclude(q => q.Answers)
                .Where(x => x.ExamExecutionId == ExamExecutionId)
                .FirstOrDefault();

            return exam;
        }

        [HttpGet]
        public ICollection<ExamAnswer> GetExamAnswers(int ExamExecutionId)
        {
            return Cx.ExamAnswers
                .Where(x => x.ExamExecutionId == ExamExecutionId)
                .Include(x => x.Question)
                .ThenInclude(q => q.Answers)
                .ToList();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExamExecution([FromBody] ExamExecution examExecution)
        {
            var existingExecution = await Cx.ExamExecutions
                .Where(x => x.ExamExecutionId == examExecution.ExamExecutionId)
                .Include(x => x.ExamAnswers)
                .ThenInclude(x => x.Question)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync();

            if (existingExecution == null)
            {
                return NotFound();
            }

            // Update the ExamAnswers
            foreach (var examAanswer in examExecution.ExamAnswers)
            {
                var existingAnswer = existingExecution.ExamAnswers.FirstOrDefault(a => a.ExamAnswerId == examAanswer.ExamAnswerId);
                if (existingAnswer != null)
                {
                    // Update existing answer
                    existingAnswer.Answer = examAanswer.Answer;
                    existingAnswer.AnswerId = examAanswer.AnswerId;
                    existingAnswer.TextAnswer = examAanswer.TextAnswer;
                    existingAnswer.TimeSpent = examAanswer.TimeSpent;
                }
            }

            await Cx.SaveChangesAsync();
            return Ok();
        }
    }
}
