using System.Data;
using System.Data.Common;
using ADO.NET.DAL.Models;
using Npgsql;

namespace ADO.NET.DAL;

public class ConnectedContext(DbConnection connection)
{
    private readonly DbConnection _connection = connection;

    public IEnumerable<Product> GetAllProducts()
    {
        var result = new List<Product>();
        _connection.Open();
        // const string sql = """
        //                     SELECT table_products.id AS id, price, item_name, quantity, is_purchased, name
        //                     FROM table_products
        //                     INNER JOIN table_users ON  table_users.id = table_products.user_id
        //                    """;

        const string sql = """
                            SELECT table_products.id AS id, price, item_name, quantity, is_purchased, name
                            FROM table_products
                            LEFT JOIN table_users ON  table_users.id = table_products.user_id
                           """;
        
        var command = _connection.CreateCommand();
        
        command.CommandText = sql;
        
        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                var id = reader.GetInt32("id");
                var itemName = reader.GetString("item_name");
                var quantity = reader.GetString("quantity");
                var price = reader.GetDecimal("price");
                var isPurchased = reader.GetBoolean("is_purchased");
                
                
                //var userName = reader.GetString("name");
                var userName = reader.IsDBNull("name") ? "Еще никто не взялся!" : reader.GetString("name");
                
                result.Add(new Product()
                {
                    Id = id,
                    IsPurchased = isPurchased,
                    UserName = userName,
                    Name = itemName,
                    Quantity = quantity,
                    Price = price,
                });
            }
        }
        _connection.Close();
        return result;
    }

    public void InsertNewUser(User user)
    {
        _connection.Open();

        var command = _connection.CreateCommand();
        command.CommandText = $"""
                                INSERT INTO table_users (name, is_driver)
                                VALUES ('{user.Name}', {user.IsDriver})
                              """;
        command.ExecuteNonQuery();
        Console.WriteLine($"Пользователь {user.Name}, {user.IsDriver} добавлен");
        _connection.Close();
    }
    public void InsertNewUserWithParameters(User user)
    {
        _connection.Open();

        var command = _connection.CreateCommand();
        command.CommandText = """
                                 INSERT INTO table_users (name, is_driver)
                                 VALUES (@name, @is_driver)
                               """;
        
        var userNameParameter = command.CreateParameter();
        userNameParameter.ParameterName = "@name";
        userNameParameter.Value = user.Name;
        command.Parameters.Add(userNameParameter);
        
        
        var isDriverParameter = command.CreateParameter();
        isDriverParameter.ParameterName = "@is_driver";
        isDriverParameter.Value = user.IsDriver;
        command.Parameters.Add(isDriverParameter);
        
        
        
        command.ExecuteNonQuery();
        Console.WriteLine($"Пользователь {user.Name}, {user.IsDriver} добавлен");
        _connection.Close();
    }
    
    public IEnumerable<User> GetAllUsers()
    {
        var result = new List<User>();
        _connection.Open();
        
        const string sql = """
                            SELECT  id, name, is_driver
                            FROM table_users;
                           """;
        
        var command = _connection.CreateCommand();
        
        command.CommandText = sql;
        
        var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            while (reader.Read())
            {
                var id = reader.GetInt32("id");
                var isDriver = reader.GetBoolean("is_driver");
                var name = reader.GetString("name");
                
                result.Add(new User()
                {
                    Id = id,
                    Name = name,
                    IsDriver = isDriver,
                });
            }
        }
        _connection.Close();
        return result;
    }

}