using EvApplicationApi.Controllers;
using EvApplicationApi.Models;
using EvApplicationApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using tests.TestTools.Doubles;

namespace tests.Controllers;

public class ApplicationItemsControllerTests
{
    [Fact]
    public void ApplicationItemPost_ReturnsApplicationAndAddsApplication_WhenApplicationIsValid()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationsRepository>();
        var controller = new ApplicationItemsController(mockRepo.Object);
        var newApplication = ApplicationDoubleFactory.CreateApplicationItem();

        // Act
        var result = controller.PostApplicationItem(newApplication);

        // Assert
        Assert.IsType<ActionResult<ApplicationItem>>(result);
        mockRepo.Verify();
    }

    [Fact]
    public void ApplicationItemGetById_ReturnsApplication_WhenApplicationIdIsValid()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationsRepository>();
        Guid testId = Guid.NewGuid();
        var testApplication = ApplicationDoubleFactory.CreateApplicationItem();
        mockRepo.Setup(repo => repo.GetApplicationItem(testId)).Returns(testApplication);
        var controller = new ApplicationItemsController(mockRepo.Object);

        // Act
        var result = controller.GetApplicationItem(testId).Value;

        // Assert
        Assert.Equal(testApplication, result);
        Assert.IsType<ApplicationItem>(result);
        mockRepo.Verify();
    }

    [Fact]
    public void ApplicationItemGetById_ReturnsNotFound_WhenApplicationIdIsInvalid()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationsRepository>();
        Guid testId = Guid.NewGuid();
        Guid differentTestId = Guid.NewGuid();
        var testApplication = ApplicationDoubleFactory.CreateApplicationItem();
        mockRepo.Setup(repo => repo.GetApplicationItem(testId)).Returns(testApplication);
        var controller = new ApplicationItemsController(mockRepo.Object);

        // Act
        var result = controller.GetApplicationItem(differentTestId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
        mockRepo.Verify();
    }
}
