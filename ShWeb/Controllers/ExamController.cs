using DataModels.Data;
using DataModels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ShWeb.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExamController : Controller
    {
        public SHcx Cx { get; }
        private readonly UserManager<User> _userManager;


        public ExamController(SHcx cx, UserManager<User> userManager)
        {
            Cx = cx;
            _userManager = userManager;
        }

        [HttpGet]
        public ExamExecution ExamExecution(int ExamExecutionId)
        {
            var exam = Cx.ExamExecutions
                .Include(e => e.ExamAnswers)
                .ThenInclude(ea => ea.BaseQuestion)
                .ThenInclude(q => q.Answers)
                .Include(e => e.FromSubject)
                .ThenInclude(sub => sub.Book)
                .Where(x => x.ExamExecutionId == ExamExecutionId)
                .FirstOrDefault();

            return exam;
        }

        [HttpGet]
        public async Task<List<ExamExecution>> DailyExamExecutions()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                //return Unauthorized("User not found.");
            }

            var today = DateTime.Today;

            // Retrieve all exams for today
            var exams = await Cx.ExamExecutions
                .Where(ex => ex.UserId == currentUser.Id
                             && ex.StartTime.HasValue
                             && ex.StartTime.Value.Date == today)
                .OrderBy(ex => ex.StartTime)
                .Include(ex => ex.FromSubject)
                .ThenInclude(sub => sub.Book)
                .ToListAsync(); // Return a list of all today's exams

            return exams;
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

            //existingExecution.IsReviewed = examExecution.IsReviewed;
            existingExecution.CachedExamScore = examExecution.CachedExamScore;
            existingExecution.ExamStatus = examExecution.ExamStatus;
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
        public async Task<ActionResult<List<ExamExecution>>> GetExamsForUserByPeriodAsync(string period)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not found.");
            }

            DateTime today = DateTime.Today;
            IQueryable<ExamExecution> exams = Cx.ExamExecutions
                                                .Where(e => e.UserId == currentUser.Id)
                                                .Include(ex => ex.FromSubject)
                                                .ThenInclude(sub => sub.Book);

            switch (period.ToLower())
            {
                case "future":
                    exams = exams.Where(e => e.StartTime.HasValue && e.StartTime.Value.Date > today).OrderBy(e => e.StartTime);
                    break;
                case "present":
                    exams = exams.Where(e => e.StartTime.HasValue && e.StartTime.Value.Date == today).OrderBy(e => e.StartTime);
                    break;
                case "past":
                    exams = exams.Where(e => e.StartTime.HasValue && e.StartTime.Value.Date < today).OrderByDescending(e => e.StartTime);
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


        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<List<ExamExecution>>> GetUserPreviousExams(int FromSubjectId, int ToSubjectId, int examExecutionId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not found");
            }

            var currentExam = await Cx.ExamExecutions
                .Where(e => e.ExamExecutionId == examExecutionId)
                .FirstOrDefaultAsync();

            if (currentExam == null || !currentExam.EndTime.HasValue)
            {
                return BadRequest("Current exam not found or not completed.");
            }

            DateTime currentExamEndTime = currentExam.EndTime.Value;

            IQueryable<ExamExecution> exams = Cx.ExamExecutions
                .Where(e => e.UserId == currentUser.Id &&
                            e.ExamExecutionId != examExecutionId &&
                            e.FromSubjectId == FromSubjectId &&
                            e.ToSubjectId == ToSubjectId &&
                            e.ExamStatus == ExamStatusEnum.Completed &&
                            e.EndTime.Value < currentExamEndTime)
                .OrderByDescending(e => e.EndTime).Take(2);


            var result = await exams.ToListAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDeliberateReviews()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not found");
            }

            var reviews = await Cx.ExamExecutions
                .Where(ex => ex.UserId == currentUser.Id && ex.ExamType == ExamTypeEnum.ReviewExam)
                .Include(ex => ex.FromSubject.Book)
                    .ThenInclude(b => b.Subjects)
                .OrderBy(ex => ex.FromSubject.Book.HebrewBookName)  // By book-name
                .ThenBy(ex => ex.FromSubject.Ordinal)               //than by fromChapter
                .Select(ex => new
                {
                    ex.ExamExecutionId,
                    ex.FromSubject.Book,
                    ex.FromSubject,
                    ex.ToSubject
                })
                .ToListAsync();

            return Ok(reviews);
        }

        [HttpGet]
        public async Task<List<ExamExecution>> ExamExecutionsByDay(DateTime day)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                //return Unauthorized("User not found");
            }

            //var startOfDay = day.Date;

            // Retrieve all exams for today
            var exams = await Cx.ExamExecutions
                .Where(ex => ex.UserId == currentUser.Id
                             && ex.ExamStatus == ExamStatusEnum.Completed
                             //&& ex.StartTime.HasValue && ex.StartTime.Value.Date == day)
                             && ex.StartTime.Value.Day == day.Month && ex.StartTime.Value.Month == day.Day && ex.StartTime.Value.Year == day.Year)
                .Include(ex => ex.ExamAnswers)
                    .ThenInclude(ea => ea.Answer)
                    .ThenInclude(ea => ea.BaseQuestion)
                    .ThenInclude(bq => bq.Answers)
                .Include(ex => ex.FromSubject)
                .Include(ex => ex.ToSubject)
                .ToListAsync(); // Return a list of all today's exams

            return exams;
        }
    }
}
