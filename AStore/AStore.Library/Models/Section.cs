using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStore.Library.Models
{
    public record Section
    {
        public string SectionName { get; init; }
        public int CustomerCount { get; init; }
    }
}
