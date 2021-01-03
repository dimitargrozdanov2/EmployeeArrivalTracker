using ReportingTool.Data.Models;
using ReportingTool.Data.Repositories.Contracts;

namespace ReportingTool.Data.Repositories
{
    public class ServiceTokenRepository : DbRepository<ServiceToken>, IServiceTokenRepository
    {
        public ServiceTokenRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
