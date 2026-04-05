using System.Data.Common;
using ADO.NET.DAL.Models;
using Dapper;

namespace ADO.NET.DAL;
//asdasdasda
public class DapperContextMy(DbConnection connection)
{
    private readonly DbConnection _connection = connection;
    
    
    // public IEnumerable<User> GetAllUsers()
    // {
    //     var result = new List<User>();
    //     _connection.Open();
    //
    //     const string sql = """
    //                         SELECT  id, name, is_driver
    //                         FROM table_users;
    //                        """;
    //
    //     var command = _connection.CreateCommand();
    //
    //     command.CommandText = sql;
    //
    //     var reader = command.ExecuteReader();
    //
    //     if (reader.HasRows)
    //     {
    //         while (reader.Read())
    //         {
    //             var id = reader.GetInt32("id");
    //             var isDriver = reader.GetBoolean("is_driver");
    //             var name = reader.GetString("name");
    //
    //             result.Add(new User()
    //             {
    //                 Id = id,
    //                 Name = name,
    //                 IsDriver = isDriver,
    //             });
    //         }
    //     }
    //     _connection.Close();
    //     return result;
    // }

    public IEnumerable<User> GetAllUsers()
    {
        var sql = """
                    SELECT id as Id, name as Name, is_driver as IsDriver 
                    FROM table_users;
                  """;
        return _connection.Query<User>(sql);
    }

    public IEnumerable<Product> GetAllProducts()
    {
        var sql = $"""
                  SELECT table_products.id as {nameof(Product.Id)},
                  item_name as {nameof(Product.Name)},
                  quantity as {nameof(Product.Quantity)},
                  price as {nameof(Product.Price)},
                  is_purchased as {nameof(Product.IsPurchased)},
                  table_users.name as  {nameof(Product.UserName)}
                  
                  FROM table_products
                      
                  LEFT JOIN table_users ON table_users.id = table_products.user_id;
                  """;
        return _connection.Query<Product>(sql);
    }

    public User? GetUserById(int userId)
    {
        var sql = $"""
                    SELECT id as {nameof(User.Id)}, name as {nameof(User.Name)}, is_driver as {nameof(User.IsDriver)} 
                    FROM table_users
                    WHERE id = @UserId;
                  """;
        return _connection.QuerySingle<User>(sql, new { UserId = userId });
    }

    public void InsertUser(User user)
    {
        var query = $"""
                     INSERT  INTO table_users
                     (name, is_driver)
                     VALUES (@{nameof(User.Name)}, @{nameof(User.IsDriver)});
                     """;
        _connection.Execute(query, user);
        
        // _connection.Execute(query, new
        // {
        //     Name = user.Name,
        //     IsDriver = user.IsDriver
        // });
    }

    public int InsertUsers(IEnumerable<User> users)
    {
        var query = $"""
                     INSERT  INTO table_users
                     (name, is_driver)
                     VALUES (@{nameof(User.Name)}, @{nameof(User.IsDriver)});
                     """;
        return _connection.Execute(query, users);
    }
}