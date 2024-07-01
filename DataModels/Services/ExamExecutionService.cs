using DataModels.Data;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                        .SingleOrDefault(u => u.UserId == examExecution.UserId);

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
                    WrongAnswers = 0,
                    CorrectAnswers = 0,
                    QuestionsAnswered = 0,
                    ExamType = ExamTypeEnum.RepeatExam,
                    ExamRepeatNumber = repeatNumber,
                    //QuestionId = examExecution.QuestionId,
                    FromSubjectId = examExecution.FromSubjectId,
                    ToSubjectId = examExecution.ToSubjectId,
                    UserId = user.UserId,
                };

                Cx.ExamExecutions.Add(newExamExecution);
            }
            Cx.SaveChanges();
        }
    }
}
