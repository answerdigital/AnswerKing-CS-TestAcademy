using Answer.King.IntegrationTests.POCO;
using Answer.King.IntegrationTests.TestData;
using Answer.King.IntegrationTests.Utilities;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using Snapshooter.NUnit;
using static RestAssured.Dsl;

namespace Answer.King.IntegrationTests.Tests;
[TestFixture, Category("IntegrationTests")]
public class AnswerKingPostOrder : BaseTestClass
{
    [Parallelizable(scope: ParallelScope.Self)]
    [Test]
    public void GetAllOrders()
    {
        Given(this.Client)
            .When()
            .Get("https://localhost:44333/api/orders")
            .Then()
            .StatusCode(200);
    }

    [TestCase("Single_Line_Order", "5.99")]
    [TestCase("Multiple_Products_Order", "18.97")]
    [TestCase("Multiple_Same_Product_Multiple_Lines_Order", "8.97")]
    [TestCase("Multiple_Same_Product_Same_Line_Order", "99.9")]
    public void ProductOrder_Success(string name, string orderTotal)
    {
        var orderRequest = DataHelper.GetOrderData(name);

        JObject orderResponse = (JObject)Given(this.Client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(201)
        .DeserializeTo(typeof(JObject));

        Snapshot.Match(orderResponse, matchOptions => matchOptions
                                                        .IgnoreField("id")
                                                        .IgnoreField("createdOn")
                                                        .IgnoreField("lastUpdated"));

        orderResponse?.SelectToken("orderTotal")?.ToString().Should().Be(orderTotal);
    }


    [TestCase("Fail_Retired_Product_Order")]
    [TestCase("Fail_Invalid_Product_Id")]
    [TestCase("Fail_No_Product_Line")]
    [TestCase("Fail_No_Quantity_Line")]
    [TestCase("Fail_No_Quantity")]
    [TestCase("Fail_Negative_Quantity")]
    public void ProductOrder_Fail(string name)
    {
        var orderRequest = DataHelper.GetOrderData(name);

        var orderResponse = (ErrorResponse)Given(this.Client)
        .Body(orderRequest)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .DeserializeTo(typeof(ErrorResponse));

        Snapshot.Match(orderResponse);
    }

    [Test]
    public void ProductOrder_Fail_NoBody()
    {
        var response = (ErrorResponse)Given(this.Client)
        .When()
        .Post("https://localhost:44333/api/orders")
        .Then()
        .StatusCode(400)
        .DeserializeTo(typeof(ErrorResponse));

        Snapshot.Match(response);
    }
}
