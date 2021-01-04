using ReportingTool.Data.Models;
using ReportingTool.Data.Repositories.Contracts;
using ReportingTool.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
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

        public async Task AddRangeAsync(IEnumerable<Arrival> arrivals) => await arrivalRepository.AddRangeAsync(arrivals);

        public IQueryable<Arrival> GetAll() => arrivalRepository.GetAll();

        public async Task DeleteRangeAsync(IEnumerable<Arrival> arrivals) => await arrivalRepository.DeleteRangeAsync(arrivals);

    }
}
