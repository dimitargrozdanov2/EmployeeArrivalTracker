using ReportingTool.Data.Models;
using ReportingTool.Data.Repositories.Contracts;

namespace ReportingTool.Data.Repositories
{
    public class ArrivalRepository : DbRepository<Arrival>, IArrivalRepository
    {
        public ArrivalRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

    }
}
