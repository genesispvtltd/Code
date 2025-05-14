using Moq;
using CustomerMergeAPI.Domain.Interfaces;
using CustomerMergeAPI.WebApi.Handlers;
using MediatR;
using CustomerMergeAPI.Domain.Models;

namespace CustomerMergeAPI.Tests.Handlers
{
    [TestClass]
    public class UpdateCustomerCommandHandlerTests
    {
        private Mock<ICustomerManager> _mockManager;
        private UpdateCustomerCommandHandler _handler;

        [TestInitialize]
        public void Setup()
        {
            _mockManager = new Mock<ICustomerManager>();
            _handler = new UpdateCustomerCommandHandler(_mockManager.Object);
        }

        [TestMethod]
        public async Task Handle_ValidCustomer_CallsUpdateCustomerAsync()
        {
           
            var customer = new Customer { CustCode = "C001", Name = "John" };
            var request = new UpdateCustomerCommand { Customer = customer };
            var cancellationToken = CancellationToken.None;

            _mockManager.Setup(m => m.UpdateCustomerAsync(customer,"John", cancellationToken)).Returns(Task.CompletedTask).Verifiable();

            var result = await _handler.Handle(request, cancellationToken);

            Assert.AreEqual(Unit.Value, result);
            _mockManager.Verify();
        }
    }
}
