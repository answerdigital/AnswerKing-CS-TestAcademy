namespace Answer.King.IntegrationTests.Utilities;
using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

internal class BaseTestClass : IDisposable
{
    private WebApplicationFactory<Program> factory;
    private HttpClient client;

    [OneTimeSetUp]
    public void Setup()
    {
        File.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, "db/Answer.King.db"));
        File.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, "db/Answer.King-log.db"));
        this.factory = new WebApplicationFactory<Program>();
        this.client = this.factory.CreateClient();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        this.client.Dispose();
        this.factory.Dispose();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
