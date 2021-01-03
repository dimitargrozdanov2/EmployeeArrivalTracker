using ReportingTool.Data;
using ReportingTool.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReportingTool.Services.Contracts
{
    public interface IArrivalService
    {
        Task AddRangeAsync(IEnumerable<Arrival> arrivals);
        Task DeleteRangeAsync(IEnumerable<Arrival> arrivals);
        Task<ICollection<Arrival>> GetAllAsync();
    }
}
