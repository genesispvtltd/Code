
using Moq;
using CustomerMergeAPI.Domain.Queries;
using CustomerMergeAPI.Domain.Interfaces;
using CustomerMergeAPI.WebApi.Handlers;
using CustomerMergeAPI.Domain.DTOs;

namespace CustomerMergeAPI.Tests.Handlers
{
    [TestClass]
    public class GetResolvedQueryHandlerTests
    {
        private Mock<ICustomerManager> _mockManager;
        private GetResolvedQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {{
            _mockManager = new Mock<ICustomerManager>();
            _handler = new GetResolvedQueryHandler(_mockManager.Object);
        }}

        [TestMethod]
        public async Task Handle_ValidRequest_ReturnsPagedResult()
        {
            var request = new GetResolvedQuery(1, 5, "test");
            var expected = new PagedCustomerResult();
            var cancellationToken = CancellationToken.None;

            _mockManager.Setup(m => m.GetResolvedGroupsAsync(1, 5, "test", cancellationToken))
                        .ReturnsAsync(expected);

            var result = await _handler.Handle(request, cancellationToken);

            Assert.AreEqual(expected, result);
        }
    }
}
