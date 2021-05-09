using AStore.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStore.Library.Repository
{
    public class InMemStoreRepository : IStoreRepository
    {
        private readonly List<Customer> customersEntered = new();

        private readonly List<Customer> customersExited = new();

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
            var sectionEnteredList = (from entered in customersEntered
                                        group entered by entered.SectionName into sectionEntered
                                        select new Section
                                        {
                                            SectionName = sectionEntered.Key,
                                            CustomerCount = sectionEntered.Count()
                                        }).ToList();

            var sectionExitedList = (from exited in customersExited
                                       group exited by exited.SectionName into sectionExited
                                       select new Section
                                       {
                                           SectionName = sectionExited.Key,
                                           CustomerCount = sectionExited.Count()
                                       }).ToList();

            var sectionList = (from en in sectionEnteredList
                              select new Section
                              {
                                  SectionName = en.SectionName,
                                  CustomerCount = (en.CustomerCount - (from ex in sectionExitedList
                                                                       where ex.SectionName == en.SectionName
                                                                       select ex.CustomerCount).FirstOrDefault())
                              }).OrderByDescending(section => section.CustomerCount);

            return await Task.FromResult(sectionList); 
        }

        public async Task ResetSectionAsync(string section)
        {
            customersEntered.RemoveAll(customer => customer.SectionName == section);
            customersExited.RemoveAll(customer => customer.SectionName == section);

            customersEntered.Add(new Customer { SectionName = section, Timestamp = DateTimeOffset.UtcNow });
            customersExited.Add(new Customer { SectionName = section, Timestamp = DateTimeOffset.UtcNow });

            await Task.CompletedTask;
        }

        public async Task<Customer> GetSectionAsync(string section)
        {
            var customer = customersEntered.FirstOrDefault(customer => customer.SectionName == section);

            return await Task.FromResult(customer);
        }
    }
}
