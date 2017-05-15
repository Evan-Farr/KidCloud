using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KidCloudProject.Controllers;
using System.Web.Mvc;

namespace KidCloudUnitTest.Controllers
{
    [TestClass]
    public class DayCaresUnitTests
    {
        [TestMethod]
        public void Controller_DayCares_Index_Get()
        {
            // Arrange
            DayCaresController controller = new DayCaresController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void Controller_DayCares_IndexParents_Get()
        {
            // Arrange
            DayCaresController controller = new DayCaresController();
            int id = 1;

            // Act
            ViewResult result = controller.IndexParents(id) as ViewResult;
            var test = result.Model;
            // Assert
            Assert.IsNotNull(result);
        }
    }
}
