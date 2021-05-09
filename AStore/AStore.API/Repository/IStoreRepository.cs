using AStore.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AStore.API.Repository
{
    public interface IStoreRepository
    {
        public Task AddCustomerAsync(Customer customerEnter);
        public Task RemoveCustomerAsync(Customer customerExit);
        public Task<IEnumerable<Section>> GetSectionListAsync();
        public Task ResetSectionAsync(string section);

        public Task<Customer> GetSectionAsync(string section);
    }
}
