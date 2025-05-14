using Moq;
using CustomerMergeAPI.Domain.Commands;
using CustomerMergeAPI.Domain.Interfaces;
using CustomerMergeAPI.WebApi.Handlers;
using MediatR;

namespace CustomerMergeAPI.Tests.Handlers
{
    [TestClass]
    public class MergeGroupHandlerTests
    {
        private Mock<ICustomerManager> _mockManager;
        private MergeGroupHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockManager = new Mock<ICustomerManager>();
            _handler = new MergeGroupHandler(_mockManager.Object);
        }

        [TestMethod]
        public async Task Handle_ValidRequest_CallsMergeGroupAsync()
        {
            // Arrange
            var request = new MergeGroupCommand
            {
                GroupKey = "G001",
                ParentCustCode = "C100",
                MergedBy = "tester"
            };
            var cancellationToken = CancellationToken.None;

            _mockManager.Setup(m => m.MergeGroupAsync(request.GroupKey, request.ParentCustCode, request.MergedBy, cancellationToken))
                        .Returns(Task.CompletedTask)
                        .Verifiable();

            // Act
            var result = await _handler.Handle(request, cancellationToken);

            // Assert
            Assert.AreEqual(Unit.Value, result);
            _mockManager.Verify();
        }
    }
}
