using ReportingTool.Data.Models;
using ReportingTool.Data.Repositories.Contracts;
using ReportingTool.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportingTool.Services
{
    public class ArrivalService : IArrivalService
    {
        private readonly IArrivalRepository arrivalRepository;
        public ArrivalService(IArrivalRepository arrivalRepository)
        {
            this.arrivalRepository = arrivalRepository;
        }

        public async Task AddRangeAsync(IEnumerable<Arrival> arrivals)
        {
            await arrivalRepository.AddRangeAsync(arrivals);
        }
        public async Task<ICollection<Arrival>> GetAllAsync()
        {
            return await arrivalRepository.GetAllAsync();
        }
        public async Task DeleteRangeAsync(IEnumerable<Arrival> arrivals)
        {
            await arrivalRepository.DeleteRangeAsync(arrivals);
        }
    }
}
