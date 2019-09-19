namespace TaskManager.Service.Tests.Business
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Service.Business;
    using Service.Repository;
    using Xunit;

    /// <summary>
    /// Test class for TaskManager
    /// </summary>
    public class TaskManagerTest : IDisposable
    {
        public ILogger<TaskManager> Logger { get; }

        /// <summary>
        /// Constructor for TaskManagerTest
        /// </summary>
        public TaskManagerTest()
        {
            ServiceProvider serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            ILoggerFactory factory = serviceProvider.GetService<ILoggerFactory>();
            Logger = factory.CreateLogger<TaskManager>();
        }

        [Fact]
        public async Task VerifyInsertCalledOnce()
        {
            // Arrange
            var mockRepository = new Mock<ITaskDetailsRepository>();
            var taskManager = new TaskManager(mockRepository.Object);
            var taskDetail = new Models.TaskDetailModel();

            // Act
            await taskManager.AddTaskDetails(taskDetail);

            // Assert
            mockRepository.Verify(t => t.Insert(taskDetail), Times.Once);
        }

        [Fact]
        public async Task VerifyUpdateCalledOnce()
        {
            // Arrange
            var mockRepository = new Mock<ITaskDetailsRepository>();
            var taskManager = new TaskManager(mockRepository.Object);
            var taskDetail = new Models.TaskDetailModel();

            // Act
            await taskManager.UpdateTaskDetails(10, taskDetail);

            // Assert
            mockRepository.Verify(t =>t.Update(10, taskDetail), Times.Once);
        }

        [Fact]
        public async Task VerifyGetAllTasksCalledOnce()
        {
            // Arrange
            Mock<ITaskDetailsRepository> mockRepository = new Mock<ITaskDetailsRepository>();
            TaskManager taskManager = new TaskManager(mockRepository.Object);

            // Act
            await taskManager.GetAllTasks();

            // Assert
            mockRepository.Verify(t => t.GetAllTasks(), Times.Once);
        }

        [Fact]
        public async Task VerifyGetCalledOnce()
        {
            // Arrange
            var mockRepository = new Mock<ITaskDetailsRepository>();
            var taskManager = new TaskManager(mockRepository.Object);

            // Act
            await taskManager.GetTaskDetail(10);

            // Assert
            mockRepository.Verify(t => t.Get(10), Times.Once);
        }

        [Fact]
        public void Verify_IsTaskValid_ReturnFalseWhenTaskContainsChildTask()
        {
            // Arrange
            var mockRepository = new Mock<ITaskDetailsRepository>();
            var taskManager = new TaskManager(mockRepository.Object);
            var taskDetail = new Models.TaskDetailModel() { Id = 1, Name = "Task 1", Priority = 20 };

            var taskDetailsList = new List<Models.TaskDetailModel>()
            {
                taskDetail,
                new Models.TaskDetailModel() {Id = 2, Name ="Task 2 ", Priority = 20, ParentTaskId = 1},
            };

            mockRepository.Setup(r => r.GetAllTasks()).Returns(Task.FromResult<IEnumerable<Models.TaskDetailModel>>(taskDetailsList));

            // Act
            var result = taskManager.IsTaskValid(taskDetail);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Verify_IsTaskValid_ReturnTrueWhenTaskContainsChildTask()
        {
            // Arrange
            var mockRepository = new Mock<ITaskDetailsRepository>();
            var manageTask = new TaskManager(mockRepository.Object);
            var taskDetail = new Models.TaskDetailModel() { Id = 1, Name = "Task 1", Priority = 20 };

            var taskDetailsList = new List<Models.TaskDetailModel>()
            {
                taskDetail,
                new Models.TaskDetailModel() {Id = 2, Name ="Task 2 ", Priority = 20, ParentTaskId = 1, EndTask = true},
            };

            mockRepository.Setup(r => r.GetAllTasks()).Returns(Task.FromResult<IEnumerable<Models.TaskDetailModel>>(taskDetailsList));

            // Act
            var result = manageTask.IsTaskValid(taskDetail);

            // Assert
            Assert.True(result);
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
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
