using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using ReportingTool.Data.Models;
using ReportingTool.Data.Repositories.Contracts;
using ReportingTool.Services;
using System;

namespace ReportingTool.Test.ServiceTests
{
    [TestFixture]
    public class ArrivalServiceTests
    {
        private Mock<IArrivalRepository> repoMock;
        private ServiceProvider serviceProvider;

        [SetUp]
        protected void TestSetup()
        {
            var databaseName = Guid.NewGuid().ToString();

            repoMock = new Mock<IArrivalRepository>();

            var builder = new ServiceCollection()
                 .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName).UseInternalServiceProvider(serviceProvider))
                .AddSingleton<ArrivalService>()
                .AddSingleton(repoMock.Object);

            serviceProvider = builder.BuildServiceProvider();
        }

        protected void TestCleanup()
        {
            serviceProvider?.Dispose();
            serviceProvider = null;
        }



    }
}