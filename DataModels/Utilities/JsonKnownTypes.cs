using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModels.Models;

namespace DataModels.Utilities
{
    public static class JsonKnownTypes
    {
        public static readonly Type[] KnownTypes = new Type[]
        {
            //typeof(BaseQuestion),
            typeof(Question),
            typeof(UserQuestion),
            // Add other types as needed
        };
    }
}
