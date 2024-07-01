using DataModels.Data;
using DataModels.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace DataModels.Services
{
    public class PlanExamService
    {
        public SHcx Cx { get; }

        public PlanExamService(SHcx cx)
        {
            Cx = cx;
        }

        //create examexecutions according to planExam 
        public void CreateExamExecutions(PlanExam planExam)
        {
            var bookFrom = Cx.Books
                .Where(book => book.BookId == planExam.FromSubject.BookId).FirstOrDefault();
            var bookTo = Cx.Books
                .Where(book => book.BookId == planExam.ToSubject.BookId).FirstOrDefault();

            int OrderBookFrom = bookFrom.BookOrder;
            int OrderBookTo = bookTo.BookOrder;

            //Get books that contains FromSubject to ToSubject.
            ICollection<Book> Books = Cx.Books
                .Where(book => book.BookOrder >= OrderBookFrom && book.BookOrder <= OrderBookTo
                            && book.BookType == planExam.FromSubject.Book.BookType)
                .Include(x => x.Subjects)
                .ToList();

            Subject[] allSubjects = Books
                .SelectMany(book => book.Subjects) // Subjects from all books
                .OrderBy(x => x.Book.BookOrder)
                .ThenBy(x => x.Ordinal)
                .ToArray();

            // Find the index of FromSubject in allSubjects
            int fromIndex = Array.FindIndex(allSubjects, subject => subject.SubjectName == planExam.FromSubject.SubjectName);

            // Find the index of ToSubject in allSubjects
            int toIndex = Array.FindIndex(allSubjects, subject => subject.SubjectName == planExam.ToSubject.SubjectName);

            // Extract the subjects between "FromSubject" and "ToSubject"
            Subject[] subjects = allSubjects
                .Skip(fromIndex) // Skip subjects before "FromSubject"
                .Take(toIndex - fromIndex + 1) // Take subjects between "FromSubject" and "ToSubject"
                .ToArray();
            //bool flag = false;
            //if (planExam.SubjectNum == 0)//by sub-subjects
            //    flag = true;

            int i = 0, j = 0, subSubjectsAdded = 0; //i = subjectPointer, j = subSubjectPointer, subSubjectsAdded
            Subject fromSubject = null, toSubject = null;

            // Create new ExamExecutions
            while (i < subjects.Length && subjects[i] != null)
            {
                fromSubject = subjects[i];

                if ((subjects[i].SubSubjectCount - j) > planExam.SubSubjectNum)
                {
                    toSubject = subjects[i];
                    j += planExam.SubSubjectNum;
                }
                else if ((subjects[i].SubSubjectCount) - j == planExam.SubSubjectNum)
                {
                    toSubject = subjects[i];
                    i++;
                    j = 0;
                }
                else //if (flag && (j + subjects[i].SubSubjectCount) < planExam.SubSubjectNum)
                {
                    while (i < subjects.Length && subjects[i].SubSubjectCount - j <= planExam.SubSubjectNum - subSubjectsAdded)
                    {
                        if (subjects[i].SubSubjectCount - j == planExam.SubSubjectNum - subSubjectsAdded)
                        {
                            toSubject = subjects[i];
                            subSubjectsAdded += (int)subjects[i].SubSubjectCount - j;
                            i++;
                            break;
                        }
                        else // (j + subjects[i].SubSubjectCount) < planExam.SubSubjectNum
                        {
                            subSubjectsAdded += (int)subjects[i].SubSubjectCount - j;
                            // if (i + 1 == subjects.Length)
                            //  break;
                            i++;
                            j = 0;
                        }
                    }
                    j = planExam.SubSubjectNum - subSubjectsAdded;
                    subSubjectsAdded = 0;
                    if (i == subjects.Length)
                        toSubject = subjects[i - 1];
                    else
                        toSubject = subjects[i];

                }

                ExamExecution newExamExecution = new ExamExecution
                {
                    StartTime = null,//check
                    EndTime = null,
                    CorrectAnswers = 0,
                    WrongAnswers = 0,
                    QuestionsAnswered = 0,
                    QuestionId = 0,
                    FromSubjectId = fromSubject.SubjectId,
                    ToSubjectId = toSubject.SubjectId,
                    PlanExamId = planExam.PlanExamId,
                    UserId = planExam.UserId
                };
                Cx.ExamExecutions.Add(newExamExecution);
                Cx.SaveChanges();
            }
        }

        //select questions for examExecution, QuestionsAmount.
        public void SelectQuestions(ExamExecution examExecution)
        {
            var fromSubject = Cx.Subjects
                .Where(sub => sub.SubjectId == examExecution.FromSubjectId).FirstOrDefault();

            var toSubject = Cx.Subjects
                .Where(sub => sub.SubjectId == examExecution.ToSubjectId).FirstOrDefault();

            var plan = Cx.PlanExams
                .Where(pl => pl.PlanExamId == examExecution.PlanExamId).FirstOrDefault();

            // subjects between fromSubject and toSubject
            var subjects = Cx.Subjects
                .Where(s => s.Ordinal >= fromSubject.Ordinal
                        && s.Ordinal <= toSubject.Ordinal)
                .ToList();

            // Questions associated with subjects
            var questions = Cx.Questions
                .Where(q => subjects.Contains(q.Subject))
                .ToList();

            // Minimum of QuestionsAmount and the total number of questions available.
            var amount = Math.Min(plan.QuestionsAmount, questions.Count);

            var random = new Random();
            var shuffledIndices = new HashSet<int>();

            var minId = questions.Min(q => q.QuestionId);
            var maxId = questions.Max(q => q.QuestionId);

            while (shuffledIndices.Count < amount)
            {
                shuffledIndices.Add(random.Next(minId, maxId));
            }

            ICollection<Question> selectedQuestions = questions
                .Where(q => shuffledIndices.Contains(q.QuestionId))
                .ToList();

            foreach (var question in selectedQuestions)
            {
                var examAnswer = new ExamAnswer
                {
                    QuestionId = question.QuestionId,
                    ExamExecutionId = examExecution.ExamExecutionId,
                };
                Cx.ExamAnswers.Add(examAnswer);
            }

            Cx.SaveChanges();
        }
    }
}
