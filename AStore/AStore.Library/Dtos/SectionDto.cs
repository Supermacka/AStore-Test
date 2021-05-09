using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStore.Library.Dtos
{
    public record SectionDto
    {
        public string SectionName { get; init; }
        public int CustomerCount { get; init; }
    }
}
