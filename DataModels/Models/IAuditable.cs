using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Models
{
    public interface IAuditable
    {
        DateTime DateCreatedAudit { get; set; }
        DateTime LastUpdatedAudit { get; set; }
    }
}
