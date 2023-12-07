namespace Answer.King.IntegrationTests.Utilities;
using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;

public class BaseTestClass : IDisposable
{
    public HttpClient Client;

    [OneTimeSetUp]
    public void Setup()
    {
        File.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, "db/Answer.King.db"));
        File.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, "db/Answer.King-log.db"));
        this.Client = new WebApplicationFactory<Program>().CreateClient();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        this.Client.Dispose();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
