using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public class DeletedRecord
    {
        public int Id { get; set; }
        public string TableName { get; set; } = string.Empty;
        public string RecordJson { get; set; } = string.Empty; // Store the serialized deleted record
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
        public string UserId { get; set; } = string.Empty; // Optional: Include user if needed
    }
}
