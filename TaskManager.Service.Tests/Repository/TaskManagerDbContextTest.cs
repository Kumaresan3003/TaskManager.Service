namespace TaskManager.Service.Tests.Repository
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Moq;
    using System;
    using Service.Repository;
    using Xunit;

    public class TaskManagerDbContextTest : IDisposable
    {

        [Fact]
        public void OnModelCreating_VerifyModelCreation()
        {
            // Arrange
            var mockModel = new Mock<ModelBuilder>(new ConventionSet());
            try
            {
                var contextOptions = new DbContextOptions<TaskManagerDbContext>();
                var taskDbContextStub = new TaskDbContextStub(contextOptions);

                var modelBuilder = new ModelBuilder(new ConventionSet());
                var model = new Model();
                var entity = new EntityType("TaskDetailModel", model, new ConfigurationSource());
                var internalModelBuilder = new InternalModelBuilder(model);
                var internalEntityTypeBuilder = new InternalEntityTypeBuilder(entity, internalModelBuilder);
                var entityTypeBuilder = new EntityTypeBuilder<Models.TaskDetailModel>(internalEntityTypeBuilder);
                mockModel.Setup(m => m.Entity<Models.TaskDetailModel>()).Returns(entityTypeBuilder);

                // Act
                taskDbContextStub.TestModelCreation(modelBuilder);
            }
            catch (Exception ex)
            {
                // Assert
                mockModel.Verify(m => m.Entity<Models.TaskDetailModel>().HasKey("Id"), Times.Once);
                Assert.NotNull(ex);
            }
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

    public class TaskDbContextStub : TaskManagerDbContext
    {
        public TaskDbContextStub(DbContextOptions options) : base(options)
        {

        }
        public void TestModelCreation(ModelBuilder model)
        {
            OnModelCreating(model);
        }
    }
}
