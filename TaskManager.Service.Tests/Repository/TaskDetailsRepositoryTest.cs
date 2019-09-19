 namespace TaskManager.Service.Tests.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;
    using TaskManager.Service.Repository;
    using Helper;

    /// <summary>
    /// Test class for TaskDetailsRepository
    /// </summary>
    public class TaskDetailsRepositoryTest : IDisposable
    {
        [Fact]
        public async Task Verify_GetAllTasks_Returns_TwoTaskDetails()
        {
            // Arrange
            var contextOptions = new DbContextOptions<TaskManagerDbContext>();
            var mockContext = new Mock<TaskManagerDbContext>(contextOptions);

            var taskRepository = new TaskDetailsRepository(mockContext.Object);

            IQueryable<Models.TaskDetailModel> taskDetailsList = new List<Models.TaskDetailModel>()
            {
                new Models.TaskDetailModel() {Id = 1, Name ="Task 1 ", Priority = 10},
                new Models.TaskDetailModel() {Id = 2, Name ="Task 2 ", Priority = 20},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Models.TaskDetailModel>>();

            mockSet.As<IAsyncEnumerable<Models.TaskDetailModel>>()
        .Setup(m => m.GetEnumerator())
        .Returns(new TestAsyncEnumerator<Models.TaskDetailModel>(taskDetailsList.GetEnumerator()));

            mockSet.As<IQueryable<Models.TaskDetailModel>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Models.TaskDetailModel>(taskDetailsList.Provider));

            mockSet.As<IQueryable<Models.TaskDetailModel>>().Setup(m => m.Expression).Returns(taskDetailsList.Expression);
            mockSet.As<IQueryable<Models.TaskDetailModel>>().Setup(m => m.ElementType).Returns(taskDetailsList.ElementType);
            mockSet.As<IQueryable<Models.TaskDetailModel>>().Setup(m => m.GetEnumerator()).Returns(() => taskDetailsList.GetEnumerator());

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            // Act
            var taskDetails = await taskRepository.GetAllTasks();

            // Assert
            Assert.Equal(2, taskDetails.Count());
        }

        [Fact]
        public async Task VerifyTaskName()
        {
            // Arrange
            var contextOptions = new DbContextOptions<TaskManagerDbContext>();
            var mockContext = new Mock<TaskManagerDbContext>(contextOptions);

            var taskRepository = new TaskDetailsRepository(mockContext.Object);

            IQueryable<Models.TaskDetailModel> taskDetailsList = new List<Models.TaskDetailModel>()
            {
                new Models.TaskDetailModel() {Id = 1, Name ="Task 1", Priority = 10},
                new Models.TaskDetailModel() {Id = 2, Name ="Task 2", Priority = 20},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Models.TaskDetailModel>>();

            mockSet.As<IAsyncEnumerable<Models.TaskDetailModel>>()
        .Setup(m => m.GetEnumerator())
        .Returns(new TestAsyncEnumerator<Models.TaskDetailModel>(taskDetailsList.GetEnumerator()));

            mockSet.As<IQueryable<Models.TaskDetailModel>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Models.TaskDetailModel>(taskDetailsList.Provider));

            mockSet.As<IQueryable<Models.TaskDetailModel>>().Setup(m => m.Expression).Returns(taskDetailsList.Expression);
            mockSet.As<IQueryable<Models.TaskDetailModel>>().Setup(m => m.ElementType).Returns(taskDetailsList.ElementType);
            mockSet.As<IQueryable<Models.TaskDetailModel>>().Setup(m => m.GetEnumerator()).Returns(() => taskDetailsList.GetEnumerator());

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            // Act
            var taskDetails = await taskRepository.Get(2);

            // Assert
            Assert.Equal("Task 2", taskDetails.Name);
        }

        [Fact]
        public async Task VerifySaveChangesCalledOnce()
        {
            // Arrange
            var contextOptions = new DbContextOptions<TaskManagerDbContext>();
            var mockContext = new Mock<TaskManagerDbContext>(contextOptions);

            var taskRepository = new TaskDetailsRepository(mockContext.Object);

            var taskDetail = new Models.TaskDetailModel() { Id = 1, Name = "Task 1 ", Priority = 10 };

            var mockSet = new Mock<DbSet<Models.TaskDetailModel>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            // Act
            await taskRepository.Insert(taskDetail);

            // Assert
            mockSet.Verify(m => m.Add(taskDetail), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task VerifyUpdateSaveChangesCalledOnce()
        {
            // Arrange
            var contextOptions = new DbContextOptions<TaskManagerDbContext>();
            var mockContext = new Mock<TaskManagerDbContext>(contextOptions);

            var taskRepository = new TaskDetailsRepository(mockContext.Object);

            var taskDetail = new Models.TaskDetailModel() { Id = 1, Name = "Task 1 ", Priority = 10 };

            var mockSet = new Mock<DbSet<Models.TaskDetailModel>>();

            mockContext.Setup(m => m.Tasks).Returns(mockSet.Object);

            // Act
            await taskRepository.Update(1, taskDetail);

            // Assert
            mockSet.Verify(m => m.Update(taskDetail), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(System.Threading.CancellationToken.None), Times.Once);
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
