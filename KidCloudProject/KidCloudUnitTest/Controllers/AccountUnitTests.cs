using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KidCloudProject.Controllers;
using KidCloudProject.Models;

namespace KidCloudUnitTest.Controllers
{
    [TestClass]
    public class AccountUnitTests
    {
        [TestMethod]
        public void Controller_Account_Login_Get()
        {
            // Arrange
            AccountController controller = new AccountController();

            // Act
            ViewResult result = controller.Login("") as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Controller_Account_Login_ReturnUrl()
        {
            // Arrange
            AccountController controller = new AccountController();
            string returnUrl = "wonderland";

            // Act
            ViewResult result = controller.Login(returnUrl) as ViewResult;

            // Assert
            Assert.AreEqual(returnUrl, result.ViewBag.ReturnUrl);
        }
        [TestMethod]
        public void Controller_Account_Login_Success()
        {
            // Arrange
            AccountController controller = new AccountController();
            LoginViewModel model = new LoginViewModel();
            string returnUrl = "wonderland";

            // Act
            var result = controller.Login(model, returnUrl);

            // Assert
            Assert.Fail();
        }
    }
}   
