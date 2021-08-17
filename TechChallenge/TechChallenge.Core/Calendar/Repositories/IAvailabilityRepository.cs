using System.Collections.Generic;
using System.Threading.Tasks;
using TechChallenge.Core.Calendar.Entities;

namespace TechChallenge.Core.Calendar.Repositories
{
    public interface IAvailabilityRepository
    {
        Task<bool> Insert(Availability availability);
        Task<List<Availability>> Find();        
        Task<List<Availability>> Find(List<string> userIds);      
    }
}