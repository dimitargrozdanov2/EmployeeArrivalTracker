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
    public class ArrivalRepositoryTests
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
                .AddSingleton<ArrivalRepository>();

            serviceProvider = builder.BuildServiceProvider();
        }

        [TearDown]
        protected void TestCleanup()
        {
            serviceProvider?.Dispose();
            serviceProvider = null;
        }

        protected Arrival CreateEntity = new Arrival() { Id = 1, EmployeeId = 2, When = DateTime.UtcNow.ToString()  };
        protected virtual List<string> CreateIgnoreProperties { get; set; } = new List<string>();
        protected List<Arrival> CreatedEntities = new List<Arrival>() { new Arrival(), new Arrival() };
        protected Arrival UpdatedEntity = new Arrival() { Id = 1, EmployeeId = 2, When = DateTime.UtcNow.ToString() };


        [Test]
        public void Add_Should_Throw_Not_Found()
        {
            Assert.ThrowsAsync<NotFoundException>(async () =>
            await serviceProvider.GetService<ArrivalRepository>().AddAsync(null));
        }

        [Test]
        public async Task Add_Should_Add_to_Database()
        {
            var result = await serviceProvider.GetService<ArrivalRepository>().AddAsync(CreateEntity);
            result.AssertEqualProperties(CreateEntity);
        }

        [Test]
        public async Task Get_Should_Return_Entity()
        {
            using var dbContext = serviceProvider.GetService<ApplicationDbContext>();
            await dbContext.Set<Arrival>().AddAsync(CreateEntity);
            await dbContext.SaveChangesAsync();
            var result = await serviceProvider.GetService<ArrivalRepository>().GetAsync(CreateEntity.Id);
            result.AssertEqualProperties(CreateEntity);
        }

        [Test]
        public async Task Get_Should_Return_Not_Found()
        {
            using var dbContext = serviceProvider.GetService<ApplicationDbContext>();
            await dbContext.Set<Arrival>().AddAsync(CreateEntity);
            await dbContext.SaveChangesAsync();
            Assert.ThrowsAsync<NotFoundException>(async () =>
                await serviceProvider.GetService<ArrivalRepository>().GetAsync(default(int)));
        }

        [Test]
        public void GetAll_Should_Return_Entities()
        {
            using var dbContext = serviceProvider.GetService<ApplicationDbContext>();
            dbContext.Set<Arrival>().AddRange(CreatedEntities);
            dbContext.SaveChanges();
            var result = serviceProvider.GetService<ArrivalRepository>().GetAll().AsQueryable();
            var x = result.ToList();
            Assert.AreEqual(x.Count, 2);
        }

        [Test]
        public async Task GetSingle_Should_Return_SingleEntity()
        {
            using var dbContext = serviceProvider.GetService<ApplicationDbContext>();
            await dbContext.Set<Arrival>().AddAsync(CreateEntity);
            await dbContext.SaveChangesAsync();
            var result = await serviceProvider.GetService<ArrivalRepository>().GetSingleAsync(e => e.Id.Equals(CreateEntity.Id));
            result.AssertEqualProperties(CreateEntity);
        }

        [Test]
        public async Task Update_Should_Return_ModifiedEntity()
        {
            using var dbContext = serviceProvider.GetService<ApplicationDbContext>();
            dbContext.Set<Arrival>().Add(CreateEntity);
            dbContext.SaveChanges();
            var updated = dbContext.Set<Arrival>().Find(CreateEntity.Id);
            UpdatedEntity.CopyProperties(updated);
            await serviceProvider.GetService<ArrivalRepository>().UpdateAsync(updated);
            updated = dbContext.Set<Arrival>().Find(UpdatedEntity.Id);
            updated.AssertEqualProperties(UpdatedEntity);
        }

        [Test]
        public void Update_Should_Return_Not_Found()
        {
            Assert.ThrowsAsync<NotFoundException>(async () =>
                await serviceProvider.GetService<ArrivalRepository>().UpdateAsync(null));
        }

        [Test]
        public async Task Delete_Should_Delete_Entity()
        {
            using var dbContext = serviceProvider.GetService<ApplicationDbContext>();
            await dbContext.Set<Arrival>().AddAsync(CreateEntity);
            await dbContext.SaveChangesAsync();
            await serviceProvider.GetService<ArrivalRepository>().DeleteAsync(CreateEntity.Id);
            var deletedEntity = await dbContext.Set<Arrival>().FindAsync(CreateEntity.Id);
            Assert.IsNull(deletedEntity);
        }
    }
}
