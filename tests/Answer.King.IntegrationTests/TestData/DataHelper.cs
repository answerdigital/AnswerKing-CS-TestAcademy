namespace Answer.King.IntegrationTests.TestData;

using System;
using System.Linq;
using Answer.King.Api.RequestModels;

internal class DataHelper
{
    public static Order GetOrderData(string name)
    {
        var orderData = OrdersData.AllOrders.FirstOrDefault(x => x.Name.Contains(name)) ?? throw new Exception("Order data cannot be found");
        {
            return orderData.Order;
        }
    }
}
