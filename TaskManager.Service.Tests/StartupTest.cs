namespace TaskManager.Service.Tests
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using TaskManager.Service.Business;
    using Xunit;

    public class StartupTest
    {
        [Fact]
        public void TestBuild()
        {
            var configuration = new Mock<IConfiguration>();
            var serviceCollection = new ServiceCollection();
            configuration.Setup(config => config.GetSection("Database").GetSection("Connection").Value).Returns("DummyConnection");
            var startUp = new Startup(configuration.Object);

            startUp.ConfigureServices(serviceCollection);

            var sp = serviceCollection.BuildServiceProvider();
            var result = sp.GetService<ITaskManager>();
            Assert.NotNull(result);
        }
    }
}
