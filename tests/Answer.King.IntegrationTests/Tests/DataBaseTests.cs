namespace Answer.King.IntegrationTests.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Npgsql;

//The tests in here are mocked data to practice how I could do DB testing and assertions with a working SQL DB

[TestFixture]
public class BeeKeeperDBTests
{
    [Test]
    public async Task OrderAssertionsProductId()
    {

        var connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=penguin;Database=postgres;";
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using var command = dataSource.CreateCommand("SELECT product_id from public.orders WHERE order_id = 4");
        await using var reader = await command.ExecuteReaderAsync();

        List<Int32> resultList = new List<Int32>();

        while (await reader.ReadAsync())
        {
            var result = reader.GetInt32(0);
            resultList.Add(result);
        }

        string resultString = string.Join(", ", resultList);

        //Assertions that can be done on a int list        
        resultList.Count().Should().Be(3);

        //Assertions that can't be done on a list
        resultString.Should().ContainAll("1", "2", "5");
        Console.WriteLine(resultString);
    }

    [Test]
    public async Task OrderAssertionsProductName()
    {

        var connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=penguin;Database=postgres;";
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using var command = dataSource.CreateCommand("SELECT pro_name from public.orders WHERE order_id = 4");
        await using var reader = await command.ExecuteReaderAsync();

        List<string> resultList = new List<string>();

        while (await reader.ReadAsync())
        {
            var result = reader.GetString(0);
            resultList.Add(result);
        }

        string resultString = string.Join(", ", resultList);

        //Assertions that can be done on a list
        resultList.Should().Contain("Fish", "Chips", "Cheese Burger");
        resultList.Should().NotContain("Gravy");

        //Assertions that can't be done on a list
        Console.WriteLine(resultString);
    }

    [Test]
    public async Task OrderAssertionsProductQuantity()
    {

        var connectionString = "Server=localhost;Port=5432;User Id=postgres;Password=penguin;Database=postgres;";
        await using var dataSource = NpgsqlDataSource.Create(connectionString);
        await using var command = dataSource.CreateCommand("SELECT quantity from public.orders WHERE order_id = 4");
        await using var reader = await command.ExecuteReaderAsync();

        List<int> resultList = new List<int>();

        while (await reader.ReadAsync())
        {
            var result = reader.GetInt32(0);
            resultList.Add(result);
            result.Should().Be(1);
        }

        string resultString = string.Join(", ", resultList);

        //Assertions that can be done on a list
        resultList.Count().Should().Be(3);

        //Assertions that can't be done on a list
        resultString.Should().Contain("1", Exactly.Thrice());
        Console.WriteLine(resultString);
    }
}
