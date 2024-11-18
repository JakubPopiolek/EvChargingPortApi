using EvApplicationApi.Controllers;
using EvApplicationApi.DTOs;
using EvApplicationApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace tests.Controllers
{
    public class ConfigurationFixture
    {
        public IConfiguration configuration;

        public ConfigurationFixture()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "PermittedFileUploadExtensions:0", ".jpg" },
                { "PermittedFileUploadExtensions:1", ".png" },
            };

            configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
        }
    }

    public class FileUploadControllerTests(ConfigurationFixture configurationFixture)
        : IClassFixture<ConfigurationFixture>
    {
        readonly ConfigurationFixture configurationFixture = configurationFixture;

        [Fact]
        public async void FileUploadGetUploadedFiles_ReturnsUploadedFiles_WhenApplicationIdIsValid()
        {
            //Arrange
            var mockRepo = new Mock<IFileUploadRepository>();

            Guid testId = Guid.NewGuid();
            List<UploadedFileDto> mockFiles =
            [
                new UploadedFileDto { Id = 1 },
                new UploadedFileDto { Id = 2 },
            ];
            mockRepo
                .Setup(repo => repo.GetUploadedFiles(testId))
                .Returns(Task.FromResult(mockFiles));

            var fileUploadController = new FileUploadController(
                mockRepo.Object,
                configurationFixture.configuration
            );

            var uploadedFiles = (OkObjectResult)
                (await fileUploadController.GetUploadedFiles(testId)).Result!;
            var uploadedFilesValue = uploadedFiles.Value;

            Assert.Equal(mockFiles, uploadedFilesValue);
            mockRepo.Verify();
        }

        [Fact]
        public async void FileUploadGetUploadedFiles_ReturnsBadRequest_WhenNoApplicationsFound()
        {
            var mockRepo = new Mock<IFileUploadRepository>();

            Guid testId = Guid.NewGuid();
            List<UploadedFileDto> mockFiles = [];
            mockRepo
                .Setup(repo => repo.GetUploadedFiles(testId))
                .Returns(Task.FromResult(mockFiles));

            var fileUploadController = new FileUploadController(
                mockRepo.Object,
                configurationFixture.configuration
            );

            var uploadedFilesError = (ObjectResult)
                (await fileUploadController.GetUploadedFiles(testId)).Result!;
            var uploadedFilesErrorMessage = uploadedFilesError.Value;

            Assert.Equal(
                $"No files found for application with id: {testId}",
                uploadedFilesErrorMessage
            );

            Assert.IsType<NotFoundObjectResult>(uploadedFilesError);
            mockRepo.Verify();
        }

        [Fact]
        public void FileUploadGetAllowedExtensions_ReturnsFileExtensionsFromConfig()
        {
            var mockRepo = new Mock<IFileUploadRepository>();

            var fileUploadController = new FileUploadController(
                mockRepo.Object,
                configurationFixture.configuration
            );

            var extensions = (ObjectResult)fileUploadController.GetAllowedExtensions().Result!;
            var extensionsValue = extensions.Value;
            var expectedExtensions = new List<string> { ".jpg", ".png" };

            Assert.Equal(extensionsValue, expectedExtensions);
        }

        [Fact]
        public async void FileUploadPost_ReturnsUploadedFiles_WhenFilesAreValid()
        {
            //Arrange
            var mockRepo = new Mock<IFileUploadRepository>();
            var fileUploadController = new FileUploadController(
                mockRepo.Object,
                configurationFixture.configuration
            );
            Guid testId = Guid.NewGuid();

            //Mock file setup

            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            FormFile mockFile = new FormFile(stream, 0, stream.Length, "file_id", fileName);

            List<IFormFile> mockFiles = [mockFile];

            //Act
            var uploadedFiles = (OkObjectResult)
                (await fileUploadController.Post(mockFiles, testId)).Result!;

            List<UploadedFileDto> uploadedFileDtos = (List<UploadedFileDto>)uploadedFiles.Value!;

            //Assert
            Assert.Equal(uploadedFileDtos[0].Name, mockFile.FileName);
        }

        [Fact]
        public async void FileUploadPost_ReturnsUnprocessableEntity_WhenFileTypeIsInvalid()
        {
            //Arrange
            var mockRepo = new Mock<IFileUploadRepository>();
            var fileUploadController = new FileUploadController(
                mockRepo.Object,
                configurationFixture.configuration
            );
            Guid testId = Guid.NewGuid();

            //Mock file setup

            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            FormFile mockFile = new FormFile(stream, 0, stream.Length, "file_id", fileName);

            List<IFormFile> mockFiles = [mockFile];

            //Act
            var uploadedFilesError = (ObjectResult)
                (await fileUploadController.Post(mockFiles, testId)).Result!;

            var uploadedFilesErrorMessage = uploadedFilesError.Value!;

            //Assert
            Assert.IsType<UnprocessableEntityObjectResult>(uploadedFilesError);
            Assert.Equal(
                "Invalid file type. File must be one of: .jpg .png.",
                uploadedFilesErrorMessage
            );
        }

        [Fact]
        public async void FileUploadPost_ReturnsUnprocessableEntity_WhenFileExtensionIsMissing()
        {
            //Arrange
            var mockRepo = new Mock<IFileUploadRepository>();
            var fileUploadController = new FileUploadController(
                mockRepo.Object,
                configurationFixture.configuration
            );
            Guid testId = Guid.NewGuid();

            //Mock file setup

            var content = "Hello World from a Fake File";
            var fileName = "test";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            FormFile mockFile = new FormFile(stream, 0, stream.Length, "file_id", fileName);

            List<IFormFile> mockFiles = [mockFile];

            //Act
            var uploadedFilesError = (ObjectResult)
                (await fileUploadController.Post(mockFiles, testId)).Result!;

            var uploadedFilesErrorMessage = uploadedFilesError.Value!;

            //Assert
            Assert.IsType<UnprocessableEntityObjectResult>(uploadedFilesError);
            Assert.Equal(
                "Invalid file type. File must be one of: .jpg .png.",
                uploadedFilesErrorMessage
            );
        }

        [Fact]
        public async void FileUploadPost_ReturnsBadReqest_WhenGuidIsEmpty()
        {
            //Arrange
            Guid testEmptyId = Guid.Empty;
            var mockRepo = new Mock<IFileUploadRepository>();

            var fileUploadController = new FileUploadController(
                mockRepo.Object,
                configurationFixture.configuration
            );

            //Act
            var error = (ObjectResult)
                (
                    await fileUploadController.Post(
                        [new FormFile(new MemoryStream(), 0, 0, "", "")],
                        testEmptyId
                    )
                ).Result!;

            var errorMessage = error.Value!;

            //Assert
            Assert.IsType<BadRequestObjectResult>(error);
            Assert.Equal("Missing Guid", errorMessage);
        }

        [Fact]
        public async void FileUploadPost_Returns500Error_WhenExtensionsInConfigAreMissing()
        {
            //Arrange
            var inMemorySettings = new Dictionary<string, string> { };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();
            var mockRepo = new Mock<IFileUploadRepository>();
            var fileUploadController = new FileUploadController(mockRepo.Object, configuration);
            Guid testId = Guid.NewGuid();

            //Act
            var error = (ObjectResult)
                (
                    await fileUploadController.Post(
                        [new FormFile(new MemoryStream(), 0, 0, "", "")],
                        testId
                    )
                ).Result!;

            // var errorMessage = error.Value!;
            //Assert
            Assert.Equal(500, error.StatusCode);
            Assert.Equal("Missing file configuration", error.Value);
            // Assert.Equal("Missing Guid", errorMessage);
        }

        [Fact]
        public async void FileUploadDelete_ReturnsDeletedFileId_WhenIdIsValid()
        {
            var mockRepo = new Mock<IFileUploadRepository>();
            var fileUploadController = new FileUploadController(
                mockRepo.Object,
                configurationFixture.configuration
            );
            long testId = 1;

            mockRepo.Setup(repo => repo.DeleteFileAsync(testId)).Returns(Task.FromResult(true));

            var deletedFile = (ObjectResult)await fileUploadController.DeleteFile(testId);
            var deletedFileValue = deletedFile.Value;

            Assert.Equal(testId, deletedFileValue);
        }

        [Fact]
        public async void FileUploadDelete_ReturnsNotFound_WhenIdIsInvalid()
        {
            var mockRepo = new Mock<IFileUploadRepository>();
            var fileUploadController = new FileUploadController(
                mockRepo.Object,
                configurationFixture.configuration
            );
            long testId = 1;

            mockRepo.Setup(repo => repo.DeleteFileAsync(testId)).Returns(Task.FromResult(true));

            var deletedFile = (ObjectResult)await fileUploadController.DeleteFile(2);
            var deletedFileValue = deletedFile.Value;

            Assert.IsType<NotFoundObjectResult>(deletedFile);
            Assert.Equal("Entity not found.", deletedFileValue);
        }
    }
}
