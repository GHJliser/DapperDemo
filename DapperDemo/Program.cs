using Dapper;
using DapperDemo.DatabaseModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DapperDemo
{
    internal class Program
    {
        public const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DapperDemo;Integrated Security=True;Persist Security Info=False;TrustServerCertificate=True";
        static void Main(string[] args)
        {
            //第一次呼叫使用
            //InitialDatabase();

            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                List<Product> products = dbConnection.Query<Product>("SELECT * FROM Product").ToList();
                Console.WriteLine(JsonConvert.SerializeObject(products, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented
                }));
            }
        }

        private static void InitialDatabase()
        {
            InitialProductType();
            InitialProduct();
        }

        private static void InitialProductType()
        {
            int dataSize = 10;
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                for (int i = 0; i < dataSize; i++)
                {
                    dbConnection.Execute("INSERT INTO ProductType VALUES(@Id, @Name, @Memo)", new ProductType { Id = i, Name = $"test{i}", Memo = $"memo{i}" });
                }
            }
        }
        private static void InitialProduct()
        {
            int dataSize = 10;
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();

                List<ProductType> types = dbConnection.Query<ProductType>("SELECT * FROM ProductType").ToList();
                for (int i = 0; i < dataSize; i++)
                {
                    dbConnection.Execute("INSERT INTO Product(Id, ProductId, Label, Price) VALUES(@Id, @ProductId, @Label, @Price)", new Product
                    {
                        Id = i,
                        ProductId = types[new Random().Next(types.Count)].Id,
                        Label = $"test{i}",
                        Price = new Random().Next(100)
                    });
                }
            }
        }
    }
}
