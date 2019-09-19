namespace TaskManager.Service.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Service.Business;
    using Service.Controllers;
    using Xunit;

    /// <summary>
    /// Test class for TaskController.
    /// </summary>
    public class TaskControllerTest : IDisposable
    {
        private const string Expected = "Task 1 Saved";

        public ILogger<TaskController> Logger { get; }

        public TaskControllerTest()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<TaskController>();
        }

        [Fact]
        public async Task VerifyGetAllTasks_Returns_TwoTaskDetails()
        {
            // Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);

            var taskDetailsList = new List<Models.TaskDetailModel>()
            {
                new Models.TaskDetailModel() {Id = 1, Name ="Task 1 ", Priority = 10},
                new Models.TaskDetailModel() {Id = 2, Name ="Task 2 ", Priority = 20},
            };

            mockManageTask.Setup(manage => manage.GetAllTasks()).Returns(Task.FromResult<IEnumerable<Models.TaskDetailModel>>(taskDetailsList));

            // Act
            var statusResult = await taskRepository.Get();

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            var taskDetailsResult = (statusResult as OkObjectResult).Value as List<Models.TaskDetailModel>;
            if (taskDetailsResult == null)
            {
                Assert.True(false, "Empty Result");
            }

            Assert.Equal(2, taskDetailsResult.Count);
        }

        [Fact]
        public async Task VerifyGetAllTasks_Throws_InternalServerErrorStatus_OnException()
        {
            // Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);

            mockManageTask.Setup(manage => manage.GetAllTasks()).Throws(new Exception());

            // Act
            var statusResult = await taskRepository.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
        }


        [Fact]
        public async Task VerifyGetTaskDetail_Return_OkStatusAndTaskDetails()
        {
            // Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);

            var taskDetail = new Models.TaskDetailModel() { Id = 1, Name = "Task 1", Priority = 10 };

            mockManageTask.Setup(manage => manage.GetTaskDetail(1)).Returns(Task.FromResult(taskDetail));

            // Act
            var statusResult = await taskRepository.Get(1);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            var taskDetailsResult = (statusResult as OkObjectResult).Value as Models.TaskDetailModel;
            Assert.Equal("Task 1", taskDetailsResult?.Name);
            Assert.Equal(10, taskDetailsResult?.Priority);
        }


        [Fact]
        public async Task VerifyGetTaskDetail_Throws_InternalServerErrorStatus_OnException()
        {
            //Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);

            mockManageTask.Setup(manage => manage.GetTaskDetail(1)).Throws(new Exception());

            //Act
            var statusResult = await taskRepository.Get(1);

            //Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task VerifyAddTaskDetails_Returns_OkStatusAndCheckTaskId()
        {
            // Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);

            var taskDetail = new Models.TaskDetailModel() { Id = 1001, Name = "Task 1", Priority = 10 };

            mockManageTask.Setup(manage => manage.AddTaskDetails(taskDetail)).Returns(Task.FromResult(1001));

            // Act
            var statusResult = await taskRepository.Post(taskDetail);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal("Task with id 1001 created successfully", (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Verify_Post_Returns_InternalServerErrorStatus_OnException()
        {
            // Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);
            var taskDetail = new Models.TaskDetailModel() { Id = 1001, Name = "Task 1", Priority = 10 };
            mockManageTask.Setup(manage => manage.AddTaskDetails(taskDetail)).Throws(new Exception());

            // Act
            var statusResult = await taskRepository.Post(taskDetail);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
        }

        [Fact]
        public async Task Verify_Put_Return_OkStatusAndCheckServiceResponse()
        {
            // Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);
            var taskDetail = new Models.TaskDetailModel() { Id = 1001, Name = "Task 1", Priority = 10 };

            mockManageTask.Setup(manage => manage.IsTaskValid(taskDetail)).Returns(true);
            mockManageTask.Setup(manage => manage.UpdateTaskDetails(1001, taskDetail)).Returns(Task.FromResult(1001));

            // Act
            var statusResult = await taskRepository.Put(1001, taskDetail);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal(Expected, (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Verify_Put_Returns_BadRequest_WhenTaskDetailIsInvalid()
        {
            // Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);
            var taskDetail = new Models.TaskDetailModel() { Id = 1001, Name = "Task 1", Priority = 10 };

            // Act
            var statusResult = await taskRepository.Put(1002, taskDetail);

            // Assert
            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("Invalid task detail", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task Verify_Put_Returns_BadRequestWhenTaskDetailIsNotValidToClose()
        {
            // Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);
            var taskDetail = new Models.TaskDetailModel() { Id = 1001, Name = "Task 1", Priority = 10, EndTask = true };
            mockManageTask.Setup(manage => manage.IsTaskValid(taskDetail)).Returns(false);

            // Act
            var statusResult = await taskRepository.Put(1001, taskDetail);

            // Assert
            Assert.NotNull(statusResult as BadRequestObjectResult);
            Assert.Equal("This task has active child tasks. Active child tasks has to be closed before closing parent task", (statusResult as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task Verify_Put_Returns_OkStatusWhenTaskDetailIsValidToClose()
        {
            // Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);

            var taskDetail = new Models.TaskDetailModel() { Id = 1001, Name = "Task 1", Priority = 10, EndTask = true };

            mockManageTask.Setup(manage => manage.IsTaskValid(taskDetail)).Returns(true);

            mockManageTask.Setup(manage => manage.UpdateTaskDetails(1001, taskDetail)).Returns(Task.FromResult(1001));

            // Act
            var statusResult = await taskRepository.Put(1001, taskDetail);

            // Assert
            Assert.NotNull(statusResult as OkObjectResult);

            Assert.Equal(Expected, (statusResult as OkObjectResult).Value);
        }

        [Fact]
        public async Task Verify_Put_Returns_ReturnInternalServerErrorStatus_OnException()
        {
            // Arrange
            var mockManageTask = new Mock<ITaskManager>();
            var taskRepository = new TaskController(mockManageTask.Object, Logger);
            var taskDetail = new Models.TaskDetailModel() { Id = 1001, Name = "Task 1", Priority = 10 };
            mockManageTask.Setup(manage => manage.IsTaskValid(taskDetail)).Returns(true);
            mockManageTask.Setup(manage => manage.UpdateTaskDetails(1001, taskDetail)).Throws(new Exception());

            // Act
            var statusResult = await taskRepository.Put(1001, taskDetail);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, (statusResult as ObjectResult)?.StatusCode);
        }

        #region IDisposable Support
        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            if (disposing)
            {
            }

            _disposedValue = true;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
