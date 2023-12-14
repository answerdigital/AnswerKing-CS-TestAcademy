namespace Answer.King.IntegrationTests.Utilities;
using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class BaseTestClass : IDisposable
{
    public HttpClient Client;

    public string testDb { get; set; } = $"Answer.King.{Guid.NewGuid()}.db";

    [SetUp]
    public void Setup()
    {
        this.Client = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseSetting("ConnectionStrings:AnswerKing", $"filename=db/{this.testDb};Connection=Shared");
        }).CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        if (Directory.Exists(Path.Combine(TestContext.CurrentContext.TestDirectory, "db")))
        {
            File.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, $"db/{testDb}"));
            File.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, "db/Answer.King-log.db"));
        }
        this.Client.Dispose();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
