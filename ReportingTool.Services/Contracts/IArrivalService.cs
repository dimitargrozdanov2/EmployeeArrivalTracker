using ReportingTool.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingTool.Services.Contracts
{
    public interface IArrivalService
    {
        Task AddRangeAsync(IEnumerable<Arrival> arrivals);

        Task DeleteRangeAsync(IEnumerable<Arrival> arrivals);

        IQueryable<Arrival> GetAll();
    }
}
