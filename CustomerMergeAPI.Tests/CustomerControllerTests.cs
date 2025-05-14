
using Moq;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CustomerMergeAPI.Domain.Commands;
using CustomerMergeAPI.Domain.DTOs;
using CustomerMergeAPI.Domain.Queries;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using CustomerMergeAPI.Domain.Constants;
using Microsoft.Extensions.Logging;

namespace CustomerMergeAPI.Tests.Controllers
{
    [TestClass]
    public class CustomerControllerTests
    {
        private Mock<IMediator> _mockMediator;
        private CustomerController _controller;
        private Mock<ILogger<CustomerController>> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockMediator = new Mock<IMediator>();
            _mockLogger = new Mock<ILogger<CustomerController>>();
            _controller = new CustomerController(_mockMediator.Object, _mockLogger.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [TestMethod]
        public async Task GetDuplicates_ReturnsOkResult()
        {
            var resultData = new PagedCustomerResult();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetDuplicatesQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(resultData);

            var result = await _controller.GetDuplicates(1, 10, "test");

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(resultData, okResult.Value);
        }

        [TestMethod]
        public async Task GetResolved_ReturnsOkResult()
        {
            var resultData = new PagedCustomerResult();
            _mockMediator.Setup(m => m.Send(It.IsAny<GetResolvedQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(resultData);

            var result = await _controller.GetResolved(1, 10, "test");

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.AreEqual(resultData, okResult.Value);
        }

        [TestMethod]
        public async Task MergeGroup_ValidRequest_ReturnsOkWithSuccessMessage()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<MergeGroupCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Unit.Value);

            var command = new MergeGroupCommand
            {
                GroupKey = "G1",
                ParentCustCode = "C1"
            };

            var result = await _controller.MergeGroup(command);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsTrue(okResult.Value is PagedCustomerResult res && res.BannerType == BannerMessageTypes.SUCCESS);
        }

        [TestMethod]
        public async Task UpdateCustomer_ValidRequest_ReturnsOkWithSuccessMessage()
        {
            _mockMediator.Setup(m => m.Send(It.IsAny<UpdateCustomerCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Unit.Value);

            var result = await _controller.UpdateCustomer(new CustomerMergeAPI.Domain.Models.Customer
            {
                CustCode = "C123",
                Name = "Updated Name"
            });

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsTrue(okResult.Value is PagedCustomerResult res && res.BannerType == BannerMessageTypes.SUCCESS);
        }
    }
}
