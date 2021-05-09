using AStore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStore.API.Repository
{
    public class InMemStoreRepository : IStoreRepository
    {
        private readonly List<Customer> customersEntered = new()
        {
            new Customer { SectionName = "Barn", Timestamp = DateTimeOffset.UtcNow },
            new Customer { SectionName = "Vuxen", Timestamp = DateTimeOffset.UtcNow },
            new Customer { SectionName = "Barn", Timestamp = DateTimeOffset.UtcNow }
        };

        private readonly List<Customer> customersExited = new()
        {
            new Customer { SectionName = "Barn", Timestamp = DateTimeOffset.UtcNow },
            new Customer { SectionName = "Barn", Timestamp = DateTimeOffset.UtcNow }
        };

        public async Task AddCustomerAsync(Customer customerEnter)
        {
            customersEntered.Add(customerEnter);

            await Task.CompletedTask;
        }

        public async Task RemoveCustomerAsync(Customer customerExit)
        {
            customersExited.Add(customerExit);

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Section>> GetSectionListAsync()
        {
            // Lista alla zoner och antal besökare i varje zon.
            var sectionEnteredQuery = (from entered in customersEntered
                                    group entered by entered.SectionName into sectionEntered
                                    orderby sectionEntered.Count() descending
                                    select new Section
                                    {
                                        SectionName = sectionEntered.Key,
                                        CustomerCount = sectionEntered.Count()
                                    }).ToList();

            var sectionExitedQuery = (from exited in customersExited
                                       group exited by exited.SectionName into sectionExited
                                       select new Section
                                       {
                                           SectionName = sectionExited.Key,
                                           CustomerCount = sectionExited.Count()
                                       }).ToList();

            var sectionList = sectionEnteredQuery.Select(en => new Section()
            {
                SectionName = en.SectionName,
                CustomerCount = en.CustomerCount - (sectionExitedQuery.FirstOrDefault(ex => ex.SectionName == en.SectionName) ?.CustomerCount ?? 0)
            });

            return await Task.FromResult(sectionList); 
        }

        public async Task ResetSectionAsync(string section)
        {
            customersEntered.RemoveAll(customer => customer.SectionName == section);
            customersExited.RemoveAll(customer => customer.SectionName == section);

            await Task.CompletedTask;
        }

        public async Task<Customer> GetSectionAsync(string section)
        {
            var customer = customersEntered.FirstOrDefault(customer => customer.SectionName == section);

            return await Task.FromResult(customer);
        }
    }
}
