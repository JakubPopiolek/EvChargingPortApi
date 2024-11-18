using EvApplicationApi.Controllers;
using EvApplicationApi.DTOs;
using EvApplicationApi.Models;
using EvApplicationApi.Repositories.Interfaces;
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
        var mockRepo = new Mock<IApplicationRepository>();
        var controller = new ApplicationItemsController(mockRepo.Object);
        var newApplication = ApplicationDoubleFactory.CreateApplicationItem();

        // Act
        var result = controller.SubmitApplication(newApplication!);

        // Assert
        Assert.IsType<Task<ActionResult<ApplicationItem>>>(result);
        mockRepo.Verify();
    }

    [Fact]
    public void ApplicationItemGetById_ReturnsApplication_WhenApplicationIdIsValid()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationRepository>();
        Guid testId = Guid.NewGuid();
        var testApplication = ApplicationDoubleFactory.CreateApplicationItem();
        var testApplicationDto = ApplicationDoubleFactory.CreateApplicationItemDto();

        mockRepo
            .Setup(repo => repo.GetApplicationItemDto(testId))
            .Returns(Task.FromResult(testApplicationDto));
        var controller = new ApplicationItemsController(mockRepo.Object);

        // Act
        var result = (OkObjectResult)controller.GetApplicationItem(testId).Result!;
        var resultValue = result.Value;

        // Assert
        Assert.Equal(testApplicationDto, resultValue);
        Assert.IsType<ApplicationItemDto>(resultValue);
        mockRepo.Verify();
    }

    [Fact]
    public void ApplicationItemGetById_ReturnsNotFound_WhenApplicationIdIsInvalid()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationRepository>();
        Guid testId = Guid.NewGuid();
        Guid differentTestId = Guid.NewGuid();
        var testApplication = ApplicationDoubleFactory.CreateApplicationItem();
        mockRepo
            .Setup(repo => repo.GetApplicationItem(testId))
            .Returns(Task.FromResult(testApplication));
        var controller = new ApplicationItemsController(mockRepo.Object);

        // Act
        var result = controller.GetApplicationItem(differentTestId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
        mockRepo.Verify();
    }
}
