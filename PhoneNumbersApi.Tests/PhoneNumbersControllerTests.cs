using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using PhoneNumberApi.Controllers;
using PhoneNumberApi.Interfaces;
using PhoneNumberApi.Models;

namespace PhoneNumbersApi.Tests
{
    [TestFixture]
    public class PhoneNumbersControllerTests
    {
        private Mock<IPhoneNumberService> _mockPhoneNumberService;
        private PhoneNumbersController _phoneNumbersController;

        [SetUp]
        public void Setup()
        {
            _mockPhoneNumberService = new Mock<IPhoneNumberService>();
            _phoneNumbersController = new PhoneNumbersController(_mockPhoneNumberService.Object);
        }

        [Test]
        public void AssignPhoneNumber_ValidRequest_ReturnsCreatedAtAction()
        {
            //Arrange
            var phoneNumber = new PhoneNumber { Number = "1234567890" };
            _mockPhoneNumberService.Setup(x => x.AssignPhoneNumber(It.IsAny<int>(), phoneNumber)).Returns(phoneNumber);

            //Act
            var result = _phoneNumbersController.AssignPhoneNumber(1, phoneNumber);

            //Assert
            Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
            CreatedAtActionResult? createdAtActionResult = (CreatedAtActionResult)result.Result;
            Assert.IsNotNull(createdAtActionResult);
            Assert.That(createdAtActionResult.ActionName, Is.EqualTo("GetPhoneNumber"));
            Assert.That(createdAtActionResult.Value, Is.EqualTo(phoneNumber));
        }

        [Test]
        public void AssignPhoneNumber_AccountNotFound_ReturnsNotFound()
        {
            //Arrange
            var phoneNumber = new PhoneNumber { Number = "1234567890" };
            _mockPhoneNumberService.Setup(x => x.AssignPhoneNumber(It.IsAny<int>(), phoneNumber)).Throws<ArgumentException>();

            //Act
            var result = _phoneNumbersController.AssignPhoneNumber(1, phoneNumber);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }

        [Test]
        public void AssignPhoneNumber_PhoneNumberAlreadyAssigned_ReturnsConflict()
        {
            //Arrange
            var phoneNumber = new PhoneNumber { Number = "1234567890" };
            _mockPhoneNumberService.Setup(x => x.AssignPhoneNumber(It.IsAny<int>(), phoneNumber)).Throws<InvalidOperationException>();

            //Act
            var result = _phoneNumbersController.AssignPhoneNumber(1, phoneNumber);

            //Assert
            Assert.IsInstanceOf<ConflictObjectResult>(result.Result);
        }

        [Test]
        public void GetPhoneNumber_ExistingId_ReturnsOk()
        {
            //Arrange
            var phoneNumber = new PhoneNumber { Id = 1, Number = "1234567890" };
            _mockPhoneNumberService.Setup(x => x.GetPhoneNumber(1)).Returns(phoneNumber);

            //Act
            var result = _phoneNumbersController.GetPhoneNumber(1);

            //Assert
            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okObjectResult = (OkObjectResult)result.Result;
            Assert.IsNotNull(okObjectResult);
            Assert.That(okObjectResult.Value, Is.EqualTo(phoneNumber));
        }

        [Test]
        public void GetPhoneNumber_NonExistentId_ReturnsNotFound()
        {
            //Arrange
            _mockPhoneNumberService.Setup(x => x.GetPhoneNumber(It.IsAny<int>())).Throws<KeyNotFoundException>();

            //Act
            var result = _phoneNumbersController.GetPhoneNumber(1);

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void AssignPhoneNumberToAccount_ValidRequest_ReturnsNoContent()
        {
            //Arrange
            var phoneNumberId = 1;
            var accountId = 1;
            _mockPhoneNumberService.Setup(x => x.AssignPhoneNumberToAccount(phoneNumberId, accountId));

            //Act
            var result = _phoneNumbersController.AssignPhoneNumberToAccount(phoneNumberId, accountId);

            //Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void AssignPhoneNumberToAccount_PhoneNumberNotFound_ReturnsNotFound()
        {
            //Arrange
            var phoneNumberId = 1;
            var accountId = 1;
            _mockPhoneNumberService.Setup(x => x.AssignPhoneNumberToAccount(phoneNumberId, accountId)).Throws<KeyNotFoundException>();

            //Act
            var result = _phoneNumbersController.AssignPhoneNumberToAccount(phoneNumberId, accountId);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public void GetPhoneNumbersForAccount_ExistingAccountId_ReturnsOk()
        {
            //Arrange
            var accountId = 1;
            var phoneNumbers = new List<PhoneNumber>
            {
                new PhoneNumber { Id = 1, Number = "1234567890", AccountId = accountId },
                new PhoneNumber { Id = 2, Number = "9876543210", AccountId = accountId }
            };
            _mockPhoneNumberService.Setup(x => x.GetPhoneNumbersForAccount(accountId)).Returns(phoneNumbers);

            //Act
            var result = _phoneNumbersController.GetPhoneNumbersForAccount(accountId);

            //Assert
            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okObjectResult = (OkObjectResult)result.Result;
            Assert.IsNotNull(okObjectResult);
            Assert.That(okObjectResult.Value, Is.EqualTo(phoneNumbers));
        }

        [Test]
        public void GetPhoneNumbersForAccount_NonExistentAccountId_ReturnsNotFound()
        {
            //Arrange
            var accountId = 1;
            _mockPhoneNumberService.Setup(x => x.GetPhoneNumbersForAccount(accountId)).Throws<KeyNotFoundException>();

            //Act
            var result = _phoneNumbersController.GetPhoneNumbersForAccount(accountId);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }
    }
}