using Microsoft.EntityFrameworkCore;

namespace DataModels.Models
{

    [Owned]
    public class UserSettings
    {
        public MemoryLevelEnum MemoryLevel { get; set; }
        public int DefaultQuestionCount { get; set; }
        public int DefaultTimeExam { get; set; }
    }

    public enum MemoryLevelEnum
    {
        Weak,
        Medium,
        High
    }
}