using KafkaClient.Core;
using KafkaClient.Core.Services;
using KafkaClient.Test.Common;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MyKafkaClient.Core.Services;

using NUnit.Framework;

namespace KafkaClient.Test.IntegratedTest.Services;

[TestFixture]
public abstract class MyServiceAbstractForTest
{
  [OneTimeSetUp]
  public void OneTimeSetUp()
  {
    IConfigurationRoot? configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.test.json", false)
      .AddEnvironmentVariables()
      .Build();

    MyClientConfiguration? options =
      configuration.GetSection("MyClientConfiguration").Get<MyClientConfiguration>();

    ServiceProvider serviceProvider = new ServiceCollection()
      .AddLogging()
      .AddConsumer<MyEntityCreated, MyEntityCreatedHandlerForTest>("my-entity-created", options)
      .AddProducer<MyEntityCreated>("my-entity-created", options)
      .BuildServiceProvider();

    _someProducer = serviceProvider.GetRequiredService<IMyProducerService<MyEntityCreated>>();
    _someConsumer = serviceProvider.GetRequiredService<IMyConsumerService<MyEntityCreated>>();
    _someBackgroundService = serviceProvider.GetServices<IHostedService>()
      .OfType<MyConsumerBackgroundService<MyEntityCreated>>().First();
  }

  [OneTimeTearDown]
  public void OneTimeTearDown()
  {
    _someConsumer.Dispose();
    _someProducer.Dispose();
    _someBackgroundService.Dispose();
  }

  protected IMyProducerService<MyEntityCreated> _someProducer;
  protected IMyConsumerService<MyEntityCreated> _someConsumer;
  protected MyConsumerBackgroundService<MyEntityCreated> _someBackgroundService;

  protected static MyEntityCreated MyEntityCreated => new() { MyEntity = new MyEntity { Id = "B39369EC-B252-43B1-A8B0-760978728348" } };
}
