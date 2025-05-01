using Xunit;
using Moq;
using GicBankApp.Domain.Factories;
using GicBankApp.Domain.ValueObjects;
using GicBankApp.Domain.Entities;
using GicBankApp.Domain.Common;

namespace GicBankApp.Tests.Domain.Factories
{
    public class TransactionFactoryTests
    {
        private readonly BusinessDate _date = BusinessDate.From("20230626");
        private readonly Money _amount = new Money(100.00m);

        [Fact]
        public void CreateTransaction_ShouldReturnDepositTransaction_WhenTypeIsD()
        {
            var mockIdGenerator = new Mock<ITransactionIdGenerator>();
            var expectedId = new TransactionId(_date, 1);
            mockIdGenerator.Setup(x => x.GenerateId(_date)).Returns(expectedId);

            var factory = new TransactionFactory(mockIdGenerator.Object);

            var transaction = factory.CreateTransaction(_date, _amount, "D");

            Assert.IsType<DepositTransaction>(transaction);
            Assert.Equal(expectedId.Value, transaction.TransactionId.Value);
            Assert.Equal(_amount, transaction.Amount);
        }

        [Fact]
        public void CreateTransaction_ShouldReturnWithdrawalTransaction_WhenTypeIsW()
        {
            var mockIdGenerator = new Mock<ITransactionIdGenerator>();
            var expectedId = new TransactionId(_date, 2);
            mockIdGenerator.Setup(x => x.GenerateId(_date)).Returns(expectedId);

            var factory = new TransactionFactory(mockIdGenerator.Object);

            var transaction = factory.CreateTransaction(_date, _amount, "W");

            Assert.IsType<WithdrawalTransaction>(transaction);
            Assert.Equal(expectedId.Value, transaction.TransactionId.Value);
            Assert.Equal(_amount, transaction.Amount);
        }

        [Theory]
        [InlineData("X")]
        [InlineData("")]
        public void CreateTransaction_ShouldThrow_WhenTypeIsInvalid(string invalidType)
        {
            var mockIdGenerator = new Mock<ITransactionIdGenerator>();
            mockIdGenerator.Setup(x => x.GenerateId(_date)).Returns(new TransactionId(_date, 1));

            var factory = new TransactionFactory(mockIdGenerator.Object);

            Assert.Throws<ArgumentException>(() => factory.CreateTransaction(_date, _amount, invalidType));
        }
    }
}
