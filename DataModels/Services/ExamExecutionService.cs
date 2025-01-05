using DataModels.Data;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataModels.Services
{
    public class ExamExecutionService
    {
        public SHcx Cx { get; }

        public ExamExecutionService(SHcx cx)
        {
            Cx = cx;
        }

        public void ProcessExamExecution(ExamExecution examExecution)
        {
            if (examExecution == null) throw new ArgumentNullException(nameof(examExecution));

            // not initiated exam
            if (examExecution.ExamType == ExamTypeEnum.InitiatedExam)//not review or repeat
            {
                if (examExecution.ExamRepeatNumber == 0) //No repeat tests have been defined yet
                {
                    var user = Cx.Users
                        .Include(u => u.UserSettings)
                        .SingleOrDefault(u => u.Id == examExecution.UserId);

                    if (user == null) throw new ArgumentException("User not found");
                    //if (user.UserSettings == null) throw new ArgumentException("UserSettings not found");

                    //When creating a user, need to set the UserSettings and then you don't need this code.

                    //if (user.UserSettings == null)
                    //{
                    //    user.UserSettings = new UserSettings
                    //    {
                    //        MemoryLevel = MemoryLevelEnum.Medium,  // default
                    //        DefaultQuestionCount = 10,  // default
                    //        DefaultTimeExam = 30  // default
                    //    };
                    //    Cx.SaveChanges();
                    //}

                    CreateFutureExamExecutions(user, examExecution);
                }
            }
        }

        public void CreateFutureExamExecutions(User user, ExamExecution examExecution)
        {
            List<int> intervals = user.UserSettings.MemoryLevel switch
            {
                MemoryLevelEnum.High => new List<int> { 2, 30, 90 },
                MemoryLevelEnum.Medium => new List<int> { 1, 2, 7, 15 },
                MemoryLevelEnum.Weak => new List<int> { 1, 2, 5, 7, 9 },
                _ => throw new ArgumentOutOfRangeException()
            };

            int repeatNumber = 0;
            foreach (int interval in intervals)
            {
                repeatNumber++;

                var newExamExecution = new ExamExecution
                {
                    StartTime = DateTime.Now.AddDays(interval),
                    CorrectAnswersNum = 0,
                    PartiallyCorrectAnswersNum = 0,
                    WrongAnswersNum = 0,
                    QuestionsAnswered = 0,
                    ExamType = ExamTypeEnum.RepeatExam,
                    ExamRepeatNumber = repeatNumber,
                    FromSubjectId = examExecution.FromSubjectId,
                    ToSubjectId = examExecution.ToSubjectId,
                    UserId = user.Id,
                    ExamStatus = ExamStatusEnum.Pending,
                };
                Cx.ExamExecutions.Add(newExamExecution);
            }
            Cx.SaveChanges();
        }

        public ExamExecution createExamExecutionBySubjectsAndExamType(int userId, int fromSubjectId, int toSubjectId, ExamTypeEnum examType)
        {
            var newExamExecution = new ExamExecution
            {
                StartTime = DateTime.Now,
                CorrectAnswersNum = 0,
                PartiallyCorrectAnswersNum = 0,
                WrongAnswersNum = 0,
                QuestionsAnswered = 0,
                ExamType = examType,
                FromSubjectId = fromSubjectId,
                ToSubjectId = toSubjectId,
                UserId = userId,
                ExamStatus = ExamStatusEnum.Pending,
            };

            Cx.Add(newExamExecution);
            Cx.SaveChanges();

            return newExamExecution;
        }

        //Select questions for examExecution, QuestionsAmount.
        public void SelectQuestionsCreateExamAnswers(ExamExecution examExecution)
        {
            var fromSubject = Cx.Subjects
                .Where(sub => sub.SubjectId == examExecution.FromSubjectId)
                .FirstOrDefault();

            var toSubject = Cx.Subjects
                .Where(sub => sub.SubjectId == examExecution.ToSubjectId)
                .FirstOrDefault();

            var user = Cx.Users.Where(u => u.Id == examExecution.UserId)
                .Include(user => user.UserSettings)
                .FirstOrDefault();

            if (fromSubject == null || toSubject == null)
            {
                throw new InvalidOperationException("Invalid subject range specified.");
            }

            var fromSubjectBook = Cx.Books
                .FirstOrDefault(book => book.BookId == fromSubject.BookId);

            var toSubjectBook = Cx.Books
                .FirstOrDefault(book => book.BookId == toSubject.BookId);

            // Subjects between fromSubject and toSubject
            var subjects = Cx.Subjects
                .Where(s => s.Ordinal >= fromSubject.Ordinal
                        && s.Ordinal <= toSubject.Ordinal
                        && s.BookId >= fromSubjectBook.BookId && s.BookId <= toSubjectBook.BookId)
                .ToList();

            // Retrieve the list of SubjectIds
            var subjectIds = subjects.Select(s => s.SubjectId).ToList();

            // Retrieve all Questions with related data
            var questionsWithUserVersions = Cx.BaseQuestions
                .Include(bq => bq.Subject)
                .Include(bq => bq.Answers)
                .OfType<Question>() // Fetch only Questions (not UserQuestions directly)
                .Where(q => subjectIds.Contains(q.SubjectId)) // Filter by related subjects using Contains
                .Select(q => new
                {
                    BaseQuestion = (BaseQuestion)q,
                    UserQuestion = q.DerivedUserQuestions
                        .FirstOrDefault(uq => uq.UserId == user.Id) // Find user-specific version if it exists
                })
                .ToList();

            // Retrieve all standalone UserQuestions created by the user
            var userCreatedQuestions = Cx.BaseQuestions
                .Include(bq => bq.Subject)
                .Include(bq => bq.Answers)
                .OfType<UserQuestion>() // Only fetch UserQuestion objects
                .Where(uq => uq.UserId == user.Id && uq.OriginalQuestionId == null // Created by user and not linked to a Question
                                && subjectIds.Contains(uq.SubjectId)) // Filter by related subjects using Contains
                .Cast<BaseQuestion>() // Cast to BaseQuestion
                .ToList();

            // Replace base questions with user versions if they exist
            BaseQuestion[] questions = questionsWithUserVersions
                .Select(q => (BaseQuestion)(q.UserQuestion ?? q.BaseQuestion)) // Replace with UserQuestion if available
                .Union(userCreatedQuestions) // Add standalone UserQuestions
                .Distinct() // Ensure no duplicates
                .ToArray();

            if(questions.Length == 0)
            {
                Console.WriteLine("0 questions!!");
                return;
            }

            // Minimum of DefaultQuestionCount from userSettings and the total number of questions available.
            var amount = Math.Min(user.UserSettings.DefaultQuestionCount, questions.Length);

            // Shuffle the list of questions
            var random = new Random();
            var shuffledQuestions = questions.OrderBy(q => random.Next()).Take(amount).ToList();

            foreach (var question in shuffledQuestions)
            {
                var examAnswer = new ExamAnswer
                {
                    BaseQuestionId = question.BaseQuestionId,
                    ExamExecutionId = examExecution.ExamExecutionId,
                };
                Cx.ExamAnswers.Add(examAnswer);
            }
            Cx.SaveChanges();
        }
    }
}
