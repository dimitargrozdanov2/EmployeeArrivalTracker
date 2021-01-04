using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using ReportingTool.Data.Models;
using ReportingTool.Services.Contracts;
using ReportingTool.Web.Controllers;
using ReportingTool.Web.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReportingTool.Test.ControllerTests
{
    [TestFixture]

    public class HomeControllerTests
    {
        private Mock<IArrivalService> arrivalServiceMock;
        private Mock<ITokenService> tokenServiceMock;

        private HomeController sut;
        private ServiceProvider sp;

        [SetUp]
        protected void TestSetup()
        {
            var sp = new ServiceCollection()
               .BuildServiceProvider();

            this.arrivalServiceMock = new Mock<IArrivalService>();
            this.tokenServiceMock = new Mock<ITokenService>();
            this.sut = new HomeController(tokenServiceMock.Object, arrivalServiceMock.Object);
        }

        [TearDown]
        protected void TestCleanup()
        {
            sp?.Dispose();

            sp = null;
        }

        [Test]
        public async Task Index_Should_Have_Redirect()
        {
            tokenServiceMock.Setup(e => e.GetServiceToken(It.IsAny<DateTime>())).ReturnsAsync((ServiceToken)null);
            var result = await this.sut.Index("", "", "", "", 1);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

    }
}
