using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using ReportingTool.Data.Exceptions;
using ReportingTool.Data.Models;
using ReportingTool.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingTool.Test.RepositoryTests
{
    [TestFixture]
    public class ServiceRepositoryTests
    {
        protected ServiceProvider serviceProvider;

        [SetUp]
        public void Setup()
        {
            var databaseName = Guid.NewGuid().ToString();
            TestCleanup();

            var builder = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName).UseInternalServiceProvider(serviceProvider))
                .AddSingleton<ServiceTokenRepository>();

            serviceProvider = builder.BuildServiceProvider();
        }

        [TearDown]
        protected void TestCleanup()
        {
            serviceProvider?.Dispose();
            serviceProvider = null;
        }

        protected ServiceToken CreateEntity = new ServiceToken() { Token = new Guid().ToString(),  Expires = DateTime.UtcNow.AddHours(2).ToString() };
        protected virtual List<string> CreateIgnoreProperties { get; set; } = new List<string>();
        protected List<ServiceToken> CreatedEntities = new List<ServiceToken>() { new ServiceToken() { Token = new Guid().ToString(), Expires = DateTime.UtcNow.AddHours(2).ToString() }, new ServiceToken() { Token = new Guid().ToString(), Expires = DateTime.UtcNow.AddHours(4).ToString() } };
        protected ServiceToken UpdatedEntity = new ServiceToken() { Token = new Guid().ToString(), Expires = DateTime.UtcNow.AddHours(3).ToString() };


        [Test]
        public void Add_Should_Throw_Not_Found()
        {
            Assert.ThrowsAsync<NotFoundException>(async () =>
            await serviceProvider.GetService<ServiceTokenRepository>().AddAsync(null));
        }

        [Test]
        public async Task Add_Should_Add_to_Database()
        {
            var result = await serviceProvider.GetService<ServiceTokenRepository>().AddAsync(CreateEntity);
            result.AssertEqualProperties(CreateEntity);
        }


        [Test]
        public Task GetAll_Should_Return_Entities()
        {
            using var dbContext = serviceProvider.GetService<ApplicationDbContext>();
            dbContext.Set<ServiceToken>().AddRange(CreatedEntities);
            dbContext.SaveChanges();
            var result = serviceProvider.GetService<ServiceTokenRepository>().GetAll().AsQueryable();
            var x = result.ToList();
            Assert.AreEqual(x.Count, 2);
            return Task.CompletedTask;
        }

    }
}
