using Moq;
using CustomerMergeAPI.Domain.Queries;
using CustomerMergeAPI.Domain.Interfaces;
using CustomerMergeAPI.WebApi.Handlers;
using CustomerMergeAPI.Domain.Models;


namespace CustomerMergeAPI.Tests.Handlers
{
    [TestClass]
    public class GetDuplicatesQueryHandlerTests
    {
        private Mock<ICustomerManager> _mockManager;
        private GetDuplicatesQueryHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockManager = new Mock<ICustomerManager>();
            _handler = new GetDuplicatesQueryHandler(_mockManager.Object);
        }
[TestMethod]
public async Task Handle_ValidRequest_ReturnsPagedResult()
{
    // Arrange
    var request = new GetDuplicatesQuery { Page = 1, PageSize = 5, Search = "test" };
    var cancellationToken = CancellationToken.None;

    var customerList = new List<Customer>
    {
        new Customer { CustCode = "C001", Name = "Test User", GroupKey = "G1" }
    };

    _mockManager.Setup(m => m.GetDuplicateGroupsAsync(request.Page, request.PageSize, request.Search, cancellationToken))
                .ReturnsAsync(customerList);

    _mockManager.Setup(m => m.GetDuplicateGroupsCountAsync(request.Search, cancellationToken))
                .ReturnsAsync(customerList.Count);

    // Act
    var result = await _handler.Handle(request, cancellationToken);

    // Assert
    Assert.IsNotNull(result);
    Assert.AreEqual(1, result.TotalPages);
    Assert.AreEqual(1, result.Data.Count);
    Assert.AreEqual("C001", result.Data[0].CustCode);
}

        
    }
}
