using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStore.Library.Models
{
    public record Customer
    {
        public string SectionName { get; init; }
        public DateTimeOffset Timestamp { get; init; }
    }
}
