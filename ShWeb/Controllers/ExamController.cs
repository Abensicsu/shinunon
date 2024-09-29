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
        public ExamExecution ExamExecution(int ExamExecutionId)
        {
            var exam = Cx.ExamExecutions
                .Include(e => e.ExamAnswers)
                .ThenInclude(ea => ea.BaseQuestion)
                .ThenInclude(q => q.Answers)
                .Where(x => x.ExamExecutionId == ExamExecutionId)
                .FirstOrDefault();

            return exam;

            //var exam = Cx.ExamExecutions
            //        .Include(e => e.ExamAnswers)
            //        .ThenInclude(ea => ea.BaseQuestion)
            //            .ThenInclude(bq => ((Question)bq).Answers) // Include Answers for Question type
            //        .Include(e => e.ExamAnswers)
            //        .ThenInclude(ea => ea.BaseQuestion)
            //            .ThenInclude(bq => ((UserQuestion)bq).Answers) // Include Answers for UserQuestion type
            //        .Where(x => x.ExamExecutionId == ExamExecutionId)
            //        .FirstOrDefault();

            //return exam;
        }

        [HttpGet]
        public ICollection<ExamAnswer> ExamAnswers(int ExamExecutionId)
        {
            return Cx.ExamAnswers
                .Where(x => x.ExamExecutionId == ExamExecutionId)
                .Include(x => x.BaseQuestion)
                .ThenInclude(q => q.Answers)
                .ToList();
        }

        //????????????????
        [HttpPut]
        public async Task<IActionResult> ExamExecution([FromBody] ExamExecution examExecution)
        {
            var existingExecution = await Cx.ExamExecutions
                .Where(x => x.ExamExecutionId == examExecution.ExamExecutionId)
                .Include(x => x.ExamAnswers)
                .ThenInclude(x => x.BaseQuestion)
                .ThenInclude(q => q.Answers)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (existingExecution == null)
            {
                return NotFound();
            }

            // Detach any existing tracked entities to avoid tracking conflicts
            Cx.ChangeTracker.Clear();

            // Attach the examExecution and update its properties
            Cx.ExamExecutions.Attach(examExecution);
            Cx.Entry(examExecution).State = EntityState.Modified;

            existingExecution.IsReviewed = examExecution.IsReviewed;

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

                    Cx.Attach(examAanswer);
                    Cx.Entry(examAanswer).State = EntityState.Modified;
                }
            }

            await Cx.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<List<ExamExecution>>> GetExamsForUserAsync(int userId, string period)
        {
            DateTime today = DateTime.Today;
            IQueryable<ExamExecution> exams = Cx.ExamExecutions.Where(e => e.UserId == userId);

            switch (period.ToLower())
            {
                case "future":
                    exams = exams.Where(e => e.StartTime.HasValue && e.StartTime.Value.Date > today);
                    break;
                case "present":
                    exams = exams.Where(e => e.StartTime.HasValue && e.StartTime.Value.Date == today);
                    break;
                case "past":
                    exams = exams.Where(e => e.StartTime.HasValue && e.StartTime.Value.Date < today);
                    break;
                default:
                    return BadRequest("Invalid period. Use 'future', 'present' or 'past'.");
            }

            var examsList = await exams.ToListAsync();

            if (examsList == null)
            {
                return NotFound();
            }

            return Ok(examsList);
        }
    }
}
