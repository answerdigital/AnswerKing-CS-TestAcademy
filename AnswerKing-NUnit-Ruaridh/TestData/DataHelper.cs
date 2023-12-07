namespace Answer.King.IntegrationTests.TestData;

using System;
using System.Linq;
using Answer.King.Api.RequestModels;

internal class DataHelper
{
    public static Order GetOrderData(string name)
    {
#pragma warning disable CA2201 // Do not raise reserved exception types
        var orderData = OrdersData.AllOrders.FirstOrDefault(x => x.Name.Contains(name)) ?? throw new Exception("Order data cannot be found");
#pragma warning restore CA2201 // Do not raise reserved exception types
        {
            return orderData.Order;
        }
    }
}
