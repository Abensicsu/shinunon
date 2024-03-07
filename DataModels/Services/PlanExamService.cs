using DataModels.Data;
using DataModels.Models;

namespace DataModels.Services
{
    public class PlanExamService
    {
        public SHcx Cx { get; }
        public PlanExamService(SHcx cx)
        {
            Cx = cx;
        }

        //create examExecutions according to planExam
        public ICollection<ExamExecution> CreateExamExecutions(PlanExam planExam)
        {
            int count = Cx.Subjects.Count(subject => subject.SubjectName == planExam.FromSubject.SubjectName);
            //planExam.ExamFrequency ??
            int numOfExecutions = count / planExam.SubjectNum;
            for (int i = 0; i < numOfExecutions; i++)
            {
                planExam.ExamExecutions.Add(new ExamExecution
                {
                    StartTime = null,
                    EndTime = null,
                    QuestionsAnswered = 0,
                    CorrectAnswers = 0,
                    WrongAnswers = 0,
                    CurrentQuestion = null,
                    Subject = planExam.FromSubject,
                    User = planExam.User
                });
            }
            return planExam.ExamExecutions;
        }

        //select questions for exams
        public void SelectQuestions(ICollection<ExamExecution> examExecutions)
        {
            for (int i = 0; i < examExecutions.Count(); i++)
            {
                //ExamExecution.
            }
        }
    }
}
