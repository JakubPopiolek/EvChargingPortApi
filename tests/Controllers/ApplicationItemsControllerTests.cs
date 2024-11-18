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
    public async void ApplicationItemSubmitApplication_ReturnsApplicationAndAddsApplication_WhenApplicationIsValid()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationRepository>();
        var controller = new ApplicationItemsController(mockRepo.Object);
        Guid testId = Guid.NewGuid();
        ApplicationItem testApplication = new ApplicationItem() { ReferenceNumber = testId };
        ApplicationItem newApplication = ApplicationDoubleFactory.CreateApplicationItem();
        newApplication.ReferenceNumber = testId;

        mockRepo
            .Setup(repo => repo.GetApplicationItem(testId))
            .Returns(Task.FromResult(testApplication)!);

        // Act
        var result = (OkObjectResult)(await controller.SubmitApplication(newApplication)).Result!;
        var resultValue = result.Value;

        // Assert
        Assert.Equal(resultValue, newApplication);
        mockRepo.Verify();
    }

    [Fact]
    public async void ApplicationItemSubmitApplication_ReturnsBadRequest_WhenApplicationIdIsEmpty()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationRepository>();
        var controller = new ApplicationItemsController(mockRepo.Object);
        ApplicationItem newApplication = ApplicationDoubleFactory.CreateApplicationItem();

        // Act
        var result = (ObjectResult)(await controller.SubmitApplication(newApplication)).Result!;
        var resultValue = result.Value;

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(resultValue, "Missing Guid");
        mockRepo.Verify();
    }

    [Fact]
    public async void ApplicationItemSubmitApplication_ReturnsNotFound_WhenApplicationIdIsNotInDatabase()
    {
        // Arrange
        var mockRepo = new Mock<IApplicationRepository>();
        var controller = new ApplicationItemsController(mockRepo.Object);
        Guid testId = Guid.NewGuid();
        ApplicationItem newApplication = ApplicationDoubleFactory.CreateApplicationItem();
        newApplication.ReferenceNumber = Guid.Empty;
        newApplication.ReferenceNumber = testId;

        // Act
        var result = (ObjectResult)(await controller.SubmitApplication(newApplication)).Result!;
        var resultValue = result.Value;

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(resultValue, $"Could not find application with GUID: {testId}");
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
            .Returns(Task.FromResult(testApplicationDto)!);
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
            .Returns(Task.FromResult(testApplication)!);
        var controller = new ApplicationItemsController(mockRepo.Object);

        // Act
        var result = controller.GetApplicationItem(differentTestId);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<NotFoundResult>(result.Result);
        mockRepo.Verify();
    }

    [Fact]
    public void ApplicationItemStartApplication_ReturnsCreatedApplicationId()
    {
        //Arrange
        var mockRepo = new Mock<IApplicationRepository>();
        Guid testId = Guid.NewGuid();
        mockRepo.Setup(repo => repo.StartApplication()).Returns(testId);

        var controller = new ApplicationItemsController(mockRepo.Object);

        //Act
        var result = (OkObjectResult)controller.StartApplication().Result!;
        var resultValue = result.Value;

        Assert.NotNull(resultValue);
        Assert.Equal(resultValue, testId);
        mockRepo.Verify();
    }
}
