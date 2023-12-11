namespace Answer.King.IntegrationTests.TestData;
using System.Collections.Generic;
using Answer.King.Api.RequestModels;

public class OrderData
{
    public required string Name;
    public required Order Order;
}

public static class OrdersData
{
    public static List<OrderData> AllOrders = new()
    {
        new OrderData()
        {
            Name = "Single_Line_Order",
            Order = new Order()
            {
                LineItems = new List<LineItem>()
                {
                    new LineItem()
                    {
                        ProductId = 1,
                        Quantity = 1,
                    },
                },
            },
        },

        new OrderData()
        {
            Name = "Multiple_Products_Order",
            Order = new Order()
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
            },
        },

        new OrderData()
        {
            Name = "Multiple_Same_Product_Multiple_Lines_Order",
            Order = new Order()
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
            },
        },

        new OrderData()
        {
            Name = "Multiple_Same_Product_Same_Line_Order",
            Order = new Order()
            {
                LineItems = new List<LineItem>()
                {
                    new LineItem()
                    {
                        ProductId = 5,
                        Quantity = 10,
                    },
                },
            },
        },

        new OrderData()
        {
            Name = "Fail_Retired_Product_Order",
            Order = new Order()
            {
                LineItems = new List<LineItem>()
                {
                   new LineItem()
                   {
                    ProductId = 3,
                    Quantity = 1,
                   },
                },
            },
        },

        new OrderData()
        {
            Name = "Fail_Invalid_Product_Id",
            Order = new Order()
            {
                LineItems = new List<LineItem>()
                {
                   new LineItem()
                   {
                    ProductId = 0,
                    Quantity = 5,
                   },
                },
            },
        },

        new OrderData()
        {
            Name = "Fail_No_Product_Line",
            Order = new Order()
            {
                LineItems = new List<LineItem>()
                {
                   new LineItem()
                   {
                    Quantity = 5,
                   },
                },
            },
        },

        new OrderData()
        {
            Name = "Fail_No_Quantity_Line",
            Order = new Order()
            {
                LineItems = new List<LineItem>()
                {
                   new LineItem()
                   {
                    ProductId = 2,
                   },
                },
            },
        },

        new OrderData()
        {
            Name = "Fail_No_Quantity",
            Order = new Order()
            {
                LineItems = new List<LineItem>()
                {
                   new LineItem()
                   {
                    ProductId = 2,
                    Quantity = 0,
                   },
                },
            },
        },

        new OrderData()
        {
            Name = "Fail_Negative_Quantity",
            Order = new Order()
            {
                LineItems = new List<LineItem>()
                {
                   new LineItem()
                   {
                    ProductId = 2,
                    Quantity = -100,
                   },
                },
            },
        },
    };
}
