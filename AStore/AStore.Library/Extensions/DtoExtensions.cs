using AStore.Library.Models;
using AStore.Library.Dtos;

namespace AStore.Library.Extensions
{
    public static class DtoExtensions
    {
        public static SectionDto AsDto(this Section section)
        {
            return new SectionDto
            {
                SectionName = section.SectionName,
                CustomerCount = section.CustomerCount
            };
        }
    }
}
