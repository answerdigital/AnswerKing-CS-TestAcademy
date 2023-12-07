using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Answer.King.Api.RequestModels;
using Answer.King.IntegrationTests.Utilities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using static RestAssured.Dsl;
using Order = Answer.King.Api.RequestModels.Order;

namespace Answer.King.IntegrationTests.Tests;

[TestFixture]

public class AnswerKingPostOrder : BaseTestClass
//{
//    private WebApplicationFactory<Program> factory;
//    private HttpClient client;

//    [OneTimeSetUp]
//    public void Setup()
//    {
//        File.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, "db/Answer.King.db"));
//        File.Delete(Path.Combine(TestContext.CurrentContext.TestDirectory, "db/Answer.King-log.db"));
//        this.factory = new WebApplicationFactory<Program>();
//        this.client = this.factory.CreateClient();
//    }

//    [OneTimeTearDown]
//    public void TearDown()
//    {
//        this.client.Dispose();
//        this.factory.Dispose();
//    }

    [Test]

    public void GetAllOrdersRestAssured()
    {
        Given(this.client)
            .When()
            .Get("https://localhost:44333/api/orders")
            .Then()
            .StatusCode(200);
    }

    [Test]
    public void NewSingleProductOrderRestAssured()
    {
        var orderRequest = new Order()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem()
                {
                    ProductId = 1,
                    Quantity = 1,
                },
            },
        };

        var response = Given(this.client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(201)
        .DeserializeTo(typeof(Domain.Orders.Order));

        Console.WriteLine(response);

        // response.orderStatus.Should().Be("Created");
        // response.orderTotal. equals subtotals added...
        // response.lineItems[0].product.name.Should().Be("Fish");
        // response.lineItems[0].product.description.Should().Be("Delicious and satisfying.");
    }

    [Test]
    public void MultipleProductsOrderRestAssured()
    {
        var orderRequest = new Order()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem()
                {
                    ProductId = 1,
                    Quantity = 1,
                },
                new LineItem()
                {
                    ProductId = 2,
                    Quantity = 1,
                },
                new LineItem()
                {
                    ProductId = 5,
                    Quantity = 1,
                },
            },
        };

        var response = Given(this.client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(201)
        .Extract()
        .Body();

        response.Should().NotBeNull() // Response exists
            .And.ContainAll("Fish", "Chips", "Cheese Burger") // The ordered products
            .And.Contain("product", Exactly.Thrice()) // 3x product line items
            .And.Contain("subTotal", Exactly.Thrice()) // 3x line item sub totals
            .And.Contain("orderTotal", Exactly.Once()) // 1x order total
            .And.NotContain("CreateOrder"); // Method name should not be revealed
    }

    [Test]
    public void MultiSameProductDiffLinesOrderRestAssured()
    {
        var orderRequest = new Order()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem()
                {
                    ProductId = 2,
                    Quantity = 1,
                },
                new LineItem()
                {
                    ProductId = 2,
                    Quantity = 1,
                },
                new LineItem()
                {
                    ProductId = 2,
                    Quantity = 1,
                },
            },
        };

        var response = Given(this.client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(201)
        .Extract()
        .Body();

        response.Should().NotBeNull() // Response exists
            .And.ContainAll("Chips") // The ordered products
            .And.Contain("product", Exactly.Once()) // 1x product line items - chips combined to same line
            .And.Contain("subTotal", Exactly.Once()) // 1x line item sub totals - chips combined to same line
            .And.Contain("orderTotal", Exactly.Once()) // 1x order total
            .And.NotContain("createOrder"); // Method name should not be revealed
    }

    [Test]
    public void MultiSameProductSameLineOrderRestAssured()
    {
        var orderRequest = new Order()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem()
                {
                    ProductId = 10,
                    Quantity = 5,
                },
            },
        };

        var response = Given(this.client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(201)
        .Extract()
        .Body();

        response.Should().NotBeNull() // Response exists
            .And.ContainAll("Pepperoni") // The ordered products
            .And.Contain("product", Exactly.Once()) // 1x product line items
            .And.Contain("subTotal", Exactly.Once()) // 1x line item sub totals
            .And.Contain("orderTotal", Exactly.Once()) // 1x order total
            .And.NotContain("createOrder"); // Method name should not be revealed
    }

    [Test]
    public void InvalidRetiredProductRestAssured()
    {
        var orderRequest = new Order()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem()
                {
                    ProductId = 3,
                    Quantity = 1,
                },
            },
        };

        var response = Given(this.client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .Extract()
        .Body();

        response.Should().NotBeNull() // Response exists
            .And.ContainAll("error", "'product' must not be retired.") // The error message (don't know what it should be)
            .And.NotContain("createOrder"); // Method name should not be revealed
    }

    [Test]
    public void BlankOrderNoBodyRestAssured()
    {
        var response = Given(this.client)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .Extract()
        .Body();

        response.Should().NotBeNull() // Response exists
            .And.ContainAll("error", "A non-empty request body is required.") // The error message
            .And.NotContain("createOrder"); // Method name should not be revealed
    }

    [Test]
    public void InvalidProductIdRestAssured()
    {
        var orderRequest = new Order()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem()
                {
                    ProductId = 0,
                    Quantity = 5,
                },
            },
        };

        var response = Given(this.client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .Extract()
        .Body();

        response.Should().NotBeNull() // Response exists
            .And.ContainAll("error", "'product Id' must not be empty.") // The error message
            .And.NotContain("createOrder"); // Method name should not be revealed
    }

    [Test]
    public void InvalidNoProductLineRestAssured()
    {
        var orderRequest = new Order()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem()
                {
                    Quantity = 5,
                },
            },
        };

        var response = Given(this.client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .Extract()
        .Body();

        response.Should().NotBeNull() // Response exists
            .And.ContainAll("error", "'product Id' must not be empty.") // The error message
            .And.NotContain("createOrder"); // Method name should not be revealed
    }

    [Test]
    public void InvalidNoQuantityLineRestAssured()
    {
        var orderRequest = new Order()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem()
                {
                    ProductId = 6,
                },
            },
        };

        var response = Given(this.client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .Extract()
        .Body();

        response.Should().NotBeNull() // Response exists
            .And.ContainAll("error", "'quantity' must not be empty.") // The error message
            .And.NotContain("createOrder"); // Method name should not be revealed
    }

    [Test]
    public void InvalidNoQuantityRestAssured()
    {
        var orderRequest = new Order()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem()
                {
                    ProductId = 1,
                    Quantity = 0,
                },
            },
        };

        var response = Given(this.client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .Extract()
        .Body();

        response.Should().NotBeNull() // Response exists
            .And.ContainAll("error", "'quantity' must not be empty.") // The error message
            .And.NotContain("createOrder"); // Method name should not be revealed
    }

    [Test]
    public void InvalidNegativeQuantityRestAssured()
    {
        var orderRequest = new Order()
        {
            LineItems = new List<LineItem>()
            {
                new LineItem()
                {
                    ProductId = 1,
                    Quantity = -100,
                },
            },
        };

        var response = Given(this.client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .Extract()
        .Body();

        response.Should().NotBeNull() // Response exists
            .And.ContainAll("error", "'quantity' must be greater than or equal to '0'.") // The error message
            .And.NotContain("createOrder"); // Method name should not be revealed
    }

    // Revisit this if CI doesn't work
    //public void Dispose()
    //{
    //    GC.SuppressFinalize(this);
    //}

    // [Test]
    // public void SnapshotTest()
    // {
    //    var orderRequest = new Order()
    //    {
    //        LineItems = new List<LineItem>()
    //        {
    //            new LineItem()
    //            {
    //                ProductId = 1,
    //                Quantity = -100,
    //            },
    //        },
    //    };
    //    Snapshot.Match(orderRequest, matchOption => matchOption.IgnoreAllFields("Id"));
    //    ResponseFormatter.FormatResponse(
    //             Given(this.client)
    //                .Body(orderRequest)
    //                .When()
    //                .Post("https://localhost:44333/api/orders")
    //                .Then()
    //                .AssertThat()
    //                .StatusCode(400)
    //                .Extract()
    //                .Body());
    // }
}




// [TestCase("MultiOrder", 200)]
// [TestCase("OrderMissingProductId", 400)]
// public void PostOrder(string testName, int status)
// {
//    var orderObject = GetTestData(testname);
//    var orderRequest = new Order()
//    {
//        LineItems = new List<LineItem>()
//        {
//            new LineItem()
//            {
//                ProductId = 1,
//                Quantity = 1,
//            },
//            new LineItem()
//            {
//                ProductId = 2,
//                Quantity = 1,
//            },
//            new LineItem()
//            {
//                ProductId = 2,
//                Quantity = 1,
//            },
//        },
//    };
//    var response = Given(this.client)
//    .Body(orderRequest)
//    .When()
//    .Post("https://localhost:44333/api/orders")
//    .Then()
//    .StatusCode(status)
//    .Extract()
//    .Body();

// // Assert response is correct
// }
