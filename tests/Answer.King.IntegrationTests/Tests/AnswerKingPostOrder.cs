using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Xml.Linq;
using Answer.King.Api.RequestModels;
using Answer.King.Domain.Orders;
using Answer.King.IntegrationTests.TestData;
using Answer.King.IntegrationTests.Utilities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Snapshooter.NUnit;
using static RestAssured.Dsl;
using Order = Answer.King.Api.RequestModels.Order;

namespace Answer.King.IntegrationTests.Tests;

[TestFixture]

public class AnswerKingPostOrder : BaseTestClass

{
    [Test]
    public void GetAllOrders()
    {
        Given(this.Client)
            .When()
            .Get("https://localhost:44333/api/orders")
            .Then()
            .StatusCode(200);
    }

    [TestCase("Single_Line_Order")]
    [TestCase("Multiple_Products_Order")]
    [TestCase("Multiple_Same_Product_Multiple_Lines_Order")]
    [TestCase("Multiple_Same_Product_Same_Line_Order")]
    public void ProductOrder_Success(string name)
    {
        var orderRequest = DataHelper.GetOrderData(name);

        var orderResponse = (Domain.Orders.Order)Given(this.Client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(201)
        .DeserializeTo(typeof(Domain.Orders.Order));

        Snapshot.Match(orderResponse, matchOptions => matchOptions
                                                        .IgnoreField("Id")
                                                        .IgnoreField("CreatedOn")
                                                        .IgnoreField("LastUpdated"));
    }

    [TestCase("Fail_Retired_Product_Order")]
    [TestCase("Fail_Invalid_Product_Id")]
    [TestCase("Fail_No_Product_Line")]
    [TestCase("Fail_No_Quantity_Line")]
    [TestCase("Fail_No_Quantity")]
    [TestCase("Fail_Negative_Quantity")]
    public void FailedOrders(string name)
    {
        var orderRequest = DataHelper.GetOrderData(name);

        var orderResponse = (ProblemDetails)Given(this.Client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .DeserializeTo(typeof(ProblemDetails));

        Snapshot.Match(orderResponse, matchOptions => matchOptions
                                                        .IgnoreField("TraceId"));
    }

    [Test]
    public void FailedOrder_NoBody()
    {
        var response = (ProblemDetails)Given(this.Client)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .DeserializeTo(typeof(ProblemDetails));

        Snapshot.Match(response, matchOptions => matchOptions
                                                        .IgnoreField("TraceId"));
    }


    //public void InvalidProductIdRestAssured()
    //    response.Should().NotBeNull() // Response exists
    //        .And.ContainAll("error", "'product Id' must not be empty.") // The error message
    //        .And.NotContain("createOrder"); // Method name should not be revealed
    //}

    //public void InvalidNoProductLineRestAssured()
    //    response.Should().NotBeNull() // Response exists
    //        .And.ContainAll("error", "'product Id' must not be empty.") // The error message
    //        .And.NotContain("createOrder"); // Method name should not be revealed


    //public void InvalidNoQuantityLineRestAssured()
    //    response.Should().NotBeNull() // Response exists
    //        .And.ContainAll("error", "'quantity' must not be empty.") // The error message
    //        .And.NotContain("createOrder"); // Method name should not be revealed



    //public void InvalidNoQuantityRestAssured()

    //    response.Should().NotBeNull() // Response exists
    //        .And.ContainAll("error", "'quantity' must not be empty.") // The error message
    //        .And.NotContain("createOrder"); // Method name should not be revealed


    //public void InvalidNegativeQuantityRestAssured()

    //    response.Should().NotBeNull() // Response exists
    //        .And.ContainAll("error", "'quantity' must be greater than or equal to '0'.") // The error message
    //        .And.NotContain("createOrder"); // Method name should not be revealed

}
