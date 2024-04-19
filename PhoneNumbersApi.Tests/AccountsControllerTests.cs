using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PhoneNumberApi.Controllers;
using PhoneNumberApi.Interfaces;
using PhoneNumberApi.Models;

namespace PhoneNumbersApi.Tests
{
    [TestFixture]
    public class AccountsControllerTests
    {
        private Mock<IAccountService> _mockAccountService;
        private AccountsController _accountsController;

        [SetUp]
        public void Setup()
        {
            _mockAccountService = new Mock<IAccountService>();
            _accountsController = new AccountsController(_mockAccountService.Object);
        }

        [Test]
        public void Get_ReturnsListOfAccounts()
        {
            //Arrange
            var accounts = new List<Account>
            {
                new Account { Id = 1, Name = "Test 1" },
                new Account { Id = 2, Name = "Test 2" }
            };
            _mockAccountService.Setup(x => x.GetAccounts()).Returns(accounts);

            //Act
            var result = _accountsController.Get();

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okObjectResult = (OkObjectResult)result.Result;
            Assert.That(okObjectResult.Value, Is.EqualTo(accounts));
        }

        [Test]
        public void CreateAccount_ValidAccount_ReturnsCreatedAtAction()
        {
            //Arrange
            var account = new Account { Id = 1, Name = "Test 1" }; 
           
            _mockAccountService.Setup(x => x.CreateAccount(account)).Returns(account);

            //Act
            var result = _accountsController.CreateAccount(account);

            //Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = (CreatedAtActionResult)result.Result;
            Assert.That(createdAtActionResult.ActionName, Is.EqualTo("GetAccount"));
            Assert.That(createdAtActionResult.Value, Is.EqualTo(account));
        }

        [Test]
        public void ToggleAccountStatus_ValidId_ReturnsNoContent()
        {
            //Arrange
            var accountId = 1;
            _mockAccountService.Setup(x => x.ToggleAccountStatus(accountId));

            //Act
            var result = _accountsController.ToggleAccountStatus(accountId);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void ToggleAccountStatus_NonExistentId_ReturnsNotFound()
        {
            //Arrange
            var accountId = 1;
            _mockAccountService.Setup(x => x.ToggleAccountStatus(accountId)).Throws<KeyNotFoundException>(); 

            //Act
            var result = _accountsController.ToggleAccountStatus(accountId);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void GetAccount_ExistingId_ReturnsOk()
        {
            //Arrange
            var account = new Account { Id = 1, Name = "Account 1" };
            _mockAccountService.Setup(x => x.GetAccount(1)).Returns(account);

            //Act
            var result = _accountsController.GetAccount(1);

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okObjResult = (OkObjectResult)result.Result;
            Assert.That(okObjResult.Value, Is.EqualTo(account));
        }

        [Test]
        public void GetAccount_NonExistentId_ReturnsNotFound()
        {
            //Arrange
            _mockAccountService.Setup(x => x.GetAccount(It.IsAny<int>())).Throws<KeyNotFoundException>();

            //Act
            var result = _accountsController.GetAccount(1);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }
    }
}
